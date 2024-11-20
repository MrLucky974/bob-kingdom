using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Data", menuName = "Bob's Kingdom/New Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [SerializeField] private string m_upgradeName;
    public string UpgradeName => m_upgradeName;

    [SerializeField, Multiline] private string m_description;
    public string Description => m_description;

    [SerializeField] private Sprite m_icon;
    public Sprite Icon => m_icon;

    [SerializeField] private int m_maxLevel = 1;
    public int MaxLevel => m_maxLevel;

    [SerializeField] private int m_initialCost = 1;

    [Tooltip("The growth rate of the upgrades's cost after each purchase (e.g., 1.15 for a 15% increase).")]
    [SerializeField] private float m_costGrowthRate = 1.15f;

    public int GetCost(int level)
    {
        return Mathf.RoundToInt(m_initialCost * Mathf.Pow(m_costGrowthRate, level));
    }
}
