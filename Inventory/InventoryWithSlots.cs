using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryWithSlots : IInventory
{
    public event Action<object, IInventoryItem, int> OnInventoryItemAddedEvent;

    public event Action<object, Type, int> OnInventoryRemovedAddedEvent;

    public event Action<object> OnInventoryStateChangedEvent;

    public int capacity { get; set; }

    public bool isFull => _slots.All(slot => slot.isFull);

    private List<IInventorySlot> _slots;

    public InventoryWithSlots(int capacity)
    {
        this.capacity = capacity;

        _slots = new List<IInventorySlot>(capacity);
        for(int i = 0; i < capacity; i++)
        {
            _slots.Add(new InventorySlot());
        }
    }

    public IInventoryItem GetItem(Type itemType)
    {
        return _slots.Find(slot => slot.itemType == itemType).item;
    }

    public IInventoryItem[] GetAllItems()
    {
        var allItems = new List<IInventoryItem>();
        foreach(var slot in _slots)
        {
            if (!slot.isEmpty)
            {
                allItems.Add(slot.item);
            }
        }
        return allItems.ToArray();
    }

    public IInventoryItem[] GetAllItems(Type itemType)
    {
        var allItemsCurrentType = new List<IInventoryItem>();
        var slotsOfType = _slots.FindAll(slot => !slot.isEmpty && slot.itemType == itemType);
        foreach(var slot in slotsOfType)
        {
            allItemsCurrentType.Add(slot.item);
        }
        return allItemsCurrentType.ToArray();
    }

    public IInventoryItem[] GetEquippedItems()
    {
        var requiredSlots = _slots.FindAll(slot => !slot.isEmpty && slot.item.state.isEquipped);
        var equippedItems = new List<IInventoryItem>();
        foreach(var slot in requiredSlots)
        {
            equippedItems.Add(slot.item);
        }
        return equippedItems.ToArray();
    }

    public int GetItemAmount(Type itemType)
    {
        var amount = 0;
        var allItemsSlots = _slots.FindAll(slot => slot.isEmpty && slot.itemType == itemType);
        foreach(var itemSlot in allItemsSlots)
        {
            amount += itemSlot.amount;
        }
        return amount;
    }

    public bool HasItem(Type type, out IInventoryItem item)
    {
        item = GetItem(type);
        return item != null;
    }

    public void TransFromSlotToSlot(object sender, IInventorySlot fromSlot, IInventorySlot toSlot)
    {
        if (fromSlot.isEmpty)
            return;

        if (fromSlot.isFull)
            return;
        
        if(!fromSlot.isEmpty && fromSlot.itemType != toSlot.itemType)
            return;

        var slotCapacity = fromSlot.capacity;
        var fits = fromSlot.amount + toSlot.amount <= slotCapacity;
        var amountToAdd = fits ? fromSlot.amount : fromSlot.amount - toSlot.amount;
        var amountLeft = fromSlot.amount - amountToAdd;

        if (toSlot.isEmpty)
        {
            toSlot.SetItem(fromSlot.item);
            fromSlot.Clear();
            OnInventoryStateChangedEvent?.Invoke(sender);
        }

        toSlot.item.state.amount += amountToAdd;
        if (fits)
        {
            fromSlot.Clear();
        }
        else
        {
            fromSlot.item.state.amount = amountLeft;
        }
    }

    public void Remove(object sender, Type itemType, int amount = 1)
    {
        var slotsWithItem = GetAllSlots(itemType);
        if(slotsWithItem.Length == 0)
        {
            return;
        }
        var amountToRemove = amount;
        var count = slotsWithItem.Length;

        for(int i = count - 1; i >= 0; i--)
        {
            var slot = slotsWithItem[i];
            if (slot.amount > amountToRemove)
            {
                slot.item.state.amount = amountToRemove;

                if (slot.amount <= 0)
                {
                    slot.Clear();
                }

                Debug.Log($"Item removed from inventory {itemType}, amount: {amountToRemove}");
                OnInventoryRemovedAddedEvent?.Invoke(sender, itemType, amountToRemove);
                OnInventoryStateChangedEvent?.Invoke(sender);

                break;
            }
            var amountRemoved = slot.amount;
            amountToRemove -= slot.amount;
            slot.Clear();

            Debug.Log($"Item removed from inventory {itemType}, amount: {amountRemoved}");
            OnInventoryRemovedAddedEvent?.Invoke(sender, itemType, amountRemoved);
            OnInventoryStateChangedEvent?.Invoke(sender);
        }
    }
    public bool TryToAdd(object sender,  IInventoryItem item)
    {
        var slotWithSameItemButNotEmpty = _slots.
            Find(slot => !slot.isEmpty && slot.itemType == item.type && !slot.isFull);

        if(slotWithSameItemButNotEmpty != null)
        {
            return TryToAddToSlot(sender, slotWithSameItemButNotEmpty, item);
        }

        var emptySlot = _slots.Find(slot => slot.isEmpty);
        if (emptySlot != null)
        {
            return TryToAddToSlot(sender, emptySlot, item);
        }
        Debug.Log("Cannot add item ({item.type})");
        return false;

    }
    public bool TryToAddToSlot(object sender, IInventorySlot slot, IInventoryItem item)
    {
        var fits = slot.amount + item.state.amount <= item.info.maxItemInSlot;
        var amountToAdd = fits ? item.state.amount : item.info.maxItemInSlot - slot.amount;
        var amountleft = item.state.amount - amountToAdd;
        var clonedItem = item.Clone();
        clonedItem.state.amount = amountToAdd;

        if (slot.isEmpty)
        {
            slot.SetItem(clonedItem);
        }
        else
        {
            slot.item.state.amount += amountToAdd;
        }
        Debug.Log($"Item {item.type} added  to inventory, amount = {amountToAdd}");
        OnInventoryItemAddedEvent?.Invoke(sender, item, amountToAdd);
        OnInventoryStateChangedEvent?.Invoke(sender);

        if (amountleft <= 0)
        {
            return true;
        }
        item.state.amount = amountleft;
        return TryToAdd(sender, item);
    }
    public IInventorySlot[] GetAllSlots(Type itemType)
    {
        return _slots.FindAll(slot => !slot.isEmpty && slot.itemType == itemType).ToArray();
    }
    public IInventorySlot[] GetAllSlots()
    {
        return _slots.ToArray();
    }
}
