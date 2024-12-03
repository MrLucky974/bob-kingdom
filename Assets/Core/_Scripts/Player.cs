using LuckiusDev.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Collections;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class Player : Singleton<Player>
{
    [Header("References")]
    [SerializeField] private Inventory m_inventory;
    public Inventory Inventory => m_inventory;

    [Header("Player Money")]
    [Tooltip("The starting amount of money the player has.")]
    [SerializeField] private ulong m_initialMoney;
    public ulong InitialMoney => m_initialMoney;

    private ulong m_currentMoney;

    /// <summary>
    /// The current amount of money the player has.
    /// </summary>
    public ulong CurrentMoney => m_currentMoney;

    [Header("Item Costs")]
    [Tooltip("The initial cost of the item.")]
    [SerializeField] private ulong m_initialItemCost;
    public ulong InitialItemCost => m_initialItemCost;

    [Tooltip("The growth rate of the item's cost after each purchase (e.g., 1.15 for a 15% increase).")]
    [SerializeField] private float m_itemCostGrowthRate = 1.15f;
    public float ItemCostGrowthRate => m_itemCostGrowthRate;

    private ulong m_currentItemCost;
    public ulong CurrentItemCost => m_currentItemCost;

    [Tooltip("The number of times the item has been purchased.")]
    private int m_itemPurchases = 0;
    public int ItemPurchases => m_itemPurchases;

    [Serializable]
    public class ItemDataContainer
    {
        [SerializeField] public List<ItemData> items;
    }

    [Header("Item Data")]
    [SerializeField] private UpgradeData m_itemUpgradeData;
    private Upgrade m_itemUpgrade;

    [SerializeField] private List<ItemDataContainer> m_containers;

    [Space]

    [SerializeField] private UpgradeData m_hearthUpgradeData;
    private Upgrade m_hearthUpgrade;
    private Coroutine m_hearthTaxCoroutine;

    public event Action<ulong> MoneyChanged;
    public event Action<ulong> ItemCostChanged;

    public void Start()
    {
        m_itemUpgrade = UpgradeManager.Instance.GetUpgrade(m_itemUpgradeData);
        m_itemUpgrade.UpgradeLevelUp += HandleItemUpgradeLevelUp;

        m_hearthUpgrade = UpgradeManager.Instance.GetUpgrade(m_hearthUpgradeData);
        m_hearthUpgrade.UpgradeLevelUp += HandleHearthTaxLevelUp;

        m_currentMoney = m_initialMoney;
        MoneyChanged?.Invoke(m_currentMoney);

        m_currentItemCost = m_initialItemCost;
        ItemCostChanged?.Invoke(m_currentItemCost);
    }

    private void OnDestroy()
    {
        m_itemUpgrade.UpgradeLevelUp -= HandleItemUpgradeLevelUp;
        m_hearthUpgrade.UpgradeLevelUp -= HandleHearthTaxLevelUp;
    }

    private void HandleHearthTaxLevelUp()
    {
        if (m_hearthUpgrade.CurrentLevel < 1)
        {
            if (m_hearthTaxCoroutine != null)
            {
                StopCoroutine(m_hearthTaxCoroutine);
                m_hearthTaxCoroutine = null;
            }
            return;
        }

        m_hearthTaxCoroutine ??= StartCoroutine(nameof(HearthTaxLoop));
    }

    private IEnumerator HearthTaxLoop()
    {
        float[] values = new float[5] { 25f, 20f, 16f, 13f, 10f };
        while (true)
        {
            float waitTime = values[m_hearthUpgrade.CurrentLevel - 1];
            float currentTime = 0f;

            while (currentTime < waitTime)
            {
                waitTime = values[m_hearthUpgrade.CurrentLevel - 1];
                yield return null;
                currentTime += Time.deltaTime;
            }

            GiveMoney(1);
            Debug.Log($"[{name}] Gave 1 coin from hearth tax!", this);
        }
    }

    private void HandleItemUpgradeLevelUp()
    {
        m_currentItemCost = (ulong)Mathf.RoundToInt(m_initialItemCost * Mathf.Pow(m_itemCostGrowthRate, m_itemUpgrade.CurrentLevel));
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
            var randomIndex = Random.Range(0, items.Count);
            itemData = items[randomIndex];
            return true;
        }

        itemData = null;
        return false;
    }

    public void GiveMoney(ulong amount)
    {
        if (amount > 0)
        {
            m_currentMoney += amount;
            MoneyChanged?.Invoke(m_currentMoney);
        }
    }

    public bool ConsumeMoney(ulong amount)
    {
        if (CanAfford(amount))
        {
            m_currentMoney -= amount;
            MoneyChanged?.Invoke(m_currentMoney);
            return true;
        }

        return false;
    }

    public bool CanAfford(ulong cost)
    {
        return m_currentMoney >= cost;
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(Player)), CanEditMultipleObjects]
public class PlayerInspector : AutoRepaintingEditor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);

        Player player = (Player)target;

        StringBuilder sb = new();
        sb.AppendLine($"Initial Money: {NumberFormatter.FormatNumberWithSuffix(player.InitialMoney)} ({player.InitialMoney})");
        sb.Append($"Current Money: {NumberFormatter.FormatNumberWithSuffix(player.CurrentMoney)} ({player.CurrentMoney})");

        EditorGUILayout.HelpBox(sb.ToString(), MessageType.None);
    }
}

#endif