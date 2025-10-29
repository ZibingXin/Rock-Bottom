using UnityEngine;

public class ResourceBlock : BlockBase
{
    [SerializeField] private ResourceType resourceType;
    [SerializeField] private int moneyWorth;
    public override void Dig()
    {
        isUsed = true;
        PlayerStats.Instance.BurnOil(priceToDig);
        PlayerStats.Instance.AddMoney(moneyWorth);
    }
}
