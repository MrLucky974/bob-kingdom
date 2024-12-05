using LuckiusDev.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeShortcut : MonoBehaviour
{
    [Header("Upgrade")]
    [SerializeField] private UpgradeData m_upgradeData;

    [Header("Interaction")]
    [SerializeField] private Button m_button;

    [Space]

    [SerializeField] private Image m_upgradeIcon;
    [SerializeField] private TextMeshProUGUI m_costLabel;

    private Upgrade m_upgrade;

    private void Start()
    {
        m_upgrade = UpgradeManager.Instance.GetUpgrade(m_upgradeData);

        m_upgrade.UpgradeRefresh += HandleRefresh;

        m_upgradeIcon.sprite = m_upgradeData.Icon;
        m_costLabel.SetText($"{NumberFormatter.FormatNumberWithSuffix(m_upgrade.GetUpgradeCost())}");
    }

    private void HandleRefresh()
    {
        //m_costLabel.gameObject.SetActive(m_upgrade.CanUpgrade());
        m_button.interactable = m_upgrade.CanUpgrade();
    }

    public void Upgrade()
    {
        if (!m_upgrade.CanUpgrade())
        {
            return;
        }

        //if (!m_costLabel.gameObject.activeInHierarchy)
        //    m_costLabel.gameObject.SetActive(true);

        if (!m_button.interactable)
            m_button.interactable = true;

        ulong cost = (ulong)m_upgrade.GetUpgradeCost();
        bool canAfford = Player.Instance.ConsumeMoney(cost);
        if (canAfford)
        {
            bool couldApply = m_upgrade.ApplyUpgrade();
            if (couldApply)
            {
                m_costLabel.SetText($"Cost: {NumberFormatter.FormatNumberWithSuffix(m_upgrade.GetUpgradeCost())}");

                if (!m_upgrade.CanUpgrade())
                {
                    //m_costLabel.gameObject.SetActive(false);
                    m_button.interactable = false;
                }
            }
        }
    }
}
