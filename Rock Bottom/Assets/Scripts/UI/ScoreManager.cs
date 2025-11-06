using System.Data;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public PlayerStats playerStats;

    //Stats Text
    public TMP_Text oilText;
    public TMP_Text moneyText;

    //Upgrade Lv
    public TMP_Text maxOilLvText;
    public TMP_Text moveSpeedLvText;
    public TMP_Text digStrengthLvText;

    private void Start()
    {
        UpdateStatus();
    }

    public void UpdateStatus()
    {
        maxOilLvText.text = "Max Oil: Lv. " + playerStats.MaxOilLv.ToString();
        moveSpeedLvText.text = "Speed: Lv. " + playerStats.MoveSpeedLv.ToString();
        digStrengthLvText.text = "Strength: Lv. " + playerStats.DigStrengthLv.ToString();
    }
    public void Update()
    {
        oilText.text = playerStats.CurrentOil.ToString() + " / " + playerStats.MaxOil.ToString();
        moneyText.text = "$" + playerStats.CurrentMoney;
    }
}
