using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemySpawner : MonoBehaviour
{
    private const int COMPOSITION_EVERY_WAVE = 4;

    [Header("References")]
    [SerializeField] private EnemyWaveSystem m_waveSystem;

    [Header("Enemy Spawn Points")]
    [SerializeField] private Transform m_spawnArea1;
    [SerializeField] private Transform m_spawnArea2;

    [Header("Status")]
    [SerializeField] private float m_spawnCoolDown = 1;

    [Header("Enemies")]
    [SerializeField] private int m_initialEnemyCount = 3;
    [SerializeField] private List<EnemyComposition> _enemyCompositions;

    private bool m_spawnActive;
    private int m_currentCompositionIndex;
    private int m_targetEnemyCount;
    private int m_spawnedEnemyCount;
    private int m_enemiesKilledCount;
    
    private EnemyWaveSystem m_wave;

    private void Awake()
    {
        // If no wave system was referenced from the Editor, find the object in the scene
        if (m_waveSystem == null)
            m_waveSystem = FindObjectOfType<EnemyWaveSystem>();

        // Will print an error if no wave system object was found in the scene
        Debug.Assert(m_waveSystem != null, $"Unable to find object of type {nameof(EnemyWaveSystem)}", this);

        m_waveSystem.WaveStatusChanged += OnWaveStatusChanged;
    }

    private void Start()
    {
        m_wave = FindObjectOfType<EnemyWaveSystem>();
        InitializeComposition();
    }

    private void OnDestroy()
    {
        m_waveSystem.WaveStatusChanged -= OnWaveStatusChanged;
    }

    private void OnWaveStatusChanged(EnemyWaveSystem.WaveState state)
    {
        switch (state)
        {
            case EnemyWaveSystem.WaveState.Idle:
                break;
            case EnemyWaveSystem.WaveState.Cooldown:
                m_targetEnemyCount = Mathf.CeilToInt(m_initialEnemyCount * Mathf.Pow(1.15f, m_waveSystem.CurrentWaveIndex));
                break;
            case EnemyWaveSystem.WaveState.InProgress:
                m_spawnActive = true;
                StartCoroutine(nameof(SpawnLoop));
                break;
            case EnemyWaveSystem.WaveState.Finished:
                UpdateCurrentComposition();

                m_spawnedEnemyCount = 0;
                m_enemiesKilledCount = 0;
                break;
        }
    }

    public void MarkEnemyAsKilled()
    {
        m_enemiesKilledCount++;

        if (m_enemiesKilledCount == m_targetEnemyCount)
        {
            m_waveSystem.CompleteWave();
        }
    }

    public void Spawn()
    {
        // On récupère la composition et par conséquent la liste d'ennemis de celle-ci.
        EnemyComposition currentComposition = _enemyCompositions[m_currentCompositionIndex];
        List<GameObject> enemies = currentComposition.enemies;

        // On prend un type d'ennemi aléatoire dans la liste pour l'instancier.
        var randomIndex = Random.Range(0, enemies.Count);
        var enemyPrefab = enemies[randomIndex];

        // On instancie l'ennemi entre deux points défini dans l'éditeur.
        var randomPosition = new Vector3(transform.position.x, Random.Range(m_spawnArea1.transform.position.y, m_spawnArea2.transform.position.y), 0);
        var instance = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        if (instance.TryGetComponent<EnemyBehavior>(out var enemy))
        {
            string instanceName = instance.name.Replace("(Clone)", "");
            instanceName = $"{m_spawnedEnemyCount:000}_" + instanceName;
            instance.name = instanceName;

            enemy.Initialize(this);
        }
        else
        {
            Destroy(instance);
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (m_spawnActive)
        {
            Spawn();
            m_spawnedEnemyCount++;

            if (m_spawnedEnemyCount >= m_targetEnemyCount)
            {
                m_spawnActive = false;
            }
            float newSpawnCoolDown = m_spawnCoolDown - (m_wave.m_currentWaveIndex / 100f);
            yield return new WaitForSeconds(newSpawnCoolDown);
            Debug.Log("spawn cooldown: " + m_spawnCoolDown + " - wave index: " + m_wave.m_currentWaveIndex + " /100");
            Debug.Log("Wave cooldown == " + newSpawnCoolDown);
        }
    }

    /// <summary>
    /// Cette méthod permet de correctement définir l'<see cref="EnemyComposition"/> de départ.
    /// </summary>
    private void InitializeComposition()
    {
        int initialCompositionIndex = 0;  // Valeur temporaire qui va servir à définir la composition de départ dans la liste '_enemyCompositions'.
        var tempWaveIndex = m_waveSystem.CurrentWaveIndex; // Stocke la valeur 'm_waveSystem.CurrentWaveIndex' dans une variable temporaire pour éviter de la modifier et causer des erreurs.

        // Tant que 'tempWaveIndex' supérieur ou égal à 'COMPOSITION_EVERY_WAVE' (= 4), c'est-à-dire qu'on peut faire 'tempWaveIndex' - 'COMPOSITION_EVERY_WAVE'.
        while (tempWaveIndex >= COMPOSITION_EVERY_WAVE)
        {
            tempWaveIndex -= COMPOSITION_EVERY_WAVE; // On retire 'COMPOSITION_EVERY_WAVE' (= 4).
            initialCompositionIndex++; // On incrémente la valeur 'initialCompositionIndex' (on "simule" le passage des vagues).
        }
        SetCurrentEnemyComposition(initialCompositionIndex); // On applique 'initialCompositionIndex' à 'm_currentCompositionIndex'.
    }

    /// <summary>
    /// Cette méthode incrémente l'index de la composition toutes les <see cref="COMPOSITION_EVERY_WAVE"/> (= 4) vagues.
    /// Voir la méthode <seealso cref="SetCurrentEnemyComposition"/>.
    /// </summary>
    private void UpdateCurrentComposition()
    {
        // Si l'index de la vague actuelle est divisible par 'COMPOSITION_EVERY_WAVE' (= 4)
        // et que 'm_waveSystem.CurrentWaveIndex' est supérieur à zéro, on incrémente 'm_currentCompositionIndex'
        // pour avoir la prochaine liste d'ennemis.
        if (m_waveSystem.CurrentWaveIndex % COMPOSITION_EVERY_WAVE == 0 && m_waveSystem.CurrentWaveIndex > 0)
        {
            SetCurrentEnemyComposition(m_currentCompositionIndex + 1);
        }
    }

    /// <summary>
    /// Cette méthode met à jour la valeur <see cref="m_currentCompositionIndex"/> tout en limitant
    /// par un <see cref="Mathf.Clamp(float, float, float)"/> la valeur minimale et maximale,
    /// pour éviter de recevoir des erreurs lors de la lecture de <see cref="_enemyCompositions"/>.
    /// </summary>
    /// <param name="newIndex"></param>
    private void SetCurrentEnemyComposition(int newIndex)
    {
        // Cette fonction permet de ne pas aller en dehors des limites des compositions définies dans l'éditeur par la liste '_enemyCompositions'.
        m_currentCompositionIndex = Mathf.Clamp(newIndex, 0, _enemyCompositions.Count - 1);
    }

    public int CurrentCompositionIndex => m_currentCompositionIndex;
    public int TargetEnemyCount => m_targetEnemyCount;
    public int SpawnedEnemyCount => m_spawnedEnemyCount;
    public int KilledEnemyCount => m_enemiesKilledCount;
}

#if UNITY_EDITOR

[CustomEditor(typeof(EnemySpawner)), CanEditMultipleObjects]
public class EnemySpawnerInspector : AutoRepaintingEditor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);

        EnemySpawner spawner = (EnemySpawner)target;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Composition Index: {spawner.CurrentCompositionIndex}");
        sb.AppendLine($"Target Enemy Count: {spawner.TargetEnemyCount}");
        sb.AppendLine($"Spawned Enemies: {spawner.SpawnedEnemyCount}");
        sb.Append($"Killed Enemies: {spawner.KilledEnemyCount}");

        EditorGUILayout.HelpBox(sb.ToString(), MessageType.None);
    }
}

#endif