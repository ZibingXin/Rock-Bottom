using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public PlayerStats playerStats;
    public UpgradeUI upgradeUI;

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
}
