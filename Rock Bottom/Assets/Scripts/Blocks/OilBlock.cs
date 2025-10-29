using UnityEngine;

public class OilBlock : BlockBase
{
    [SerializeField] private float oilAmount = 20f;
    public override void Dig()
    {
        isUsed = true;
        PlayerStats.Instance.RefillOil(oilAmount);
    }
}
