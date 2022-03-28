using UnityEngine;
using UnityEngine.EventSystems;

public class UIInventorySlot : UISlot
{
    [SerializeField] UIInventoryItem _uiInventoryItem;

    public IInventorySlot slot { get; private set; }

    private UIInventory _uiInventory;

    private void Awake()
    {
        _uiInventory = GetComponentInParent<UIInventory>();
    }

    public void SetSlot(IInventorySlot newSlot)
    {
        slot = newSlot;
    }

    public override void OnDrop(PointerEventData eventData)
    {
        var otherItemUI = eventData.pointerDrag.GetComponent<UIInventoryItem>();
        var otherSlotUI = otherItemUI.GetComponentInParent<UIInventorySlot>();
        var otherSlot = otherSlotUI.slot;
        var inventory = _uiInventory._inventory;

        inventory.TransFromSlotToSlot(this, otherSlot, slot);

        Refresh();
        otherSlotUI.Refresh();
        //ќбновл€ем €чейку слота
    }

    public void Refresh()
    {
        if(slot != null)
        {
            _uiInventoryItem.Refresh(slot);
        }
    }
}
