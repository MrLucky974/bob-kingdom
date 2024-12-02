using LuckiusDev.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_moneyLabel;
    [SerializeField] private TextMeshProUGUI m_itemCostLabel;

    [Space]

    [SerializeField] private CanvasGroup m_upgradePanel;
    [SerializeField] private GameObject m_block;

    [Space]

    [SerializeField] private UpgradeData m_autoMergeUpgradeData;
    [SerializeField] private Button m_autoMergeButton;

    private Upgrade m_autoMergeUpgrade;

    private Player m_player;
    private Inventory m_inventory;

    private void Awake()
    {
        m_autoMergeUpgrade = UpgradeManager.Instance.GetUpgrade(m_autoMergeUpgradeData);
        m_autoMergeUpgrade.UpgradeLevelUp += OnAutoMergeUpgrade;
        m_autoMergeButton.interactable = false;

        m_player = Player.Instance;
        m_inventory = m_player.Inventory;

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

    private void OnAutoMergeUpgrade()
    {
        m_autoMergeButton.interactable = true;
    }

    private void OnMoneyChanged(ulong money)
    {
        m_moneyLabel.SetText(NumberFormatter.FormatNumberWithSuffix(money));
    }

    private void OnItemCostChanged(ulong cost)
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
        if (m_autoMergeUpgrade.CurrentLevel >= 2)
        {
            StartCoroutine(MergeItemsRecursively());
        }
        else
        {
            MergeItemsOnce();
        }
    }

    private IEnumerator MergeItemsRecursively()
    {
        bool itemsMerged = false;
        do
        {
            yield return MergeItemsStep((value) => itemsMerged = value);
        }
        while (itemsMerged);
    }

    private IEnumerator MergeItemsStep(Action<bool> callback)
    {
        callback(MergeItemsOnce());
        yield return null;
    }

    private bool MergeItemsOnce()
    {
        if (m_inventory == null)
        {
            Debug.LogWarning("No inventory specified, make sure to reference in the inspector!");
            return false;
        }

        if (SceneReferences.draggableItemPrefab == null)
        {
            Debug.LogWarning("No prefab specified, make sure to reference in the inspector!");
            return false;
        }

        bool itemsMerged = false; // Track if any merges happened in this iteration
        var slots = m_inventory.GetAllSlots();

        // Dictionary to track the count of items by their data
        Dictionary<ItemData, (GameObject slotObject, Transform parentSlot)> itemTracker = new();

        int slotIndex = -1;
        foreach (var slot in slots)
        {
            slotIndex += 1;

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
                    var newInstance = SceneReferences.draggableItemPrefab.Create(nextItemTier);
                    newInstance.transform.SetParent(parentSlot);
                    newInstance.transform.localPosition = Vector3.zero; // Reset position

                    // Remove from tracker to allow further merges
                    itemTracker.Remove(slotItem.ItemData);

                    itemsMerged = true; // A merge happened

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

        return itemsMerged;
    }

    public void BuyItem()
    {
        if (m_inventory == null)
        {
            Debug.LogWarning("No inventory specified, make sure to reference in the inspector!");
            return;
        }

        if (SceneReferences.draggableItemPrefab == null)
        {
            Debug.LogWarning("No prefab specified, make sure to reference in the inspector!", SceneReferences.Instance);
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
            var instance = SceneReferences.draggableItemPrefab.Create(itemData);
            instance.transform.SetParent(slot.transform);
        }
    }
}
