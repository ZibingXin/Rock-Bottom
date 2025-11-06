using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum TileType { Dirt, Rock, Iron, Gold, Oil }

[Serializable]
public class Weights
{
    [Range(0, 100)] public int Dirt = 50;
    [Range(0, 100)] public int Rock = 20;
    [Range(0, 100)] public int Iron = 20;
    [Range(0, 100)] public int Gold = 5;
    [Range(0, 100)] public int Oil = 5;

    public int Sum => Dirt + Rock + Iron + Gold + Oil;
}

[Serializable]
public class Stratum
{
    [Tooltip("Inclusive depth intervals, closed intervals. For example: 0¨C19, 20¨C39, ...")]
    public int startY = 0;
    public int endY = 19;
    public Weights weights = new Weights();

    [Header("Clustering parameters/per 20-stratum")]
    [Tooltip("Within this stratum, the number of iron ore clusters (centroid count)")] 
    public int ironClusters = 3;
    [Tooltip("Within this stratum, the number of gold ore clusters")] 
    public int goldClusters = 2;
    [Tooltip("Within this stratum, the number of oil clusters")] 
    public int oilClusters = 2;

    [Tooltip("Target size range for iron ore clusters")] 
    public Vector2Int ironClusterSize = new Vector2Int(6, 12);
    [Tooltip("Target size range for gold ore clusters")] 
    public Vector2Int goldClusterSize = new Vector2Int(3, 7);
    [Tooltip("Target size range for oil clusters")] 
    public Vector2Int oilClusterSize = new Vector2Int(2, 5);

    [Range(0f, 1f), Tooltip("Decaying outward from the centre (the smaller, the more compact)")] 
    public float falloff = 0.6f;
}

public class MapGenerator : MonoBehaviour
{
    [Header("Map Size")]
    [Min(1)] public int width = 10;
    [Min(1)] public int height = 100;

    [Header("Randomness and Playability Constraints")]
    public int seed = 12345;
    [Tooltip("Ensure that at least one reachable oil block appears every N layers (if none exists, force injection).")] 
    public int ensureOilEveryNLevels = 10;
    [Tooltip("If an entire row consists solely of rock, force one to change to soil to prevent a deadlock caused by an impenetrable wall.")] 
    public bool ensureRowNonBlocking = true;

    [Header("Layered configuration")]
    public List<Stratum> strata = new List<Stratum>();

    // Generate result (row-major: y rows, x columns)
    private TileType[,] grid;
    public TileType[,] Grid => grid; // For reading by other systems

    System.Random rng;

    void Reset()
    {
        // When clicking Reset in the Inspector, the five stratum weights specified by the question author are automatically populated.
        strata = BuildDefaultStrata();
    }

    void Awake()
    {
        if (strata == null || strata.Count == 0)
            strata = BuildDefaultStrata();
    }

    [ContextMenu("Generate Now")]
    public void GenerateNow(int seed = 12345)
    {
        grid = Generate(seed, strata, width, height);
    }

    public TileType[,] Generate(int seedValue, List<Stratum> s, int w, int h)
    {
        rng = new System.Random(seedValue);
        grid = new TileType[w, h];

        // 1) Basic filling: perform block-wise sampling according to the weight of each stratum
        for (int y = 0; y < h; y++)
        {
            var st = FindStratumForY(s, y);
            for (int x = 0; x < w; x++)
                grid[x, y] = SampleByWeights(st.weights);
        }

        // 2) Commodity groups: iron ore, gold ore, oil
        foreach (var st in s)
        {
            GrowClustersForRange(st, w, h, TileType.Iron, st.ironClusters, st.ironClusterSize, st.falloff);
            GrowClustersForRange(st, w, h, TileType.Gold, st.goldClusters, st.goldClusterSize, st.falloff);
            GrowClustersForRange(st, w, h, TileType.Oil, st.oilClusters, st.oilClusterSize, st.falloff);
        }

        // 3) Playability adjustments
        if (ensureOilEveryNLevels > 0)
            EnsureOilEveryBand(ensureOilEveryNLevels);

        if (ensureRowNonBlocking)
            EnsureEachRowHasNonRock();

        // 4) Add two columns of rock on the left and right as boundaries
        var extendedGrid = new TileType[w + 2, h];
        for (int y = 0; y < h; y++)
        {
            extendedGrid[0, y] = TileType.Rock;
            extendedGrid[w + 1, y] = TileType.Rock;
            for (int x = 0; x < w; x++)
                extendedGrid[x + 1, y] = grid[x, y];
        }

        return extendedGrid;
    }

    // ¡ª¡ª Utility Functions ¡ª¡ª
    Stratum FindStratumForY(List<Stratum> s, int y)
    {
        for (int i = 0; i < s.Count; i++)
            if (y >= s[i].startY && y <= s[i].endY)
                return s[i];
        return s[s.Count - 1];
    }

