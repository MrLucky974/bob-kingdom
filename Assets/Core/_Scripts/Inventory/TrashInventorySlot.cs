using UnityEngine;
using UnityEngine.EventSystems;

public class TrashInventorySlot : MonoBehaviour, IDropHandler
{
    private const float COST_MULTIPLIER = 0.5f;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject.TryGetComponent<DraggableItem>(out _))
        {
            var currentItemCost = Player.Instance.CurrentItemCost;
            Player.Instance.GiveMoney((ulong)Mathf.RoundToInt(currentItemCost * COST_MULTIPLIER));
            Destroy(droppedObject);
        }
    }
}
