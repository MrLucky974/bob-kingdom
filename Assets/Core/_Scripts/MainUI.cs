using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private ItemData m_itemData;
    [SerializeField] private DraggableItem m_draggableItemPrefab;

    [Space]

    [SerializeField] private Inventory m_inventory;

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
