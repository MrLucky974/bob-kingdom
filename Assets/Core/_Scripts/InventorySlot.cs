using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
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
            var slotObject = transform.GetChild(0);
            DraggableItem slotItem = slotObject.GetComponent<DraggableItem>();

            if (slotItem == null)
                return;


        }
    }
}
