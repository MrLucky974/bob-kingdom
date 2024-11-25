using System;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public float HealthRatio => (float)_currentHealth / _maxHealth;

    public Action HealthChanged;

    private void Start()
    {
        _currentHealth = _maxHealth;
        HealthChanged?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        HealthChanged?.Invoke();
    }

    public void Update()
    {
        if (_currentHealth <= 0)
        {
            Debug.Log("GAME OVER !");
            enabled = false;
        }
    }
}
