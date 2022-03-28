using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : UIItem
{
    [SerializeField] private Image _imageIcon;
    [SerializeField] private Text _textAmount;

    public IInventoryItem item { get; private set; }

    public void Refresh(IInventorySlot slot)
    {
        if (slot.isEmpty)
        {
            CleanUp();
            return;
        }

        item = slot.item;
        _imageIcon.sprite = item.info.itemIcon;
        _imageIcon.gameObject.SetActive(true);
        _textAmount.gameObject.SetActive(true);

        var textAmountEnabled = slot.amount > 1;
        _textAmount.gameObject.SetActive(textAmountEnabled);

        if (_textAmount)
        {
            _textAmount.text = $"x{slot.amount}";
        }

    }
    private void CleanUp()
    {
        _textAmount.gameObject.SetActive(false);
        _imageIcon.gameObject.SetActive(false);
    }
}
