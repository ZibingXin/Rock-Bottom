using UnityEngine;

public class OilBlock : BlockBase
{
    [SerializeField] private int oilAmount = 20;
    public override void Dig()
    {
        isUsed = true;
        PlayerStats.Instance.RefillOil(oilAmount);
    }
}
