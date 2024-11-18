using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private DraggableItem m_draggableItemPrefab;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        DraggableItem item = droppedObject.GetComponent<DraggableItem>();

        if (transform.childCount == 0) // Slot is empty
        {
            if (item == null)
                return;

            item.SetParentAfterDrag(transform);
        }
        else // Slot already has an item
        {
            var slotObject = transform.GetChild(0).gameObject;

            if (!slotObject.TryGetComponent<DraggableItem>(out var slotItem))
                return;

            if (slotItem.ItemData == null || item.ItemData == null)
                return;

            // Check if both items are the same
            if (slotItem.ItemData == item.ItemData)
            {
                var nextItemTier = slotItem.ItemData.NextItemTier;

                if (nextItemTier != null)
                {
                    Destroy(slotObject);
                    Destroy(droppedObject);

                    var instance = m_draggableItemPrefab.Create(nextItemTier);
                    instance.transform.SetParent(transform);
                }
            }
        }
    }
}
