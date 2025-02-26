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
    public static InventorySlotUI currentlySelectedSlot;

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
            // Obtenemos el InventoryUI de la zona sobre la que se soltó
            InventoryUI targetInventoryUI = result.gameObject.GetComponentInParent<InventoryUI>();

            // Si encontramos un inventario distinto del nuestro...
            if (targetInventoryUI != null && targetInventoryUI != _inventory)
            {
                // Si arrastramos DESDE la tienda (source) HACIA el inventario del jugador (target), entonces COMPRAMOS
                if (_inventory.IsShopInventory && !targetInventoryUI.IsShopInventory)
                {
                    _inventory.BuyItem(_item);
                }
                // Si arrastramos DESDE el inventario del jugador HACIA la tienda, entonces VENDEMOS
                else if (!_inventory.IsShopInventory && targetInventoryUI.IsShopInventory)
                {
                    _inventory.SellItem(_item);
                }

                // Actualizamos ambas interfaces
                _inventory.UpdateInventory();
                targetInventoryUI.UpdateInventory();

                // Destruimos el objeto arrastrado (el "slot clon" en caso de la tienda)
                Destroy(gameObject);
                return;
            }
        }

        // Si no se soltó en un área válida, restauramos la posición original
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

        // Si hacemos clic en el mismo slot que ya está seleccionado, lo deseleccionamos
        if (currentlySelectedSlot == this)
        {
            SetHighlight(false);
            currentlySelectedSlot = null;
            Debug.Log("Deseleccionado: " + _item.name);
        }
        else
        {
            // Seleccionamos este slot
            SetHighlight(true);
            currentlySelectedSlot = this;
            Debug.Log("Seleccionado: " + _item.name);
        }
    }

    private void SetHighlight(bool active)
    {
        if (highlightImage != null)
        {
            highlightImage.enabled = active;
        }
    }

}

