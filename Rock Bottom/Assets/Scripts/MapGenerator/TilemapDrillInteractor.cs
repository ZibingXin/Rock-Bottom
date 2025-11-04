using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDrillInteractor : MonoBehaviour
{
    public Tilemap tilemap;
    public AudioSource sfx;

    [Header("Data")]
    public DrillCostsConfig costs = new();
    public DrillWorthConfig worth = new();
    public PlayerStats playerStats;

    private DrillContext ctx;
    private Vector3Int cell;

    void Start()
    {
        ctx = new DrillContext(sfx, playerStats.CurrentOil, costs, worth);
        ctx.onMined += (t, c) =>
        {
            if (t == ResourceType.Oil)
            {
                playerStats.RefillOil(costs.oilGain);
            }
            else
            {
                if (playerStats.CurrentOil < costs.CostFor(t)) return;
                playerStats.BurnOil(costs.CostFor(t));
                playerStats.AddMoney(worth.WorthFor(t));
            }
            var tileBase = tilemap.GetTile(c) as ResourceTile;
            if (tileBase != null)
            {
                tileBase.HandleDig(c, tilemap, ctx);
            }
            Debug.Log($"Step to {t} {c}, Oil: {playerStats.CurrentOil} / 100, Money: {playerStats.CurrentMoney}");
        };
    }

    public DrillContext GetContext() => ctx;
}

public class DrillContext
{
    public Action<ResourceType, Vector3Int> onMined;

    private AudioSource sfx;
    private int oil;
    private int money;
    private DrillCostsConfig costs = new();
    private DrillWorthConfig worth = new();

    public DrillContext(AudioSource sfx, int startOil, DrillCostsConfig costs, DrillWorthConfig worth)
    {
        this.sfx = sfx; oil = startOil; this.costs = costs; this.worth = worth;
    }

    public void PlayOneShot(AudioClip clip) { if (clip && sfx) sfx.PlayOneShot(clip); }
    public void RaiseMined(ResourceType t, Vector3Int cell) => onMined?.Invoke(t, cell);

}

// ！！ Interaction context: Provided by the drill rig/player script ！！
[Serializable]
public class DrillCostsConfig
{
    public int dirt = 1;
    public int rock = 10;
    public int iron = 3;
    public int gold = 6;
    public int oilGain = 10; // Oil's positive response

    public DrillCostsConfig() { }
    public int CostFor(ResourceType k)
    {
        return k switch
        {
            ResourceType.Dirt => dirt,
            ResourceType.Rock => rock,
            ResourceType.Iron => iron,
            ResourceType.Gold => gold,
            ResourceType.Oil => -oilGain,// Negative values indicate refuelling.
            _ => 1,
        };
    }
}

[Serializable]
public class DrillWorthConfig
{
    public int dirt = 1;
    public int rock = 0;
    public int iron = 5;
    public int gold = 20;
    public int oil = 0;

    public DrillWorthConfig() { }
    public int WorthFor(ResourceType k)
    {
        return k switch
        {
            ResourceType.Dirt => dirt,
            ResourceType.Rock => rock,
            ResourceType.Iron => iron,
            ResourceType.Gold => gold,
            ResourceType.Oil => oil,
            _ => 0,
        };
    }
}