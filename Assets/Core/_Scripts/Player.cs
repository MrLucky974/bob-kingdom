using LuckiusDev.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : Singleton<Player>
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

    [Serializable]
    public class ItemDataContainer
    {
        [SerializeField] public List<ItemData> items;
    }

    [Header("Item Data")]
    [SerializeField] private UpgradeData m_itemUpgradeData;
    [SerializeField] private List<ItemDataContainer> m_containers;

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
        if (m_containers == null || m_containers.Count < 1)
        {
            Debug.LogWarning("No containers specified, make sure to reference in the inspector!");
            itemData = null;
            return false;
        }

        var upgrade = UpgradeManager.Instance.GetUpgrade(m_itemUpgradeData);
        int containerLevel = upgrade.CurrentLevel;
        if (containerLevel >= m_containers.Count)
        {
            containerLevel = m_containers.Count - 1;
        }

        var currentContainer = m_containers[containerLevel];
        var items = currentContainer.items;

        if (items == null || items.Count < 1)
        {
            Debug.LogWarning("No items specified for current container, make sure to reference in the inspector!");
            itemData = null;
            return false;
        }

        if (ConsumeMoney(m_currentItemCost))
        {
            m_itemPurchases++;
            m_currentItemCost = Mathf.CeilToInt(m_initialItemCost * Mathf.Pow(m_itemCostGrowthRate, m_itemPurchases));
            ItemCostChanged?.Invoke(m_currentItemCost);

            var randomIndex = Random.Range(0, items.Count);
            itemData = items[randomIndex];
            return true;
        }

        itemData = null;
        return false;
    }

    public bool ConsumeMoney(int amount)
    {
        if (CanAfford(amount))
        {
            m_currentMoney -= amount;
            MoneyChanged?.Invoke(m_currentMoney);
            return true;
        }

        return false;
    }

    public bool CanAfford(int cost)
    {
        return m_currentMoney >= cost;
    }
}
