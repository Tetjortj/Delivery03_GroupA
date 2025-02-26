using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image Image;
    public TextMeshProUGUI AmountText;

    private Canvas _canvas;
    private Transform _parent;
    private ItemBase _item;
    private InventoryUI _inventory;
    private GraphicRaycaster _raycaster;

    public void Initialize(ItemSlot slot, InventoryUI inventory)
    {
        Image.sprite = slot.Item.ImageUI;
        Image.SetNativeSize();

        AmountText.text = slot.Amount > 1 ? "x" + slot.Amount.ToString() : "";
        AmountText.enabled = slot.Amount > 1;

        _item = slot.Item;
        _inventory = inventory;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _parent = transform.parent;

        // Verificar si _canvas ya está asignado
        if (!_canvas)
        {
            _canvas = GetComponentInParent<Canvas>();

            if (_canvas == null)
            {
                Debug.LogError("❌ No se encontró un Canvas en los padres del objeto.");
                return;
            }

            _raycaster = _canvas.GetComponent<GraphicRaycaster>();
        }

        // Mover objeto al Canvas
        transform.SetParent(_canvas.transform, true);
        transform.SetAsLastSibling();
    }


    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            InventoryUI targetInventoryUI = result.gameObject.GetComponentInParent<InventoryUI>();

            if (targetInventoryUI != null && targetInventoryUI.Inventory != _inventory)
            {
                _inventory.Inventory.RemoveItem(_item);
                targetInventoryUI.Inventory.AddItem(_item);

                _inventory.UpdateInventory();
                targetInventoryUI.UpdateInventory();

                Destroy(gameObject);
                return;
            }
        }

        transform.SetParent(_parent);
        transform.localPosition = Vector3.zero;
    }

}

