using LuckiusDev.Experiments;
using System;
using System.Collections;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int m_initialMaxHealth = 100;
    private int m_maxHealth = 100;
    private int m_currentHealth;

    [Header("Upgrades")]
    [SerializeField] private UpgradeData m_healUpgradeData;
    private Upgrade m_healUpgrade;
    [SerializeField] private UpgradeData m_sturdyWallUpgradeData;
    private Upgrade m_sturdyWallUpgrade;

    public int MaxHealth => m_maxHealth;
    public int CurrentHealth => m_currentHealth;
    public float HealthRatio => (float)m_currentHealth / m_maxHealth;

    public Action HealthChanged;

    private void Start()
    {
        m_healUpgrade = UpgradeManager.Instance.GetUpgrade(m_healUpgradeData);
        m_healUpgrade.SetCustomApplicationCheck(() =>
        {
            return (CurrentHealth >= MaxHealth, "Health is full!");
        });
        m_healUpgrade.UpgradeLevelUp += HandleHealUpgrade;

        m_sturdyWallUpgrade = UpgradeManager.Instance.GetUpgrade(m_sturdyWallUpgradeData);
        m_sturdyWallUpgrade.UpgradeLevelUp += HandleSturdyWallUpgrade;

        m_maxHealth = m_initialMaxHealth;
        m_currentHealth = m_maxHealth;
        HealthChanged?.Invoke();
        m_healUpgrade.Refresh();
    }

    private void OnDestroy()
    {
        m_healUpgrade.UpgradeLevelUp -= HandleHealUpgrade;
        m_sturdyWallUpgrade.UpgradeLevelUp -= HandleSturdyWallUpgrade;
    }

    private void HandleSturdyWallUpgrade()
    {
        int currentLevel = m_sturdyWallUpgrade.CurrentLevel;
        float additionalHealth = m_maxHealth * 0.25f;
        m_maxHealth = m_maxHealth + Mathf.RoundToInt(additionalHealth * currentLevel);
        m_currentHealth += Mathf.RoundToInt((m_maxHealth - m_initialMaxHealth) * 0.5f);
        HealthChanged?.Invoke();
        m_healUpgrade.Refresh();

        Debug.Log($"[{name}] New wall max health: {m_maxHealth}", this);
    }

    private void HandleHealUpgrade()
    {
        Debug.Log($"[{name}] Healing wall!", this);
        const int healAmount = 5;
        Heal(healAmount);
    }

    public void Heal(int amount)
    {
        if (m_currentHealth >= m_maxHealth)
        {
            if (m_currentHealth > m_maxHealth) { m_currentHealth = m_maxHealth; }
            return;
        }

        m_currentHealth += amount;
        HealthChanged?.Invoke();
        m_healUpgrade.Refresh();
    }

    public void TakeDamage(int damage)
    {
        if (m_currentHealth <= 0)
        {
            return;
        }

        m_currentHealth -= damage;
        HealthChanged?.Invoke();
        m_healUpgrade.Refresh();

        if (m_currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER !");
        StartCoroutine(nameof(Defeat));
    }

    private IEnumerator Defeat()
    {
        yield return new WaitForSeconds(3);
        SceneTransitionManager.Load("GameOverScene");
        enabled = false;
    }
}
