using System;
using UnityEngine;

public class DrillContext
{
    public int damagePerHit = 1;
    public Action<ResourceKind, Vector3Int> onMined;
    public Action<ResourceKind, Vector3Int, int> onHit; // Remaining durability

    private AudioSource sfx;
    private int oil;
    private int score;
    private DrillCostsConfig costs;

    public DrillContext(AudioSource sfx, int startOil, DrillCostsConfig costs, int damagePerHit = 1)
    {
        this.sfx = sfx; oil = startOil; this.costs = costs; this.damagePerHit = damagePerHit;
    }

    public int GetOil() => oil;
    public void ModOil(int delta) { oil = Mathf.Max(0, oil + delta); }
    public void AddScore(int s) { score += s; }
    public int GetCostFor(ResourceKind k) => costs.CostFor(k);
    public void PlayOneShot(AudioClip clip) { if (clip && sfx) sfx.PlayOneShot(clip); }

    public void RaiseMined(ResourceKind k, Vector3Int cell) => onMined?.Invoke(k, cell);
    public void RaiseHit(ResourceKind k, Vector3Int cell, int remain) => onHit?.Invoke(k, cell, remain);
}