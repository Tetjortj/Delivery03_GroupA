using UnityEngine;
using TMPro;

public class PlayerWallet : MonoBehaviour
{
    [SerializeField] private int money = 100;  // Dinero inicial del jugador
    [SerializeField] private TMP_Text moneyText; // Referencia al texto UI que muestra el dinero

    void Start()
    {
        UpdateMoneyUI();
    }

    // Comprueba si el jugador puede costear un item
    public bool CanAfford(int cost)
    {
        return money >= cost;
    }

    // Resta el dinero gastado y actualiza la UI
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

    // Suma el dinero ganado al vender y actualiza la UI
    public void EarnMoney(int amount)
    {
        money += amount;
        Debug.Log("Dinero después de vender: " + money);
        UpdateMoneyUI();
    }

    // Actualiza el texto de la UI para mostrar el dinero actual
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
