using TMPro;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public PlayerStats playerStats;

    public TMP_Text maxOilLvText;
    public TMP_Text oilCostText;

    public TMP_Text moveSpeedLvText;
    public TMP_Text moveSpeedCostText;

    public TMP_Text digStrengthLvText;
    public TMP_Text digStrengthCostText;

    private void Start()
    {
        UpdateUpgradeUI();
    }

    public void UpdateUpgradeUI()
    {
        maxOilLvText.text = "Max Oil: Lv. " + playerStats.MaxOilLv.ToString();
        oilCostText.text = "Cost: $" + (50 * playerStats.MaxOilLv).ToString();
        moveSpeedLvText.text = "Speed: Lv. " + playerStats.MoveSpeedLv.ToString();
        moveSpeedCostText.text = "Cost: $" + (50 * playerStats.MoveSpeedLv).ToString();
        digStrengthLvText.text = "Strength: Lv. " + playerStats.DigStrengthLv.ToString();
        digStrengthCostText.text = "Cost: $" + (50 * playerStats.DigStrengthLv).ToString();
    }
}
