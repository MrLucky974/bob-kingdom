using System.Collections;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] float _maxHealth;
    [SerializeField] float _currentHealth;
    [SerializeField] float _invincibleTime;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }
    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
    }
    public void Update()
    {
        if(_currentHealth <= 0)
        {
            Debug.Log("GAME OVER !");
        }
    }
}
