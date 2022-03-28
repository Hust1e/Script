using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterUI
{
    private InventoryItemInfo _appleInfo;
    private InventoryItemInfo _pepperinfo;
    private UIInventorySlot[] _uISlots;

    public InventoryWithSlots inventory { get; }

    public TesterUI(InventoryItemInfo appleInfo, InventoryItemInfo pepperInfo, UIInventorySlot[] uiSlots)
    {
        _appleInfo = appleInfo;
        _pepperinfo = pepperInfo;
        _uISlots = uiSlots;

        inventory = new InventoryWithSlots(21);
        inventory.OnInventoryStateChangedEvent += OnInventoryStateChanged;
    }

    public void FillSlots()
    {
        var allslots = inventory.GetAllSlots();
        var availibleSlots = new List<IInventorySlot>(allslots);

        var filledSlots = 5;
        for (int i = 0; i < filledSlots; i++)
        {
            var filledSlot = AddRandomApplesInRandomSlot(availibleSlots);
            availibleSlots.Remove(filledSlot);

            filledSlot = AddRandomPeppersInRandomSlot(availibleSlots);
            availibleSlots.Remove(filledSlot);
        }
        SetUpInventoryUI(inventory);
    }
    
    public IInventorySlot AddRandomApplesInRandomSlot(List<IInventorySlot> slots)
    {
        var rSlotsIndex = Random.Range(0, slots.Count);
        var rSlot = slots[rSlotsIndex];
        var rCount = Random.Range(1, 4);
        var apple = new Apple(_appleInfo);
        apple.state.amount = rCount;
        inventory.TryToAddToSlot(this, rSlot, apple);
        return rSlot;
    }

    public IInventorySlot AddRandomPeppersInRandomSlot(List<IInventorySlot> slots)
    {
        var rSlotsIndex = Random.Range(0, slots.Count);
        var rSlot = slots[rSlotsIndex];
        var rCount = Random.Range(1, 4);
        var pepper = new Pepper(_pepperinfo);
        pepper.state.amount = rCount;
        inventory.TryToAddToSlot(this, rSlot, pepper);
        return rSlot;
    }

    private void SetUpInventoryUI(InventoryWithSlots inventory)
    {
        var allSlots = inventory.GetAllSlots();
        int allSlotsCount = allSlots.Length;
        for(int i = 0; i < allSlotsCount; i++)
        {
            var slot = allSlots[i];
            var uiSlot = _uISlots[i];
            uiSlot.SetSlot(slot);
            uiSlot.Refresh();
        }
    }

    private void OnInventoryStateChanged(object sender)
    {
        foreach (var uiSlot in _uISlots)
        {
            uiSlot.Refresh();
        }
    } 
}
