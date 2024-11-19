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

    private void Start()
    {
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
        var upgrade = UpgradeManager.Instance.GetUpgrade(m_upgradeData);

        if (upgrade.CurrentLevel >= m_upgradeData.MaxLevel)
        {
            return;
        }

        upgrade.ApplyUpgrade();
        for (int i = 0; i < m_upgradeData.MaxLevel; i++)
        {
            var image = m_upgradeIndices[i];
            if (image != null)
            {
                var color = i < upgrade.CurrentLevel ? m_upgradeColor : m_defaultColor;
                image.color = color;
            }
        }

        if (upgrade.CurrentLevel >= m_upgradeData.MaxLevel)
        {
            m_button.interactable = false;
        }
    }
}
