using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryUI : MonoBehaviour
{
    public Inventory Inventory;        
    public InventorySlotUI SlotPrefab;       
    public bool IsShopInventory;         
    public PlayerWallet playerWallet;    

    public Button buyButton;
    public Button sellButton;

    public InventorySlotUI selectedSlot;

    private List<GameObject> _shownObjects = new List<GameObject>();

    void Start()
    {
        if (playerWallet == null)
        {
            playerWallet = FindObjectOfType<PlayerWallet>();
            if (playerWallet == null)
            {
                Debug.LogError("❌ No se encontró un PlayerWallet en la escena.");
            }
        }

        if (IsShopInventory)
        {
            Inventory = FindObjectOfType<ShopManager>().shopInventoryRuntime;
            Inventory.ResetInventory();
        }
        else
        {
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
            playerWallet.SpendMoney(item.Cost); 
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
        playerWallet.EarnMoney(item.Cost / 2); 
        Inventory.RemoveItem(item);
        FindObjectsOfType<InventoryUI>().First(i => i.IsShopInventory).Inventory.AddItem(item);
        UpdateInventory();
        playerWallet.UpdateMoneyUI();
    }


    public void OnSlotSelected(InventorySlotUI slot)
    {
        selectedSlot = slot;
        UpdateActionButtons();
    }

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

    public void OnBuyButtonClicked()
    {
        if (selectedSlot != null)
        {
            BuyItem(selectedSlot.ItemRef);
            selectedSlot.ClearHighlight();
            selectedSlot = null;
        }
    }

    public void OnSellButtonClicked()
    {
        if (selectedSlot != null)
        {
            SellItem(selectedSlot.ItemRef);
            selectedSlot.ClearHighlight();
            selectedSlot = null;
        }
    }

}
