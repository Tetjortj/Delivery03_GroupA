using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIManager : MonoBehaviour
{
    public Inventory shopInventory;  // Inventario de la tienda (ScriptableObject)
    public Transform shopContainer;  // Contenedor donde se mostrarán los ítems (Grid Layout Group)
    public GameObject shopSlotPrefab; // Prefab de cada slot de la tienda

    void Start()
    {
        LoadShopItems();
    }

    void LoadShopItems()
    {
        // Verificar si el inventario de la tienda está asignado
        if (shopInventory == null)
        {
            Debug.LogError("❌ El inventario de la tienda no está asignado en el ShopUIManager.");
            return;
        }

        // Limpiar la tienda antes de cargar los ítems (para evitar duplicados)
        foreach (Transform child in shopContainer)
        {
            Destroy(child.gameObject);
        }

        // Obtener los slots desde el inventario
        List<ItemSlot> slots = shopInventory.GetSlots();
        if (slots == null || slots.Count == 0)
        {
            Debug.LogWarning("⚠️ El inventario de la tienda está vacío.");
            return;
        }

        // Crear un slot para cada ítem en el inventario
        foreach (ItemSlot slot in slots)
        {
            if (slot.Item == null) continue;

            GameObject newSlot = Instantiate(shopSlotPrefab, shopContainer);

            // Asignar la imagen del ítem
            newSlot.transform.Find("ItemImage").GetComponent<Image>().sprite = slot.Item.ImageUI;

            // Asignar la cantidad (si es apilable, mostrar cuántos hay)
            TextMeshProUGUI quantityText = newSlot.transform.Find("Quantity (TMP)").GetComponent<TextMeshProUGUI>();
            quantityText.text = slot.Amount > 1 ? "x" + slot.Amount.ToString() : "";
        }
    }
}