using TMPro;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public PlayerStats playerStats;

    public TMP_Text maxOilLvText;
    public TMP_Text oilCostText;

    public TMP_Text drillWorthLvText;
    public TMP_Text drillWorthCostText;

    public TMP_Text digStrengthLvText;
    public TMP_Text digStrengthCostText;

    public TMP_Text currentMoneyText;

    private void Start()
    {
        UpdateUpgradeUI();
    }

    public void UpdateUpgradeUI()
    {
        maxOilLvText.text = "Lv. " + playerStats.MaxOilLv.ToString();
        oilCostText.text = "Cost: $" + (50 * playerStats.MaxOilLv).ToString();
        drillWorthCostText.text = "Cost: $" + (50 * playerStats.DrillWorthLv).ToString();
        drillWorthLvText.text = "Lv. " + playerStats.DrillWorthLv.ToString();
        digStrengthLvText.text = "Lv. " + playerStats.DigStrengthLv.ToString();
        digStrengthCostText.text = "Cost: $" + (50 * playerStats.DigStrengthLv).ToString();
        currentMoneyText.text = "Money: $" + playerStats.CurrentMoney.ToString();
    }
}
