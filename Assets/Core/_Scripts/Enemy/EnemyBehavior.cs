using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private const int DEFAULT_MAX_HEALTH = 10;

    [Header("Stats")]
    [SerializeField] private EnemyData m_enemyData;

    [Space]

    [SerializeField] private int m_damage;
    [SerializeField] private float m_attackCooldown;
    [SerializeField] private float m_movementSpeed;
    private int m_health;
    [SerializeField] private ulong m_gold;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer m_spriteRenderer;
    [SerializeField] private float m_flashDuration = 0.3f;
    [SerializeField] private OscillatorRotation m_hitRotationOscillator;

    private bool m_wallContact;
    private Wall m_wall;
    private bool m_canAttack;

    private EnemySpawner m_spawner;
    private EnemyWaveSystem m_wave;

    public void Initialize(EnemySpawner spawner)
    {
        m_spawner = spawner;
    }

    private void Start()
    {
        m_wave = FindObjectOfType<EnemyWaveSystem>();
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
        m_health = Mathf.FloorToInt(m_health + (m_wave.m_currentWaveIndex / 2));
        m_gold = (ulong)(m_health);
    }

    private void Update()
    {
        if (m_health <= 0)
        {
            Death();
            return;
        }

        transform.Translate(new Vector3(-m_movementSpeed * Time.deltaTime, 0, 0), Space.World);

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            OnClick(Input.GetTouch(0).position);
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            OnClick(Input.mousePosition);
        }

        if (m_wallContact && m_canAttack)
        {
            if (m_wall.HasSpikes)
            {
                const int baseDamage = 1;
                const float damageMultiplier = 1.5f;
                int selfDamage = Mathf.RoundToInt(baseDamage * Mathf.Pow(damageMultiplier, m_wall.WallSpikesLevel - 1));
                Damage(selfDamage);
            }

            m_wall.TakeDamage(m_damage);
            SoundManager.Play(SoundBank.WallHitSFX, 0.1f, 0.1f);
            m_canAttack = false;
            StartCoroutine(nameof(AttackCooldown));
        }
    }

    private void OnClick(Vector3 screenClickPosition)
    {
        // Convert screen position to world position.
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenClickPosition);

        // Perform a 2D raycast at the click/tap position.
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero, Mathf.Infinity);

        if (hit.transform == transform)
        {
            var upgrade = UpgradeManager.Instance.GetUpgrade(SceneReferences.godHandUpgradeData);
            int[] damageValues = { 0, 1, 3, 5 };

            if (upgrade.CurrentLevel > 0)
            {
                int damageIndex = upgrade != null ? upgrade.CurrentLevel : 0;
                Damage(damageValues[damageIndex]);
            }
        }
    }

    public void Damage(int amount)
    {
        m_health -= amount;
        m_hitRotationOscillator.Play(1000f);
        SceneReferences.damageIndicatorPrefab.Create(amount, Color.red, transform);
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
        SoundManager.Play(SoundBank.MobDeathSFX, 0.1f, 0.1f);
        SoundManager.Play(SoundBank.CoinSFX, 0.1f, 0.1f);
        Instantiate(SceneReferences.coinExplosionPrefab, transform.position, Quaternion.identity);
        GiveCoins();
        ApplyVampirism();
        m_spawner.MarkEnemyAsKilled();

        Destroy(gameObject);
    }

    private void GiveCoins()
    {
        var midasTouchUpgrade = UpgradeManager.Instance.GetUpgrade(SceneReferences.midasTouchUpgradeData);
        float[] values = { 0f, 0.25f, 0.5f };

        ulong money = (ulong)Mathf.RoundToInt(m_gold * (1f + values[midasTouchUpgrade.CurrentLevel]));
        Player.Instance.GiveMoney(money);
    }

    /// <summary>
    /// Attempts to apply the "Vampirism" effect, which provides a chance to heal a wall 
    /// based on the current level of the Vampirism upgrade.
    /// </summary>
    /// <remarks>
    /// - The Vampirism effect uses a predefined success chance array corresponding to 
    ///   upgrade levels.
    /// - If the Vampirism effect is successful, it heals the wall for a random amount 
    ///   between a minimum and maximum value.
    /// - The method validates the upgrade level before attempting to apply the effect.
    /// </remarks>
    private void ApplyVampirism()
    {
        // Retrieve the Vampirism upgrade data from the Upgrade Manager
        var vampirismUpgrade = UpgradeManager.Instance.GetUpgrade(SceneReferences.vampirismUpgradeData);

        // Ensure the Vampirism upgrade data exists
        if (vampirismUpgrade != null)
        {
            // Get the current level of the Vampirism upgrade
            int currentLevel = vampirismUpgrade.CurrentLevel;

            if (currentLevel < 1)
            {
                return;
            }

            // Define the chance of success for each level (index corresponds to level - 1)
            float[] chance = new float[3] { 5f, 10f, 15f };

            // Validate the current level of the Vampirism upgrade
            if (currentLevel > chance.Length)
            {
                Debug.LogError("Invalid vampirism upgrade level.");
                return;
            }

            // Retrieve the success chance based on the current level
            float value = chance[currentLevel - 1];

            // Roll a random value between 0 and 100 to determine success
            float roll = Random.Range(0f, 100f);
            bool isSuccessful = roll < value;

            // If the Vampirism effect succeeds, heal the wall
            if (isSuccessful)
            {
                // Define the range of healing values
                const int minHealAmount = 2;
                const int maxHealAmount = 4;

                // Calculate a random heal value within the range
                int healValue = Random.Range(minHealAmount, maxHealAmount + 1);

                // Apply healing to the wall
                SceneReferences.wall.Heal(healValue);

                // Log the healing action for debugging purposes
                Debug.Log($"[{name}] Healing {healValue} HP to wall.", this);
            }
        }
        else
        {
            Debug.LogError("Vampirism upgrade data is null or not found.");
        }
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
        m_gold = m_enemyData.GoldOnDeath;
    }
}
