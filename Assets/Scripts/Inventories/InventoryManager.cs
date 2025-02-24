using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public Inventory playerInventory;
    public Inventory shopInventory;
    public int playerCoins = 100; // Monedas iniciales

    public void BuyItem(Item item)
    {
        if (playerCoins >= item.cost && shopInventory.items.Contains(item))
        {
            playerCoins -= item.cost;
            playerInventory.items.Add(item);
            shopInventory.items.Remove(item);
            Debug.Log($"Compraste {item.itemName}. Monedas restantes: {playerCoins}");
        }
        else
        {
            Debug.Log("No tienes suficientes monedas o el objeto no está en la tienda.");
        }
    }

    public void SellItem(Item item)
    {
        if (playerInventory.items.Contains(item))
        {
            playerCoins += item.cost / 2; // Se vende por la mitad del precio
            playerInventory.items.Remove(item);
            shopInventory.items.Add(item);
            Debug.Log($"Vendiste {item.itemName}. Monedas actuales: {playerCoins}");
        }
        else
        {
            Debug.Log("No puedes vender un objeto que no tienes.");
        }
    }

    public void UseItem(Item item)
    {
        if (playerInventory.items.Contains(item) && item.type == Item.ItemType.Food)
        {
            Debug.Log($"Usaste {item.itemName} y recuperaste vida.");
            playerInventory.items.Remove(item);
        }
        else
        {
            Debug.Log("No puedes usar este objeto.");
        }
    }
}

