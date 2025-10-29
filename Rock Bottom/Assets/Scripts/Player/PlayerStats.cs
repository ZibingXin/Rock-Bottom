using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    //Oil
    [SerializeField] private float currentOil = 100;
    [SerializeField] private float maxOil = 100;

    //Movement
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float digStrength = 1f; //When Player Controller consumes oil it will divide by this value

    //Money
    [SerializeField] private int currentMoney = 0;

    public static PlayerStats Instance;

    private void Start()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    public void BurnOil(float amount)
    {
        currentOil -= amount;
        if (currentOil < 0)
        {
            currentOil = 0;
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