    TileType SampleByWeights(Weights w)
    {
        int r = rng.Next(0, Math.Max(1, w.Sum));
        int acc = 0;
        if ((acc += w.Dirt) > r) return TileType.Dirt;
        if ((acc += w.Rock) > r) return TileType.Rock;
        if ((acc += w.Iron) > r) return TileType.Iron;
        if ((acc += w.Gold) > r) return TileType.Gold;
        return TileType.Oil;
    }

    void GrowClustersForRange(Stratum st, int w, int h, TileType target, int clusters, Vector2Int sizeRange, float falloff)
    {
        if (clusters <= 0) return;
        int minY = Mathf.Clamp(st.startY, 0, h - 1);
        int maxY = Mathf.Clamp(st.endY, 0, h - 1);
        if (minY > maxY) return;

        for (int i = 0; i < clusters; i++)
        {
            int cx = rng.Next(0, w);
            int cy = rng.Next(minY, maxY + 1);
            int targetSize = rng.Next(sizeRange.x, sizeRange.y + 1);
            FloodGrow(cx, cy, target, targetSize, falloff, w, h);
        }
    }

    void FloodGrow(int sx, int sy, TileType target, int targetSize, float falloff, int w, int h)
    {
        Queue<(int x, int y)> q = new Queue<(int x, int y)>();
        HashSet<(int x, int y)> visited = new HashSet<(int x, int y)>();
        q.Enqueue((sx, sy));
        visited.Add((sx, sy));

        int placed = 0;
        while (q.Count > 0 && placed < targetSize)
        {
            var (x, y) = q.Dequeue();
            grid[x, y] = target;
            placed++;

            foreach (var (nx, ny) in Neigh4(x, y, w, h))
            {
                if (visited.Contains((nx, ny))) continue;
                float dist = Mathf.Abs(nx - sx) + Mathf.Abs(ny - sy);
                float p = Mathf.Pow(falloff, dist); // The further from the centre, the less likely it is to grow.
                if (rng.NextDouble() < p)
                {
                    q.Enqueue((nx, ny));
                }
                visited.Add((nx, ny));
            }
        }
    }

    IEnumerable<(int x, int y)> Neigh4(int x, int y, int w, int h)
    {
        if (x > 0) yield return (x - 1, y);
        if (x < w - 1) yield return (x + 1, y);
        if (y > 0) yield return (x, y - 1);
        if (y < h - 1) yield return (x, y + 1);
    }

    void EnsureOilEveryBand(int band)
    {
        int h = height;
        int w = width;
        for (int y0 = 0; y0 < h; y0 += band)
        {
            int y1 = Mathf.Min(h - 1, y0 + band - 1);
            bool hasOil = false;
            for (int y = y0; y <= y1 && !hasOil; y++)
                for (int x = 0; x < w && !hasOil; x++)
                    if (grid[x, y] == TileType.Oil) hasOil = true;

            if (!hasOil)
            {
                // Select one square of clay/iron/gold within this zone and replace it with oil (clay takes precedence).
                List<(int x, int y)> candidates = new List<(int x, int y)>();
                for (int y = y0; y <= y1; y++)
                    for (int x = 0; x < w; x++)
                        if (grid[x, y] != TileType.Rock) candidates.Add((x, y));
                if (candidates.Count == 0)
                {
                    // In extreme cases, where the entire rock mass is affected, a one-grade change shall be enforced.
                    int rx = rng.Next(0, w);
                    int ry = rng.Next(y0, y1 + 1);
                    grid[rx, ry] = TileType.Oil;
                }
                else
                {
                    var pick = candidates[rng.Next(0, candidates.Count)];
                    grid[pick.x, pick.y] = TileType.Oil;
                }
            }
        }
    }

    void EnsureEachRowHasNonRock()
    {
        for (int y = 0; y < height; y++)
        {
            bool allRock = true;
            for (int x = 0; x < width; x++)
                if (grid[x, y] != TileType.Rock) { allRock = false; break; }
            if (allRock)
            {
                int rx = rng.Next(0, width);
                grid[rx, y] = TileType.Dirt;
            }
        }
    }

    List<Stratum> BuildDefaultStrata()
    {
        var list = new List<Stratum>();

        list.Add(new Stratum { startY = 0, endY = 19, weights = new Weights { Dirt = 70, Rock = 10, Iron = 18, Gold = 0, Oil = 2 } });
        list.Add(new Stratum { startY = 20, endY = 39, weights = new Weights { Dirt = 60, Rock = 10, Iron = 25, Gold = 3, Oil = 2 } });
        list.Add(new Stratum { startY = 40, endY = 59, weights = new Weights { Dirt = 50, Rock = 25, Iron = 16, Gold = 6, Oil = 3 } });
        list.Add(new Stratum { startY = 60, endY = 79, weights = new Weights { Dirt = 40, Rock = 25, Iron = 13, Gold = 9, Oil = 3 } });
        list.Add(new Stratum { startY = 80, endY = 99, weights = new Weights { Dirt = 40, Rock = 25, Iron = 10, Gold = 12, Oil = 3 } });

        return list;
    }
}
