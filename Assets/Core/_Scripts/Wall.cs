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
        if (_currentHealth <= 0)
        {
            return;
        }

        _currentHealth -= damage;
        HealthChanged?.Invoke();
        m_healUpgrade.Refresh();

        if (_currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        Debug.Log("GAME OVER !");
        SoundManager.Play(SoundBank.WallColapsingSFX, 0.1f, 0.1f);
        StartCoroutine(nameof(Defeat));
    }

    private IEnumerator Defeat()
    {
        yield return new WaitForSeconds(3);
        SceneTransitionManager.Load("GameOverScene");
        enabled = false;
    }
}
