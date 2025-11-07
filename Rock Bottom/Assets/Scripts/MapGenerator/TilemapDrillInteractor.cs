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
                //if (playerStats.CurrentOil < costs.CostFor(ResourceType.Dirt)) 
                if (playerStats.CurrentOil < playerStats.FinalOilCost(costs.CostFor(t))) 
                { 
                    return;
                }
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
    private float oil;
    private int money;
    private DrillCostsConfig costs = new();
    private DrillWorthConfig worth = new();

    public DrillContext(AudioSource sfx, float startOil, DrillCostsConfig costs, DrillWorthConfig worth)
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
    public float dirt = 1;
    public float rock = 10;
    public float iron = 3;
    public float gold = 6;
    public float oilGain = 10; // Oil's positive response

    public DrillCostsConfig() { }
    public float CostFor(ResourceType k)
    {
        return k switch
        {
            ResourceType.Dirt => dirt,
            ResourceType.Rock => rock,
            ResourceType.Iron => iron,
            ResourceType.Gold => gold,
            //ResourceType.Oil => -oilGain,// Negative values indicate refuelling.
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

    public PlayerStats playerStats;

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

    public void UpdateDrillWorth()
    {
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStats reference not set in DrillWorthConfig.");
            return;
        }

        int level = playerStats.DrillWorthLv;

        dirt = 0 + (level - 1) * 10;
        rock = 0;
        iron = 200 + (level - 1) * 20;
        gold = 2000 + (level - 1) * 50;
        oil = 0;

        playerStats.SetDrillWorthLv(level);
        PlayerPrefs.SetInt("DrillWorthLv", level);
    }
}