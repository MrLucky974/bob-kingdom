using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    [Header("Upgrade")]
    [SerializeField] private UpgradeData m_upgradeData;

    [Header("Interaction")]
    [SerializeField] private Button m_button;

    [Header("Display")]
    [SerializeField] private Color m_defaultColor;
    [SerializeField] private Color m_upgradeColor;

    [Space]

    [SerializeField] private RectTransform m_indicesContainer;
    [SerializeField] private Image m_upgradeIndexTemplate;

    [Space]

    [SerializeField] private TextMeshProUGUI m_nameLabel;
    [SerializeField] private TextMeshProUGUI m_descriptionLabel;
    [SerializeField] private TextMeshProUGUI m_costLabel;

    private Image[] m_upgradeIndices;

    private Upgrade m_upgrade;

    private void Start()
    {
        m_upgrade = UpgradeManager.Instance.GetUpgrade(m_upgradeData);

        m_nameLabel.SetText(m_upgradeData.UpgradeName);
        m_descriptionLabel.SetText(m_upgradeData.Description);
        m_costLabel.SetText($"Cost: {m_upgrade.GetUpgradeCost()}");

        m_upgradeIndices = new Image[m_upgradeData.MaxLevel];
        for (int i = 0; i < m_upgradeData.MaxLevel; i++)
        {
            var instance = Instantiate(m_upgradeIndexTemplate, m_indicesContainer);
            instance.name = $"Index_{i:000}";
            instance.gameObject.SetActive(true);
            m_upgradeIndices[i] = instance;
        }
    }

    public void Upgrade()
    {
        if (m_upgrade.IsMaxed)
        {
            return;
        }

        int cost = m_upgrade.GetUpgradeCost();
        bool canAfford = Player.Instance.ConsumeMoney(cost);
        if (canAfford)
        {
            m_upgrade.ApplyUpgrade();

            for (int i = 0; i < m_upgradeData.MaxLevel; i++)
            {
                var image = m_upgradeIndices[i];
                if (image != null)
                {
                    var color = i < m_upgrade.CurrentLevel ? m_upgradeColor : m_defaultColor;
                    image.color = color;
                }
            }
            m_costLabel.SetText($"Cost: {m_upgrade.GetUpgradeCost()}");

            if (m_upgrade.IsMaxed)
            {
                m_costLabel.gameObject.SetActive(false);
                m_button.interactable = false;
            }
        }
    }
}
