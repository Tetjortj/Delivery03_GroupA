using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField]
    private List<ItemSlot> slots = new List<ItemSlot>();

    public int Length => slots.Count;
    public Action OnInventoryChange;

    private void OnEnable()
    {
        if (slots == null) slots = new List<ItemSlot>();
    }

    public List<ItemSlot> GetSlots()
    {
        return slots;
    }

    public void AddItem(ItemBase item)
    {
        if (slots == null) slots = new List<ItemSlot>();

        var slot = GetSlot(item);

        if ((slot != null) && (item.IsStackable))
        {
            slot.AddOne();
        }
        else
        {
            slot = new ItemSlot(item);
            slots.Add(slot);
        }

        OnInventoryChange?.Invoke();
    }

    public void RemoveItem(ItemBase item)
    {
        if (slots == null) return;

        var slot = GetSlot(item);

        if (slot != null)
        {
            slot.RemoveOne();
            if (slot.IsEmpty()) RemoveSlot(slot);
        }

        OnInventoryChange?.Invoke();
    }

    private void RemoveSlot(ItemSlot slot)
    {
        slots.Remove(slot);
    }

    private ItemSlot GetSlot(ItemBase item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].HasItem(item)) return slots[i];
        }

        return null;
    }

    public ItemSlot GetSlot(int i)
    {
        return slots[i];
    }
}
