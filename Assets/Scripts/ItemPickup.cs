using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            bool added = InventoryManager.Instance.AddItem(itemID);
            if (added)
            {
                Destroy(gameObject);
            }
        }
    }
}
