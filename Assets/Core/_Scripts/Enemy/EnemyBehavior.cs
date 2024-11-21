using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private const int DEFAULT_MAX_HEALTH = 10;

    [Header("Stats")]
    [SerializeField] private EnemyData m_enemyData;

    [Space]

    [SerializeField] private float m_damage;
    [SerializeField] private float m_attackCooldown;
    [SerializeField] private float m_movementSpeed;
    private float m_health;

    private bool m_wallContact;
    private Wall m_wall;
    private bool m_canAttack;

    private EnemySpawner m_spawner;

    private void Start()
    {
        m_spawner = FindObjectOfType<EnemySpawner>();
        m_canAttack = true;
        m_wallContact = false;

        if (m_enemyData != null)
        {
            LoadEnemyData();
        }
        else
        {
            m_health = DEFAULT_MAX_HEALTH;
        }
    }


    private void Update()
    {
        if (m_health <= 0)
        {
            m_spawner._enemyKilled++;
            Destroy(gameObject);
        }

        transform.Translate(new Vector3(-m_movementSpeed * Time.deltaTime, 0, 0));

        if (m_wallContact && m_canAttack)
        {
            m_wall.TakeDamage(m_damage);
            m_canAttack = false;
            StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(m_attackCooldown);
        m_canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Wall"))
        {
            m_movementSpeed = 0;
            m_wallContact = true;
            m_wall = collision.GetComponent<Wall>();
        }
    }

    private void LoadEnemyData()
    {
        m_health = m_enemyData.maxHealth;
        m_damage = m_enemyData.damage;
        m_attackCooldown = m_enemyData.attackCooldown;
        m_movementSpeed = m_enemyData.speed;
    }
}
