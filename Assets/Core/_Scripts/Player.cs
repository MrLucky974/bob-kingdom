using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int m_initialMoney;
    private int m_currentMoney;
    public int CurrentMoney => m_currentMoney;

    [SerializeField] private int m_initalItemCost;

    [SerializeField] private ItemData m_itemData;

    private int m_currentItemCost;

    public event Action<int> MoneyChanged;
    public event Action<int> ItemCostChanged;


    public void Start()
    {
        m_currentMoney = m_initialMoney;
        MoneyChanged?.Invoke(m_currentMoney);

        m_currentItemCost = m_initalItemCost;
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

            m_currentItemCost++;
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
