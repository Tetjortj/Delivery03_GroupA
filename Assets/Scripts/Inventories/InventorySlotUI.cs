using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Image Image;
    public TextMeshProUGUI AmountText;

    private Canvas _canvas;
    private Transform _parent;
    private ItemBase _item;
    private InventoryUI _inventory;
    private GraphicRaycaster _raycaster;

    // Objeto que se usará para resaltar el slot
    public Image highlightImage;

    // Slot actualmente seleccionado (estático para poder deseleccionar el anterior)
    public static InventorySlotUI currentlySelectedSlot;

    // Propiedades para que otros scripts puedan obtener la información del slot
    public ItemBase ItemRef { get { return _item; } }
    public InventoryUI InventoryRef { get { return _inventory; } }

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

        // Mueve el slot al Canvas para que se vea por encima
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

            if (targetInventoryUI != null && targetInventoryUI != _inventory)
            {
                if (_inventory.IsShopInventory && !targetInventoryUI.IsShopInventory)
                {
                    _inventory.BuyItem(_item);
                }
                else if (!_inventory.IsShopInventory && targetInventoryUI.IsShopInventory)
                {
                    _inventory.SellItem(_item);
                }

                _inventory.UpdateInventory();
                targetInventoryUI.UpdateInventory();

                // Destruye el objeto arrastrado (en caso de ser un clon en la tienda)
                Destroy(gameObject);
                return;
            }
        }

        // Si no se soltó en un área válida, vuelve a la posición original
        transform.SetParent(_parent);
        transform.localPosition = Vector3.zero;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Si ya hay un slot seleccionado y es distinto de este, deselecciónalo
        if (currentlySelectedSlot != null && currentlySelectedSlot != this)
        {
            currentlySelectedSlot.SetHighlight(false);
        }

        // Si se hace clic en el mismo slot seleccionado, se deselecciona
        if (currentlySelectedSlot == this)
        {
            SetHighlight(false);
            currentlySelectedSlot = null;
            Debug.Log("Deseleccionado: " + _item.name);
        }
        else
        {
            SetHighlight(true);
            currentlySelectedSlot = this;
            Debug.Log("Seleccionado: " + _item.name);

            // Notifica al InventoryUI que se ha seleccionado este slot
            if (_inventory != null)
            {
                _inventory.OnSlotSelected(this);
            }
        }
    }

    private void SetHighlight(bool active)
    {
        if (highlightImage != null)
        {
            highlightImage.enabled = active;
        }
    }

    // Método público para borrar el resaltado (útil al ejecutar la acción mediante botón)
    public void ClearHighlight()
    {
        SetHighlight(false);
    }
}
