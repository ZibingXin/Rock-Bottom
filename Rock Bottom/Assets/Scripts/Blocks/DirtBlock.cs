using UnityEngine;

public class DirtBlock : BlockBase
{
    public override void Dig()
    {
        isUsed = true;
        PlayerStats.Instance.BurnOil(priceToDig);
    }
}
