using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDrillInteractor : MonoBehaviour
{
    public Tilemap tilemap;
    public AudioSource sfx;

    [Header("Data")]
    public DrillCostsConfig costs = new();
    public PlayerStats playerStats;

    private DrillContext ctx;
    private Vector3Int cell;

    void Awake()
    {
        ctx = new DrillContext(sfx, playerStats.CurrentOil, costs);
        ctx.onMined += (t, c) => { 
            DrillConfirm(t, playerStats.CurrentOil);
            //Debug.Log($"Mined {t} @ {c}. Oil={ctx.GetOil()}. Score={ctx.GetScore()}"); 
        };
    }

    private void DrillConfirm(ResourceType t, int currentOil)
    {
        if (t != ResourceType.Oil)
        {
            switch (t)
            {
                case ResourceType.Dirt:
                    if (currentOil < costs.dirt) return;
                    playerStats.BurnOil(costs.dirt);
                    
                    playerStats.AddMoney(1);
                    break;
                case ResourceType.Rock:
                    if (currentOil < costs.rock) return;
                    playerStats.BurnOil(costs.rock);
                    break;
                case ResourceType.Iron:
                    if (currentOil < costs.iron) return;
                    playerStats.BurnOil(costs.iron);
                    break;
                case ResourceType.Gold:
                    if (currentOil < costs.gold) return;
                    playerStats.BurnOil(costs.gold);
                    break;
            }  
        }
        else if (t == ResourceType.Oil)
        {
            playerStats.RefillOil(costs.oilGain);
        }

        var tileBase = tilemap.GetTile(cell) as ResourceTile;
        if (tileBase != null)
        {
            tileBase.HandleDig(cell, tilemap, ctx);
        }
    }

    public void GetTile()
    {

    }

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    var world = cam.ScreenToWorldPoint(Input.mousePosition);
        //    Vector3Int cell = tilemap.WorldToCell(world);
        //    var tileBase = tilemap.GetTile(cell) as ResourceTile;
        //    if (tileBase != null)
        //    {
        //        tileBase.HandleDig(cell, tilemap, ctx);
        //    }
        //}
    }
}

public class DrillContext
{
    public Action<ResourceType, Vector3Int> onMined;

    private AudioSource sfx;
    private int oil;
    private int money;
    private DrillCostsConfig costs = new();

    public DrillContext(AudioSource sfx, int startOil, DrillCostsConfig costs)
    {
        this.sfx = sfx; oil = startOil; this.costs = costs;
    }

    public int GetOil() => oil;
    public int GetScore() => money;
    public void ModOil(int delta) { oil = Mathf.Max(0, oil + delta); }
    public void AddMoney(int s) { money += s; }
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