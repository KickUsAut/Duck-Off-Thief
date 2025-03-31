// CraftingUI.cs – komplette Version mit Vorschau & Craft-Button + UI-Toggle mit Inventar

using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public static CraftingUI Instance;

    [Header("Crafting Slots")]
    public GameObject inputSlot1;
    public GameObject inputSlot2;
    public GameObject resultSlot;
    public GameObject craftingPanel; // Panel für gesamtes Crafting-UI

    private Image icon1;
    private Image icon2;
    private Image resultIcon;

    private string id1;
    private string id2;

    private void Awake()
    {
        Instance = this;

        icon1 = inputSlot1.transform.Find("ItemIcon").GetComponent<Image>();
        icon2 = inputSlot2.transform.Find("ItemIcon").GetComponent<Image>();
        resultIcon = resultSlot.transform.Find("ItemIcon").GetComponent<Image>();

        // Panel am Anfang verstecken
        craftingPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isActive = !craftingPanel.activeSelf;
            craftingPanel.SetActive(isActive);
        }
    }

    public void AddIngredient(string id, Sprite sprite)
    {
        if (string.IsNullOrEmpty(id1))
        {
            id1 = id;
            icon1.sprite = sprite;
            icon1.enabled = true;
        }
        else if (string.IsNullOrEmpty(id2))
        {
            id2 = id;
            icon2.sprite = sprite;
            icon2.enabled = true;
        }
        else
        {
            Debug.Log("Beide Crafting-Slots voll");
            return;
        }

        UpdatePreview();
    }

    private void UpdatePreview()
    {
        if (string.IsNullOrEmpty(id1) || string.IsNullOrEmpty(id2))
        {
            resultIcon.enabled = false;
            return;
        }

        string resultID = InventoryManager.Instance.FindCraftResult(id1, id2);

        if (!string.IsNullOrEmpty(resultID))
        {
            Sprite resultSprite = InventoryManager.Instance.GetItemIcon(resultID);
            resultIcon.sprite = resultSprite;
            resultIcon.enabled = true;
        }
        else
        {
            resultIcon.sprite = null;
            resultIcon.enabled = false;
        }
    }

    public void Craft()
    {
        string resultID = InventoryManager.Instance.FindCraftResult(id1, id2);

        if (!string.IsNullOrEmpty(resultID))
        {
            Sprite resultSprite = InventoryManager.Instance.GetItemIcon(resultID);
            resultIcon.sprite = resultSprite;
            resultIcon.enabled = true;

            // Zutaten aus Inventar entfernen
            InventoryManager.Instance.RemoveItem(id1);
            InventoryManager.Instance.RemoveItem(id2);

            bool added = InventoryManager.Instance.AddItem(resultID);
            if (added)
            {
                Debug.Log("Gecraftet: " + resultID);
                ClearSlots();
            }
            else
            {
                Debug.Log("Inventar voll, Ergebnis nicht gespeichert.");
            }
        }
        else
        {
            Debug.Log("Ungültige Kombination.");
        }
    }

    public void ClearSlots()
    {
        id1 = null;
        id2 = null;

        icon1.sprite = null;
        icon2.sprite = null;
        icon1.enabled = false;
        icon2.enabled = false;

        resultIcon.sprite = null;
        resultIcon.enabled = false;
    }
}