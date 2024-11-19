using LuckiusDev.Utils;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [SerializeField] private UpgradeData[] m_upgradesData;

    private Dictionary<UpgradeData, Upgrade> m_upgrades = new();

    protected override void Awake()
    {
        base.Awake();
        InitializeUpgrades();
    }

    private void InitializeUpgrades()
    {
        foreach (var data in m_upgradesData)
        {
            var upgrade = new Upgrade(data);
            m_upgrades[data] = upgrade;
        }
    }

    public Upgrade GetUpgrade(UpgradeData data)
    {
        return m_upgrades.TryGetValue(data, out var upgrade) ? upgrade : null;
    }
}
