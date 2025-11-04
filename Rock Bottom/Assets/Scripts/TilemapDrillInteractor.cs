using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDrillInteractor : MonoBehaviour
{
    public Tilemap tilemap;
    public Camera cam;
    public AudioSource sfx;

    [Header("Data")]
    public DrillCostsConfig costs = new();
    public int startOil = 50;

    private DrillContext ctx;

    void Awake()
    {
        if (!cam) cam = Camera.main;
        ctx = new DrillContext(sfx, startOil, costs);
        ctx.onMined += (t, c) => Debug.Log($"Mined {t} @ {c}. Oil={ctx.GetOil()}. Score={ctx.GetScore()}");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var world = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cell = tilemap.WorldToCell(world);
            var tileBase = tilemap.GetTile(cell) as ResourceTile;
            if (tileBase != null)
            {
                tileBase.HandleDig(cell, tilemap, ctx);
            }
        }
    }
}

public class DrillContext
{
    public Action<ResourceType, Vector3Int> onMined;

    private AudioSource sfx;
    private int oil;
    private int score;
    private DrillCostsConfig costs = new();

    public DrillContext(AudioSource sfx, int startOil, DrillCostsConfig costs)
    {
        this.sfx = sfx; oil = startOil; this.costs = costs;
    }

    public int GetOil() => oil;
    public int GetScore() => score;
    public void ModOil(int delta) { oil = Mathf.Max(0, oil + delta); }
    public void AddScore(int s) { score += s; }
    public int GetCostFor(ResourceType t) => costs.CostFor(t);
    public void PlayOneShot(AudioClip clip) { if (clip && sfx) sfx.PlayOneShot(clip); }
    public void RaiseMined(ResourceType t, Vector3Int cell) => onMined?.Invoke(t, cell);

}

// ！！ Interaction context: Provided by the drill rig/player script ！！
[Serializable]
public class DrillCostsConfig
{
    public int dirt = 2;
    public int rock = 12;
    public int iron = 4;
    public int gold = 8;
    public int oilGain = 10; // Oil's positive response

    public int CostFor(ResourceType k)
    {
        switch (k)
        {
            case ResourceType.Dirt: return dirt;
            case ResourceType.Rock: return rock;
            case ResourceType.Iron: return iron;
            case ResourceType.Gold: return gold;
            case ResourceType.Oil: return -oilGain; // Negative values indicate refuelling.
            default: return 1;
        }
    }
}