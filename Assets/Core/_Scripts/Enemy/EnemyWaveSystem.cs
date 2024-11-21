using System;
using System.Collections;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyWaveSystem : MonoBehaviour
{
    public enum WaveState
    {
        Idle,
        Cooldown,
        InProgress,
        Finished,
    }

    [Header("References")]
    [SerializeField] private EnemySpawner m_enemySpawner;

    [Header("Status")]
    [SerializeField] private bool m_autoStart = true;
    [SerializeField] private int m_initialWaveIndex = 0;
    [SerializeField] private float m_waveCooldownTime = 3f;
    public float WaveCooldownTime => m_waveCooldownTime;

    private int m_currentWaveIndex = 0;
    public int CurrentWaveIndex => m_currentWaveIndex;

    private WaveState m_waveState;
    public WaveState CurrentWaveState => m_waveState;
    public event Action<WaveState> WaveStatusChanged;

    private float m_remainingCooldownTime;
    public float RemainingCooldownTime => m_remainingCooldownTime;

    private void Awake()
    {
        // If no spawner was referenced from the Editor, find the object in the scene
        if (m_enemySpawner == null)
            m_enemySpawner = FindObjectOfType<EnemySpawner>();

        // Will print an error if no enemy spawner object was found in the scene
        Debug.Assert(m_enemySpawner != null, $"Unable to find object of type {nameof(EnemySpawner)}", this);
    }

    private void Start()
    {
        m_currentWaveIndex = m_initialWaveIndex;

        if (m_autoStart)
        {
            StartCoroutine(nameof(WaveCooldown));
        }
        else
        {
            m_waveState = WaveState.Idle;
            WaveStatusChanged?.Invoke(m_waveState);
        }
    }

    private IEnumerator WaveCooldown()
    {
        m_waveState = WaveState.Cooldown;
        WaveStatusChanged?.Invoke(m_waveState);

        m_remainingCooldownTime = m_waveCooldownTime;
        while (m_remainingCooldownTime > 0f)
        {
            yield return null;
            m_remainingCooldownTime -= Time.deltaTime;
        }

        //yield return new WaitForSeconds(_waveCooldown);

        StartWave();
    }

    public void StartWave()
    {
        m_waveState = WaveState.InProgress;
        WaveStatusChanged?.Invoke(m_waveState);
    }

    public void CompleteWave()
    {
        m_currentWaveIndex++;
        m_waveState = WaveState.Finished;
        WaveStatusChanged?.Invoke(m_waveState);

        StartCoroutine(WaveCooldown());
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(EnemyWaveSystem)), CanEditMultipleObjects]
public class EnemyWaveSystemInspector : Editor
{
    private void OnEnable()
    {
        // Subscribe to the update event
        EditorApplication.update += OnEditorUpdate;
    }

    private void OnDisable()
    {
        // Unsubscribe from the update event
        EditorApplication.update -= OnEditorUpdate;
    }

    private void OnEditorUpdate()
    {
        // Force the inspector to repaint
        Repaint();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnemyWaveSystem waveSystem = (EnemyWaveSystem)target;
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Current Wave: {waveSystem.CurrentWaveIndex}");
        sb.Append($"Wave State: {waveSystem.CurrentWaveState}");
        if (waveSystem.CurrentWaveState == EnemyWaveSystem.WaveState.Cooldown)
        {
            sb.Append($"\nCooldown Time: {waveSystem.RemainingCooldownTime:0.00}s / {waveSystem.WaveCooldownTime:0.00}s");
        }

        EditorGUILayout.HelpBox(sb.ToString(), MessageType.None);
    }
}

#endif