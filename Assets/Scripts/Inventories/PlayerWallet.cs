using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    public int Money = 100; // Cantidad inicial de dinero

    public bool CanAfford(int amount)
    {
        return Money >= amount;
    }

    public void SpendMoney(int amount)
    {
        if (CanAfford(amount))
        {
            Money -= amount;
        }
    }

    public void AddMoney(int amount)
    {
        Money += amount;
    }

    public int GetMoney()
    {
        return Money;
    }
}
