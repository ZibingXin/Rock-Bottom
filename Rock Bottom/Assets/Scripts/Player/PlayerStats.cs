using UnityEditor.ShortcutManagement;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //Oil
    [SerializeField] private float currentOil = 100; //current amount of oil
    [SerializeField] private float maxOil = 100; //maximum amount of oil

    //Movement
    [SerializeField] private float moveSpeed = 5f; //Affects how fast the player moves and how quickly they can dig
    [SerializeField] private float digStrength = 1f; //When Player Controller consumes oil it will divide by this value

    //Money
    [SerializeField] private int currentMoney = 0; //current amount of money
    [SerializeField] private int drillWorth = 1; 

    //upgrade UI
    [SerializeField] public GameObject gameOverScreen;
    [SerializeField] private int maxOilLv = 1;
    [SerializeField] private int moveSpeedLv = 1;
    [SerializeField] private int digStrengthLv = 1;
    [SerializeField] private int drillWorthLv = 1;

    public float CurrentOil { get { return currentOil; } }
    public float MaxOil { get { return maxOil; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public float DigStrength { get { return digStrength; } }
    public int CurrentMoney { get { return currentMoney; } }
    public int DrillWorth { get { return drillWorth; } }

    public int MaxOilLv { get { return maxOilLv; } }
    public int MoveSpeedLv { get { return moveSpeedLv; } }
    public int DigStrengthLv { get { return digStrengthLv; } }
    public int DrillWorthLv { get { return drillWorthLv; } }
    public void SetDrillWorthLv(int value) { drillWorthLv = value; }

    public static PlayerStats Instance;

    private void Start()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }

        currentMoney = 0;
        ResetPlayer();
    }

    public void ResetPlayer()
    {
        currentOil = maxOil;
    }

    public float FinalOilCost(float baseCost)
    {
        return Mathf.Clamp(baseCost - digStrength, 1, baseCost);
    }

    public void BurnOil(float amount)
    {
        currentOil -= FinalOilCost(amount);
        if (currentOil <= 0)
        {
            currentOil = 0;
            GameOver();
        }
        Debug.Log("Oil remaining: " + currentOil);
    }

    public void RefillOil(float amount)
    {
        currentOil += amount;
        Debug.Log("Oil refilled. Current oil: " + currentOil);
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log("Added money: " + amount);
    }

    public void ReduceMoney(int amount)
    {
        currentMoney -= amount;
        Debug.Log("Reduced money: " + amount);
    }

    //Upgrades
    public void UpgradeOil()
    {
        //Increase max oil
        maxOil += 20;
        maxOilLv += 1;
    }

    public void UpgradeMoveSpeed()
    {
        //Increase move speed
        moveSpeed += 1f;
        moveSpeedLv += 1;
    }

    public void UpgradeDigStrength()
    {
        //Increase dig strength
        digStrength += 0.5f;
        digStrengthLv += 1;
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }
}
