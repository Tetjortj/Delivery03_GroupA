using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryUI : MonoBehaviour
{
    public Inventory Inventory;             // Asigna el ScriptableObject correspondiente (Shop o Player) en el Inspector
    public InventorySlotUI SlotPrefab;        // Prefab para cada slot en la UI
    public bool IsShopInventory; // Indica si este UI es para la tienda
    public PlayerWallet playerWallet;

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

    public void BuyItem(ItemBase item)
    {
        // Asegúrate de tener asignado el PlayerWallet
        if (playerWallet == null)
        {
            Debug.LogError("❌ PlayerWallet no está asignado.");
            return;
        }

        // Verifica que el jugador pueda costear el item
        if (playerWallet.CanAfford(item.Cost))
        {
            // Descuenta el costo del dinero del jugador
            playerWallet.SpendMoney(item.Cost);

            // Agrega el item al inventario del jugador.
            // Nota: No removemos el item de la tienda, ya que se supone que siempre debe estar visible.
            InventoryUI playerInventoryUI = FindObjectsOfType<InventoryUI>().First(i => !i.IsShopInventory);
            playerInventoryUI.Inventory.AddItem(item);

            // Actualiza las interfaces
            UpdateInventory();
            playerWallet.UpdateMoneyUI();
        }
        else
        {
            Debug.Log("No tienes suficiente dinero para comprar este item.");
        }
    }

    public void SellItem(ItemBase item)
    {
        // Asegúrate de tener asignado el PlayerWallet
        if (playerWallet == null)
        {
            Debug.LogError("❌ PlayerWallet no está asignado.");
            return;
        }

        // Al vender, se otorga la mitad del costo original
        int sellPrice = item.Cost / 2;
        playerWallet.EarnMoney(sellPrice);

        // Remueve el item del inventario del jugador
        Inventory.RemoveItem(item);

        // (Opcional) Agrega el item de vuelta al inventario de la tienda
        InventoryUI shopInventoryUI = FindObjectsOfType<InventoryUI>().First(i => i.IsShopInventory);
        shopInventoryUI.Inventory.AddItem(item);

        // Actualiza la UI y el dinero mostrado
        UpdateInventory();
        playerWallet.UpdateMoneyUI();
    }


}