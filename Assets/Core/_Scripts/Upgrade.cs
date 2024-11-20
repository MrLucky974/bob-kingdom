using System;
using UnityEngine;

public class Upgrade
{
    public UpgradeData Data { get; private set; }
    public int CurrentLevel { get; private set; } = 0;

    public bool IsMaxed => CurrentLevel >= Data.MaxLevel;

    public event Action<Upgrade> UpgradeLevelUp;

    public Upgrade(UpgradeData data)
    {
        Data = data;
    }

    public void ApplyUpgrade()
    {
        if (IsMaxed)
        {
            Debug.LogWarning($"{Data.UpgradeName} is already maxed out!");
            return;
        }

        CurrentLevel++;
        UpgradeLevelUp?.Invoke(this);
    }

    public int GetUpgradeCost()
    {
        return IsMaxed ? 0 : Data.GetCost(CurrentLevel);
    }
}
