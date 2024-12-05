using System;
using UnityEngine;

public class Upgrade
{
    public UpgradeData Data { get; private set; }
    public int CurrentLevel { get; private set; } = 0;

    public bool IsMaxed => Data.MaxLevel != -1 && CurrentLevel >= Data.MaxLevel;

    public event Action UpgradeLevelUp;
    public event Action UpgradeRefresh;

    private Func<(bool isValid, string message)> m_customCheck;

    public Upgrade(UpgradeData data)
    {
        Data = data;
    }

    public void Refresh()
    {
        UpgradeRefresh?.Invoke();
    }

    public bool CanUpgrade()
    {
        if (IsMaxed)
        {
            return false;
        }

        if (m_customCheck != null)
        {
            var (isValid, _) = m_customCheck.Invoke();
            return !isValid;
        }

        return true;
    }

    public bool ApplyUpgrade()
    {
        if (IsMaxed)
        {
            Debug.LogWarning($"{Data.UpgradeName} is already maxed out!");
            return false;
        }

        if (m_customCheck != null)
        {
            var (isValid, message) = m_customCheck.Invoke();
            if (isValid)
            {
                Debug.LogWarning($"{Data.UpgradeName} cannot be upgraded: {message}");
                return false;
            }
        }
        SoundManager.Play(SoundBank.UpgradeSFX, 0.1f, 0.1f);
        CurrentLevel++;
        UpgradeLevelUp?.Invoke();

        return true;
    }

    public int GetUpgradeCost()
    {
        return IsMaxed ? 0 : Data.GetCost(CurrentLevel);
    }

    public void SetCustomApplicationCheck(Func<(bool isValid, string message)> checkFunction)
    {
        m_customCheck = checkFunction;
    }
}
