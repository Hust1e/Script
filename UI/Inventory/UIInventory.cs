using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [SerializeField] private InventoryItemInfo _appleinfo;
    [SerializeField] private InventoryItemInfo _pepperinfo;

    public InventoryWithSlots _inventory => tester.inventory;

    private TesterUI tester;    

    private void Start()
    {
        var uiSlots = GetComponentsInChildren<UIInventorySlot>();
        tester = new TesterUI(_appleinfo, _pepperinfo, uiSlots);
        tester.FillSlots();
    }
}
