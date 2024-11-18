using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Money")]
    [Tooltip("The starting amount of money the player has.")]
    [SerializeField] private int m_initialMoney;

    private int m_currentMoney;

    /// <summary>
    /// The current amount of money the player has.
    /// </summary>
    public int CurrentMoney => m_currentMoney;

    [Header("Item Costs")]
    [Tooltip("The initial cost of the item.")]
    [SerializeField] private int m_initialItemCost;

    [Tooltip("The growth rate of the item's cost after each purchase (e.g., 1.15 for a 15% increase).")]
    [SerializeField] private float m_itemCostGrowthRate = 1.15f;

    private int m_currentItemCost;

    [Tooltip("The number of times the item has been purchased.")]
    private int m_itemPurchases = 0;

    [Header("Item Data")]
    [Tooltip("The data associated with the item, such as its name or description.")]
    [SerializeField] private ItemData m_itemData;

    public event Action<int> MoneyChanged;
    public event Action<int> ItemCostChanged;

    public void Start()
    {
        m_currentMoney = m_initialMoney;
        MoneyChanged?.Invoke(m_currentMoney);

        m_currentItemCost = m_initialItemCost;
        ItemCostChanged?.Invoke(m_currentItemCost);
    }

    public bool BuyItem(out ItemData itemData)
    {
        if (m_itemData == null)
        {
            Debug.LogWarning("No item data specified, make sure to reference in the inspector!");
            itemData = null;
            return false;
        }

        if (CanAfford(m_currentItemCost))
        {
            m_currentMoney -= m_currentItemCost;
            MoneyChanged?.Invoke(m_currentMoney);

            m_itemPurchases++;
            m_currentItemCost = Mathf.CeilToInt(m_initialItemCost * Mathf.Pow(m_itemCostGrowthRate, m_itemPurchases));
            ItemCostChanged?.Invoke(m_currentItemCost);

            itemData = m_itemData;
            return true;
        }

        itemData = null;
        return false;
    }

    public bool CanAfford(int cost)
    {
        return m_currentMoney >= cost;
    }
}
