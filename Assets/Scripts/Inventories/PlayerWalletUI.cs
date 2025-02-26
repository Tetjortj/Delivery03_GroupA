using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalletUI : MonoBehaviour, IPickUp
{
    public Inventory Inventory;

    public void PickUp(ICanBePicked item)
    {
        // Add picked up item to player inventory
        Inventory.AddItem(item.GetItem());
    }    
}
