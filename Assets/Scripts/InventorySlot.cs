using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    private string itemID;
    private bool isFull = false;

    public bool IsFull => isFull;
    public string ItemID => itemID;

    public void SetItem(string id, Sprite itemIcon)
    {
        itemID = id;
        icon.sprite = itemIcon;
        icon.enabled = true;
        isFull = true;
    }

    public void ClearSlot()
    {
        itemID = null;
        icon.sprite = null;
        icon.enabled = false;
        isFull = false;
    }

    public void OnClick()
    {
        if (isFull)
        {
            // Hier: Nur ID + Icon übergeben, Slot selbst bleibt!
            CraftingUI.Instance.AddIngredient(itemID, icon.sprite);
        }
    }
}
