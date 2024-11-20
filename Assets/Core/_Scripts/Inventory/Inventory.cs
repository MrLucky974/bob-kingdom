using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> m_slots = new List<InventorySlot>();

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);

            if (!child.TryGetComponent<InventorySlot>(out var slot))
                return;

            if (m_slots.Contains(slot))
                return;

            m_slots.Add(slot);
        }
    }

    public List<InventorySlot> GetAllSlots()
    {
        return new List<InventorySlot>(m_slots);
    }

    public InventorySlot GetFirstAvailableSlot()
    {
        for (int i = 0; i < m_slots.Count; i++)
        {
            var slot = m_slots[i];
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }

        return null;
    }

    public InventorySlot GetRandomAvailableSlot()
    {
        List<InventorySlot> availableSlots = new List<InventorySlot>();
        for (int i = 0; i < m_slots.Count; i++)
        {
            var slot = m_slots[i];
            if (slot.transform.childCount == 0)
            {
                availableSlots.Add(slot);
            }
        }

        if (availableSlots.Count == 0)
        {
            return null;
        }

        var randomIndex = Random.Range(0, availableSlots.Count);
        return availableSlots[randomIndex];
    }
}
