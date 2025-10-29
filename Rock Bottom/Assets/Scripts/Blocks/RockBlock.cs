using UnityEngine;

public class RockBlock : BlockBase
{
    public override void Dig()
    {
        PlayerStats.Instance.BurnOil(priceToDig);
    }
}
