using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public PlayerStats playerStats;
    public UpgradeUI upgradeUI;
    public DrillWorthConfig drillWorthConfig;

    private int cost;

    private void Start()
    {
        playerStats = PlayerStats.Instance;
    }

    public int CalculateCost(int currentLevel)
    {
        return 50 * currentLevel;
    }

    public void UpgradeMaxOil()
    {
        cost = CalculateCost(playerStats.MaxOilLv);
        if (playerStats.CurrentMoney >= cost)
        {
            playerStats.UpgradeOil(); 
            playerStats.ReduceMoney(cost);
            //upgradeUI.UpdateUpgradeUI();
        }
        else
        {
            Debug.Log("Not enough money to upgrade Max Oil.");
        }
    }    

    public void UpgradeMoveSpeed()
    {
        cost = CalculateCost(playerStats.MoveSpeedLv);
        if (playerStats.CurrentMoney >= cost)
        {
            playerStats.UpgradeMoveSpeed(); 
            playerStats.ReduceMoney(cost);
            //upgradeUI.UpdateUpgradeUI();
        }
        else
        {
            Debug.Log("Not enough money to upgrade Move Speed.");
        }
    }

    public void UpgradeDigStrength()
    {
        cost = CalculateCost(playerStats.DigStrengthLv);
        if (playerStats.CurrentMoney >= cost)
        {
            playerStats.UpgradeDigStrength(); 
            playerStats.ReduceMoney(cost);
            //upgradeUI.UpdateUpgradeUI();
        }
        else
        {
            Debug.Log("Not enough money to upgrade Dig Strength.");
        }
    }

    public void UpgradeDrillWorth()
    {
        cost=CalculateCost(playerStats.DrillWorthLv);
        if (playerStats.CurrentMoney >= cost)
        {
            playerStats.ReduceMoney(cost);
            //drillWorthConfig.UpdateDrillWorth();
            //upgradeUI.UpdateUpgradeUI();
        }
        else
        {
            Debug.Log("Not enough money to upgrade Drill Worth.");
        }
    }

    public int GetUpgradeCost(string upgradeType)
    {
        switch (upgradeType)
        {
            case "MaxOil":
                return CalculateCost(playerStats.MaxOilLv);
            case "MoveSpeed":
                return CalculateCost(playerStats.MoveSpeedLv);
            case "DigStrength":
                return CalculateCost(playerStats.DigStrengthLv);
            default:
                Debug.LogError("Invalid upgrade type: " + upgradeType);
                return 0;
        }
    }
}
