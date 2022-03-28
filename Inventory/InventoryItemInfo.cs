using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemInfo", menuName = "Gameplay/Items/Create New item info")]
public class InventoryItemInfo : ScriptableObject, IInventoryItemInfo
{
    [SerializeField] private string _id;
    [SerializeField] private string _title;
    [SerializeField] private string _description;
    [SerializeField] private int _maxItemInSlot;
    [SerializeField] private Sprite _icon;

    public string id => _id;

    public string title => _title;

    public string description => _description;

    public int maxItemInSlot => _maxItemInSlot;

    public Sprite itemIcon => _icon;
}
