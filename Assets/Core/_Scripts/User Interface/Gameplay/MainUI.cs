using LuckiusDev.Utils;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] private DraggableItem m_draggableItemPrefab;

    [Space]

    [SerializeField] private Player m_player;
    [SerializeField] private Inventory m_inventory;

    [Space]

    [SerializeField] private TextMeshProUGUI m_moneyLabel;
    [SerializeField] private TextMeshProUGUI m_itemCostLabel;

    [Space]

    [SerializeField] private CanvasGroup m_upgradePanel;
    [SerializeField] private GameObject m_block;

    [Space]

    [SerializeField] private UpgradeData m_autoMergeUpgradeData;
    [SerializeField] private Button m_autoMergeButton;

    private Upgrade m_autoMergeUpgrade;

    private void Awake()
    {
        m_autoMergeUpgrade = UpgradeManager.Instance.GetUpgrade(m_autoMergeUpgradeData);
        m_autoMergeUpgrade.UpgradeLevelUp += OnAutoMergeUpgrade;
        m_autoMergeButton.interactable = false;

        m_player.MoneyChanged += OnMoneyChanged;
        m_player.ItemCostChanged += OnItemCostChanged;
    }

    private void OnDestroy()
    {
        m_autoMergeUpgrade.UpgradeLevelUp -= OnAutoMergeUpgrade;

        m_player.MoneyChanged -= OnMoneyChanged;
        m_player.ItemCostChanged -= OnItemCostChanged;
    }

    #region Event Calls

    private void OnAutoMergeUpgrade(Upgrade upgrade)
    {
        m_autoMergeButton.interactable = true;
    }

    private void OnMoneyChanged(int money)
    {
        m_moneyLabel.SetText(NumberFormatter.FormatNumberWithSuffix(money));
    }

    private void OnItemCostChanged(int cost)
    {
        m_itemCostLabel.SetText(NumberFormatter.FormatNumberWithSuffix(cost));
    }

    #endregion

    public void OpenUpgradePanel()
    {
        m_upgradePanel.alpha = 1f;
        m_upgradePanel.interactable = true;
        m_upgradePanel.blocksRaycasts = true;
        m_upgradePanel.GetComponent<RectTransform>().localScale = Vector3.one;
        m_block.SetActive(true);
    }

    public void CloseUpgradePanel()
    {
        m_upgradePanel.alpha = 0f;
        m_upgradePanel.interactable = false;
        m_upgradePanel.blocksRaycasts = false;
        m_upgradePanel.GetComponent<RectTransform>().localScale = Vector3.zero;
        m_block.SetActive(false);
    }

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

        var slot = m_inventory.GetRandomAvailableSlot();
        if (slot == null) // TODO : Disable button when no available slot
        {
            Debug.LogWarning("No slot available!");
            return;
        }

        if (m_player.BuyItem(out var itemData))
        {
            var instance = m_draggableItemPrefab.Create(itemData);
            instance.transform.SetParent(slot.transform);
        }
    }
}
