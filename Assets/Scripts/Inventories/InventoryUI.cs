using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryUI : MonoBehaviour
{
    public Inventory Inventory;             // Asigna el ScriptableObject (Shop o Player) en el Inspector
    public InventorySlotUI SlotPrefab;        // Prefab para cada slot en la UI
    public bool IsShopInventory;              // Indica si este UI es para la tienda
    public PlayerWallet playerWallet;         // Referencia al PlayerWallet (puedes asignarla o buscarla en Start)

    // Botones para comprar o vender; asigna estos en el Inspector
    public Button buyButton;
    public Button sellButton;

    // Slot seleccionado actualmente
    public InventorySlotUI selectedSlot;

    private List<GameObject> _shownObjects = new List<GameObject>();

    void Start()
    {
        // Intenta encontrar el PlayerWallet si no está asignado manualmente
        if (playerWallet == null)
        {
            playerWallet = FindObjectOfType<PlayerWallet>();
            if (playerWallet == null)
            {
                Debug.LogError("❌ No se encontró un PlayerWallet en la escena.");
            }
        }

        // Para la tienda, usamos la copia runtime desde ShopManager y reseteamos
        if (IsShopInventory)
        {
            Inventory = FindObjectOfType<ShopManager>().shopInventoryRuntime;
            Inventory.ResetInventory();
        }
        else
        {
            // Para el jugador, se asume que el Inventory se asigna en el Inspector
            Inventory.GetSlots().Clear();
        }
        ClearInventory();
        FillInventory(Inventory);
        UpdateActionButtons();
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
        UpdateActionButtons();
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

    // Lógica para comprar un item (se ejecuta tanto desde drag como desde botón)
    public void BuyItem(ItemBase item)
    {
        if (!IsShopInventory) return;
        if (playerWallet == null)
        {
            Debug.LogError("❌ PlayerWallet no está asignado.");
            return;
        }
        if (playerWallet.CanAfford(item.Cost))
        {
            playerWallet.SpendMoney(item.Cost);  // Descuenta el costo del item
            FindObjectsOfType<InventoryUI>().First(i => !i.IsShopInventory).Inventory.AddItem(item);
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
        if (IsShopInventory) return;
        if (playerWallet == null)
        {
            Debug.LogError("❌ PlayerWallet no está asignado.");
            return;
        }
        playerWallet.EarnMoney(item.Cost / 2);  // Acredita la mitad del costo
        Inventory.RemoveItem(item);
        FindObjectsOfType<InventoryUI>().First(i => i.IsShopInventory).Inventory.AddItem(item);
        UpdateInventory();
        playerWallet.UpdateMoneyUI();
    }


    // Se invoca cuando se selecciona un slot en la UI
    public void OnSlotSelected(InventorySlotUI slot)
    {
        selectedSlot = slot;
        UpdateActionButtons();
    }

    // Actualiza la visibilidad de los botones según el tipo de inventario
    public void UpdateActionButtons()
    {
        if (IsShopInventory)
        {
            if (buyButton != null)
                buyButton.gameObject.SetActive(true);
            if (sellButton != null)
                sellButton.gameObject.SetActive(false);
        }
        else
        {
            if (buyButton != null)
                buyButton.gameObject.SetActive(false);
            if (sellButton != null)
                sellButton.gameObject.SetActive(true);
        }
    }

    // Método para que el botón de compra ejecute la acción sobre el slot seleccionado
    // Método para el botón de compra
    public void OnBuyButtonClicked()
    {
        if (selectedSlot != null)
        {
            // Llama a BuyItem, que dentro de su lógica descuenta el dinero
            BuyItem(selectedSlot.ItemRef);
            selectedSlot.ClearHighlight();
            selectedSlot = null;
        }
    }

    // Método para el botón de venta
    public void OnSellButtonClicked()
    {
        if (selectedSlot != null)
        {
            // Llama a SellItem, que dentro de su lógica acredita la wallet
            SellItem(selectedSlot.ItemRef);
            selectedSlot.ClearHighlight();
            selectedSlot = null;
        }
    }

}
