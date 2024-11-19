using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private Button m_button;

    [Header("Display")]
    [SerializeField] private Color m_defaultColor;
    [SerializeField] private Color m_upgradeColor;

    [Space]

    [SerializeField] private RectTransform m_indicesContainer;
    [SerializeField] private Image m_upgradeIndexTemplate;

    private int m_maxUpgradeLevel;
    private int m_currentUpgradeLevel = 0;

    private Image[] m_upgradeIndices;

    private void Start()
    {
        m_maxUpgradeLevel = Random.Range(1, 5);

        m_upgradeIndices = new Image[m_maxUpgradeLevel];
        for (int i = 0; i < m_maxUpgradeLevel; i++)
        {
            var instance = Instantiate(m_upgradeIndexTemplate, m_indicesContainer);
            instance.name = $"Index_{i:000}";
            instance.gameObject.SetActive(true);
            m_upgradeIndices[i] = instance;
        }
    }

    public void Upgrade()
    {
        if (m_currentUpgradeLevel >= m_maxUpgradeLevel)
        {
            m_currentUpgradeLevel = m_maxUpgradeLevel;
            return;
        }

        m_currentUpgradeLevel++;
        for (int i = 0; i < m_maxUpgradeLevel; i++)
        {
            var image = m_upgradeIndices[i];
            if (image != null)
            {
                var color = i < m_currentUpgradeLevel ? m_upgradeColor : m_defaultColor;
                image.color = color;
            }
        }

        if (m_currentUpgradeLevel >= m_maxUpgradeLevel)
        {
            m_button.interactable = false;
            m_currentUpgradeLevel = m_maxUpgradeLevel;
        }
    }
}
