using System;

public interface IInventorySlot
{
    bool isEmpty { get; }
    bool isFull { get; }
    
    IInventoryItem item { get; }
    Type itemType { get; }
    int amount { get; }
    int capacity { get; }

    void SetItem(IInventoryItem item);
    void Clear();
}
