using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryUI : MonoBehaviour
{
    public Inventory Inventory;             // Asigna el ScriptableObject correspondiente (Shop o Player) en el Inspector
    public InventorySlotUI SlotPrefab;        // Prefab para cada slot en la UI
    public bool IsShopInventory;              // Indica si este UI es para la tienda

    private List<GameObject> _shownObjects = new List<GameObject>();

    void Start()
    {
        // Si es el inventario de la tienda, asigna la copia runtime desde ShopManager y resetea
        if (IsShopInventory)
        {
            Inventory = FindObjectOfType<ShopManager>().shopInventoryRuntime;
            Inventory.ResetInventory(); // Restaura a los valores originales del asset
        }
        else
        {
            // Inventario del jugador: se asume que está asignado en el Inspector y se vacía al inicio
            Inventory.GetSlots().Clear();
        }
        ClearInventory();
        FillInventory(Inventory);
    }

    private void OnEnable()
    {
        Inventory.OnInventoryChange += UpdateInventory;
    }

    private void OnDisable()
    {
        Inventory.OnInventoryChange -= UpdateInventory;
    }

    public void UpdateInventory()
    {
        ClearInventory();
        FillInventory(Inventory);
    }

    private void ClearInventory()
    {
        foreach (var obj in _shownObjects)
        {
            if (obj != null)
                Destroy(obj);
        }
        _shownObjects.Clear();
    }

    private void FillInventory(Inventory inventory)
    {
        if (inventory == null) return;
        for (int i = 0; i < inventory.Length; i++)
        {
            _shownObjects.Add(AddSlot(inventory.GetSlot(i)));
        }
    }

    private GameObject AddSlot(ItemSlot inventorySlot)
    {
        var element = Instantiate(SlotPrefab, Vector3.zero, Quaternion.identity, transform);
        element.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
        element.Initialize(inventorySlot, this);
        return element.gameObject;
    }

    public void UseItem(ItemBase item)
    {
        Inventory.RemoveItem(item);
    }

    public void RemoveItem(ItemBase item)
    {
        Inventory.RemoveItem(item);
        UpdateInventory();
    }
}