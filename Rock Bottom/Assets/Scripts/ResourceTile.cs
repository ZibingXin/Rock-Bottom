using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;


/*
 * ResourceTile template (inherits from TileBase)
 * - Five block types: Dirt / Rock / Iron / Gold / Oil
 * - Supports: Durability, mining events, drop/refuel, rare light effects (Prefab, optional URP 2D Light or emitting Sprite)
 * - Usage: Create one ScriptableObject resource per resource type (Create > Tiles > ResourceTile),
 *         Use within the Tile Palette; Call HandleHit() via Drill logic at runtime.
*/

public enum ResourceKind { Dirt, Rock, Iron, Gold, Oil }

[CreateAssetMenu(menuName = "Tiles/ResourceTile", fileName = "ResourceTile")]
public class ResourceTile : TileBase
{
    [Header("Appearance and Data")]
    public ResourceKind kind;
    public Sprite sprite;
    [Tooltip("Dig away the required durability. Rock can yield higher values.")]
    public int durability = 1;

    [Header("Resource Effect (Takes effect upon completion of mining)")]
    public int oilDeltaOnMined = 0; // Oil may be set to +10, with standard blocks configured to be consumed during drilling calculations.
    public int scoreOnMined = 0;

    [Header("Feedback (optional)")]
    public AudioClip mineSfx;
    public GameObject mineVfxPrefab; // Particle or small explosion
    [Tooltip("Rare Glow: Assign a glow Prefab (containing Light2D or a glow Sprite) to Gold/Oil.")]
    public GameObject glowPrefab;

    // ！！ TileBase Interface ！！
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref UnityEngine.Tilemaps.TileData tileData)
    {
        tileData.sprite = sprite;
        tileData.color = Color.white; // The tint may be adjusted according to rarity.
        tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
        tileData.colliderType = Tile.ColliderType.None; // Blocking by excavation logic control
        tileData.gameObject = glowPrefab; // This Prefab will be instantiated on the TilemapRenderer (only when "Has GameObject" is enabled).
    }

    // ！！ Runtime: Invoked by external interactive scripts during excavation ！！
    public void HandleHit(Vector3Int cell, Tilemap tilemap, DrillContext ctx)
    {
        int trueCost = ctx.GetCostFor(kind); // Consumption introduced from external sources into different blocks
        ctx.ModOil(-trueCost);
        ctx.PlayOneShot(mineSfx);

        durability -= Mathf.Max(1, ctx.damagePerHit);
        if (durability <= 0)
        {
            // Drop/Settlement
            if (oilDeltaOnMined != 0) ctx.ModOil(oilDeltaOnMined);
            if (scoreOnMined > 0) ctx.AddScore(scoreOnMined);

            // VFX
            if (mineVfxPrefab)
            {
                var world = tilemap.CellToWorld(cell) + tilemap.tileAnchor;
                GameObject.Instantiate(mineVfxPrefab, world, Quaternion.identity);
            }

            // Remove Tile (or replace with Dirt, etc.)
            tilemap.SetTile(cell, null);
            ctx.RaiseMined(kind, cell);
        }
        else
        {
            ctx.RaiseHit(kind, cell, durability);
        }
    }
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

    public int CostFor(ResourceKind k)
    {
        switch (k)
        {
            case ResourceKind.Dirt: return dirt;
            case ResourceKind.Rock: return rock;
            case ResourceKind.Iron: return iron;
            case ResourceKind.Gold: return gold;
            case ResourceKind.Oil: return -oilGain; // Negative values indicate refuelling.
            default: return 1;
        }
    }
}


