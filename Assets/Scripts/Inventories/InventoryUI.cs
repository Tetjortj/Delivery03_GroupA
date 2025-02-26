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
        playerWallet = FindObjectOfType<PlayerWallet>();
        if (playerWallet == null)
        {
            Debug.LogError("❌ No se encontró un PlayerWallet en la escena.");
        }
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
        // Solo compramos si este UI es la tienda
        if (!IsShopInventory) return;

        if (playerWallet == null)
        {
            Debug.LogError("❌ PlayerWallet no está asignado.");
            return;
        }

        // Verificamos que el jugador pueda costear el item
        if (playerWallet.CanAfford(item.Cost))
        {
            // Descontamos el dinero
            playerWallet.SpendMoney(item.Cost);

            // Si quieres que el item NO desaparezca de la tienda, no lo quites:
            // Inventory.RemoveItem(item);

            // Agregamos el item al inventario del jugador
            FindObjectsOfType<InventoryUI>().First(i => !i.IsShopInventory).Inventory.AddItem(item);

            // Refrescamos la interfaz
            UpdateInventory();
            playerWallet.UpdateMoneyUI();
        }
        else
        {
            Debug.Log("No tienes suficiente dinero.");
        }
    }

    public void SellItem(ItemBase item)
    {
        // Solo vendemos si este UI es el inventario del jugador
        if (IsShopInventory) return;

        if (playerWallet == null)
        {
            Debug.LogError("❌ PlayerWallet no está asignado.");
            return;
        }

        // Gana la mitad del coste
        playerWallet.EarnMoney(item.Cost / 2);

        // Remueve el item del inventario del jugador
        Inventory.RemoveItem(item);

        // Lo añade al inventario de la tienda
        FindObjectsOfType<InventoryUI>().First(i => i.IsShopInventory).Inventory.AddItem(item);

        // Refrescamos la interfaz
        UpdateInventory();
        playerWallet.UpdateMoneyUI();
    }


}