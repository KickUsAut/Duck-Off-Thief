using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("UI & Slots")]
    public GameObject inventoryUI;
    public InventorySlot[] slots; // manuell zugewiesen im Inspector

    [Header("Item-Datenbank")]
    public List<ItemData> itemDatabase; // Liste von ID + Sprite
    private Dictionary<string, Sprite> itemIcons = new Dictionary<string, Sprite>();

    [Header("Crafting-Rezepte")]
    public List<CraftingRecipe> recipes; // Rezeptliste

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Icons aus Datenbank vorbereiten
        foreach (ItemData item in itemDatabase)
        {
            if (!itemIcons.ContainsKey(item.id))
                itemIcons.Add(item.id, item.icon);
        }
    }

    private void Start()
    {
        foreach (InventorySlot slot in slots)
        {
            slot.ClearSlot();
        }

        inventoryUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool isActive = !inventoryUI.activeSelf;
            inventoryUI.SetActive(isActive);

            // Maus zeigen/verstecken
            Cursor.visible = isActive;
            Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Craft("shovel"); // Test
        }
    }

    public bool AddItem(string id)
    {
        if (!itemIcons.ContainsKey(id))
        {
            Debug.LogWarning("Kein Icon gefunden für ID: " + id);
            return false;
        }

        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsFull)
            {
                slot.SetItem(id, itemIcons[id]);
                Debug.Log("Item hinzugefügt: " + id);
                return true;
            }
        }

        Debug.Log("Inventar voll!");
        return false;
    }


    public void Craft(string resultID)
    {
        CraftingRecipe recipe = recipes.Find(r => r.resultID == resultID);
        if (recipe == null)
        {
            Debug.LogWarning("Rezept nicht gefunden: " + resultID);
            return;
        }

        List<InventorySlot> usedSlots = new List<InventorySlot>();

        // Prüfen, ob Zutaten vorhanden sind
        foreach (string ingredient in recipe.ingredientIDs)
        {
            InventorySlot foundSlot = System.Array.Find(slots, s => s.IsFull && s.ItemID == ingredient && !usedSlots.Contains(s));
            if (foundSlot != null)
            {
                usedSlots.Add(foundSlot);
            }
            else
            {
                Debug.Log("Nicht genug Zutaten für: " + resultID);
                return;
            }
        }

        // Zutaten entfernen
        foreach (InventorySlot slot in usedSlots)
        {
            slot.ClearSlot();
        }

        // Ergebnis hinzufügen
        bool added = AddItem(resultID);
        if (added)
        {
            Debug.Log("Gecraftet: " + resultID);
        }
    }

    public string FindCraftResult(string id1, string id2)
    {
        foreach (CraftingRecipe recipe in recipes)
        {
            string[] ing = recipe.ingredientIDs;

            if ((ing[0] == id1 && ing[1] == id2) || (ing[0] == id2 && ing[1] == id1))
            {
                return recipe.resultID;
            }
        }

        return null;
    }

    public Sprite GetItemIcon(string id)
    {
        if (itemIcons.ContainsKey(id))
        {
            return itemIcons[id];
        }
        return null;
    }

    public void RemoveItem(string id)
{
    foreach (InventorySlot slot in slots)
    {
        if (slot.IsFull && slot.ItemID == id)
        {
            slot.ClearSlot();
            return;
        }
    }

    Debug.LogWarning("Item nicht gefunden zum Entfernen: " + id);
}


}
