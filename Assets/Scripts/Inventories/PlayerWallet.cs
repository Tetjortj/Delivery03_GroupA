using UnityEngine;
using TMPro;

public class PlayerWallet : MonoBehaviour
{
    [SerializeField] private int money = 100; 
    [SerializeField] private TMP_Text moneyText;

    void Start()
    {
        UpdateMoneyUI();
    }

    public bool CanAfford(int cost)
    {
        return money >= cost;
    }

    public void SpendMoney(int amount)
    {
        if (CanAfford(amount))
        {
            money -= amount;
            Debug.Log("Dinero restante: " + money);
            UpdateMoneyUI();
        }
        else
        {
            Debug.LogWarning("No hay suficiente dinero para gastar " + amount);
        }
    }

    public void EarnMoney(int amount)
    {
        money += amount;
        Debug.Log("Dinero después de vender: " + money);
        UpdateMoneyUI();
    }

    public void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "$" + money;
            Debug.Log("Dinero actualizado: " + money);
        }
        else
        {
            Debug.LogError("moneyText no asignado en PlayerWallet");
        }
    }
}
