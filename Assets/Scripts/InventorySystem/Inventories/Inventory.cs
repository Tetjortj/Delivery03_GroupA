using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Shop/Inventory")]
public class Inventory : ScriptableObject
{
    public List<Item> items = new List<Item>();
}

