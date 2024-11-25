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
    private int m_health;
    [SerializeField] private int m_Gold;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private float m_flashDuration = 0.3f;

    private bool m_wallContact;
    private Wall m_wall;
    private bool m_canAttack;

    private EnemySpawner m_spawner;

    public void Initialize(EnemySpawner spawner)
    {
        m_spawner = spawner;
    }

    private void Start()
    {
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

    private void OnDestroy()
    {
        m_spawner.MarkEnemyAsKilled();
    }

    private void Update()
    {
        if (m_health <= 0)
        {
            Death();
        }

        transform.Translate(new Vector3(-m_movementSpeed * Time.deltaTime, 0, 0));

        if (m_wallContact && m_canAttack)
        {
            m_wall.TakeDamage(m_damage);
            m_canAttack = false;
            StartCoroutine(nameof(AttackCooldown));
        }
    }

    public void Damage(int amount)
    {
        m_health -= amount;
        Flash();
    }

    private Coroutine m_flashCoroutine;

    private void Flash()
    {
        if (m_flashCoroutine != null)
        {
            StopCoroutine(m_flashCoroutine);
        }
        m_flashCoroutine = StartCoroutine(nameof(FlashCoroutine));
    }

    private IEnumerator FlashCoroutine()
    {
        m_spriteRenderer.material.SetFloat("_Flashing", 1f);
        yield return new WaitForSeconds(m_flashDuration);
        m_spriteRenderer.material.SetFloat("_Flashing", 0f);
        m_flashCoroutine = null;
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(m_attackCooldown);
        m_canAttack = true;
    }

    private void Death()
    {
        //TODO : rajouter un VFX avec le sprite de coin et un SFX !
        Player.Instance.GiveMoney(m_Gold);
        Destroy(gameObject);
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
        m_Gold = m_enemyData.GoldOnDeath;
    }
}
