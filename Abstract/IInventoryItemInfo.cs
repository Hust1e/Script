using UnityEngine;

public interface IInventoryItemInfo
{
    string id { get; }
    string title { get; }
    string description { get; }
    int maxItemInSlot { get; }
    Sprite itemIcon { get; }
}
