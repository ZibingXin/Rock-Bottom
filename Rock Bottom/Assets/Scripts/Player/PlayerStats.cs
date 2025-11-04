using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //Oil
    [SerializeField] private int currentOil = 100; //current amount of oil
    [SerializeField] private int maxOil = 100; //maximum amount of oil

    //Money
    [SerializeField] private int currentMoney = 0; //current amount of money

    public int CurrentOil { get { return currentOil; } }
    //public int MaxOil { get { return maxOil; } }
    public int CurrentMoney { get { return currentMoney; } }

    public static PlayerStats Instance;

    private void Start()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    public void BurnOil(int amount)
    {
        currentOil -= amount;
        if (currentOil < 0)
        {
            currentOil = 0;
        }
        //Debug.Log("Oil remaining: " + currentOil);
    }

    public void RefillOil(int amount)
    {
        currentOil += amount;
        if (currentOil > maxOil)
        {
            currentOil = maxOil;
        }
        //Debug.Log("Oil refilled. Current oil: " + currentOil);
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        //Debug.Log("Added money: " + amount);
    }

    public void ReduceMoney(int amount)
    {
        currentMoney -= amount;
        //Debug.Log("Reduced money: " + amount);
    }

    //Upgrades
    public void UpgradeOil()
    {
        //Increase max oil
        //maxOil += 20;
    }

    public void UpgradeMoveSpeed()
    {
        //Increase move speed
        //moveSpeed += 1f;
    }

    public void UpgradeDigStrength()
    {
        //Increase dig strength
        //digStrength += 0.5f;
    }

    public void GameOver()
    {

    }
}
