using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;


/*
 * ResourceTile template (inherits from TileBase)
 * - Five block types: Dirt / Rock / Iron / Gold / Oil
 * - Supports: Durability, mining events, drop/refuel, rare light effects (Prefab, optional URP 2D Light or emitting Sprite)
 * - Usage: Create one ScriptableObject resource per resource type (Create > Tiles > ResourceTile),
 *         Use within the Tile Palette; Call HandleDig() via Drill logic at runtime.
*/

public enum ResourceType { Dirt, Rock, Iron, Gold, Oil }

[CreateAssetMenu(menuName = "Tiles/ResourceTile", fileName = "ResourceTile")]
public class ResourceTile : TileBase
{
    [Header("Appearance and Data")]
    public ResourceType type;
    public bool IsDiggable => type != ResourceType.Rock;
    public Sprite sprite;

    [Header("Resource Effect")]
    public int oilDeltaOnMined = 0;
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
    public void HandleDig(Vector3Int cell, Tilemap tilemap, DrillContext ctx)
    {
        if (IsDiggable)
        {
            int trueCost = ctx.GetCostFor(type); // Consumption introduced from external sources into different blocks
            ctx.ModOil(-trueCost);
            ctx.PlayOneShot(mineSfx);

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
            ctx.RaiseMined(type, cell);
        }
    }
}




