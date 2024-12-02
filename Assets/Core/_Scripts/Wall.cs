using LuckiusDev.Experiments;
using System;
using System.Collections;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _maxHealth;
    private int _currentHealth;

    [Header("Upgrades")]
    [SerializeField] private UpgradeData m_healUpgradeData;
    private Upgrade m_healUpgrade;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public float HealthRatio => (float)_currentHealth / _maxHealth;

    public Action HealthChanged;

    private void Start()
    {
        m_healUpgrade = UpgradeManager.Instance.GetUpgrade(m_healUpgradeData);
        m_healUpgrade.SetCustomApplicationCheck(() =>
        {
            return (CurrentHealth >= MaxHealth, "Health is full!");
        });
        m_healUpgrade.UpgradeLevelUp += HandleHealUpgrade;

        _currentHealth = _maxHealth;
        HealthChanged?.Invoke();
        m_healUpgrade.Refresh();
    }

    private void OnDestroy()
    {
        m_healUpgrade.UpgradeLevelUp -= HandleHealUpgrade;
    }

    private void HandleHealUpgrade()
    {
        Debug.Log($"[{name}] Healing wall!", this);
        const int healAmount = 5;
        _currentHealth += healAmount;
        HealthChanged?.Invoke();
        m_healUpgrade.Refresh();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        HealthChanged?.Invoke();
        m_healUpgrade.Refresh();
    }

    public void Update()
    {
        if (_currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER !");
        StartCoroutine(Defeat());
    }
    IEnumerator Defeat()
    {
        yield return new WaitForSeconds(3);
        SceneTransitionManager.Load("GameOver");
        enabled = false;
    }
}
