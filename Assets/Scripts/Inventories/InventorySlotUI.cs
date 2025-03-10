﻿using System.Collections.Generic;
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

    public Image highlightImage;
    public TextMeshProUGUI PriceText;

    public static InventorySlotUI currentlySelectedSlot;

    public ItemBase ItemRef { get { return _item; } }
    public InventoryUI InventoryRef { get { return _inventory; } }

    public void Initialize(ItemSlot slot, InventoryUI inventory)
    {
        Image.sprite = slot.Item.ImageUI;
        Image.SetNativeSize();

        AmountText.text = slot.Amount > 1 ? "x" + slot.Amount.ToString() : "";
        AmountText.enabled = slot.Amount > 1;

        if (inventory.IsShopInventory)
        {
            PriceText.text = "$" + slot.Item.Cost.ToString();
            PriceText.enabled = true;
        }
        else
        {
            PriceText.enabled = false;
        }

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

                Destroy(gameObject);
                return;
            }
        }

        transform.SetParent(_parent);
        transform.localPosition = Vector3.zero;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentlySelectedSlot != null && currentlySelectedSlot != this)
        {
            currentlySelectedSlot.SetHighlight(false);
        }

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

    public void ClearHighlight()
    {
        SetHighlight(false);
    }
}
