using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDrillInteractor : MonoBehaviour
{
    public Tilemap tilemap;
    public Camera cam;
    public AudioSource sfx;

    [Header("Data")]
    public DrillCostsConfig costs = new DrillCostsConfig();
    public int startOil = 50;
    public int damagePerHit = 1;

    private DrillContext ctx;

    void Awake()
    {
        if (!cam) cam = Camera.main;
        ctx = new DrillContext(sfx, startOil, costs, damagePerHit);
        ctx.onMined += (k, c) => Debug.Log($"Mined {k} @ {c}. Oil={ctx.GetOil()}");
        ctx.onHit += (k, c, r) => Debug.Log($"Hit {k} @ {c}. RemainHP={r} Oil={ctx.GetOil()}");
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
                tileBase.HandleHit(cell, tilemap, ctx);
            }
        }
    }
}
