using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private ItemData m_itemData;
    [SerializeField] private DraggableItem m_draggableItemPrefab;

    [Space]

    [SerializeField] private Inventory m_inventory;

    public void MergeAllItems()
    {
        var slots = m_inventory.GetAllSlots();

        // Dictionary to track the count of items by their data
        Dictionary<ItemData, (GameObject slotObject, Transform parentSlot)> itemTracker = new();

        foreach (var slot in slots)
        {
            if (slot.transform.childCount == 0) // Skip empty slots
                continue;

            var slotObject = slot.transform.GetChild(0).gameObject;
            var slotItem = slotObject.GetComponent<DraggableItem>();

            if (slotItem == null || slotItem.ItemData == null) // Skip invalid items
                continue;

            if (itemTracker.ContainsKey(slotItem.ItemData))
            {
                // Merge items if another one of the same type exists
                var (existingObject, parentSlot) = itemTracker[slotItem.ItemData];

                var nextItemTier = slotItem.ItemData.NextItemTier;
                if (nextItemTier != null)
                {
                    // Destroy both items
                    Destroy(existingObject);
                    Destroy(slotObject);

                    // Create the next tier item
                    var newInstance = m_draggableItemPrefab.Create(nextItemTier);
                    newInstance.transform.SetParent(parentSlot);
                    newInstance.transform.localPosition = Vector3.zero; // Reset position

                    // Remove from tracker to allow further merges
                    itemTracker.Remove(slotItem.ItemData);

                    // Add new item to the tracker
                    itemTracker[nextItemTier] = (newInstance.gameObject, parentSlot);
                }
            }
            else
            {
                // Track this item
                itemTracker[slotItem.ItemData] = (slotObject, slot.transform);
            }
        }
    }

    public void BuyItem()
    {
        if (m_inventory == null)
        {
            Debug.LogWarning("No inventory specified, make sure to reference in the inspector!");
            return;
        }

        if (m_draggableItemPrefab == null)
        {
            Debug.LogWarning("No prefab specified, make sure to reference in the inspector!");
            return;
        }

        if (m_itemData == null)
        {
            Debug.LogWarning("No item data specified, make sure to reference in the inspector!");
            return;
        }

        var slot = m_inventory.GetRandomAvailableSlot();
        if (slot == null) // TODO : Disable button when no available slot
        {
            Debug.LogWarning("No slot available!");
            return;
        }

        var instance = m_draggableItemPrefab.Create(m_itemData);
        instance.transform.SetParent(slot.transform);
    }
}
