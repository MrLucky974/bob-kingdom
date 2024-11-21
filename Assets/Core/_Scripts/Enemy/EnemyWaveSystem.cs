using System;
using System.Collections;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Syst�me de gestion des vagues d'ennemis dans un jeu.
/// Ce script contr�le l'�tat des vagues (en attente, en progression, termin�es, etc.),
/// d�clenche les vagues et applique un d�lai entre celles-ci.
/// </summary>
public class EnemyWaveSystem : MonoBehaviour
{
    /// <summary>
    /// �tats possibles d'une vague.
    /// </summary>
    public enum WaveState
    {
        Idle,        // En attente, aucune vague active.
        Cooldown,    // D�lai avant le d�but de la prochaine vague.
        InProgress,  // Une vague est en cours.
        Finished,    // La vague actuelle est termin�e.
    }

    [Header("References")]
    [SerializeField] private EnemySpawner m_enemySpawner; // Spawner responsable de g�n�rer les ennemis dans le jeu.

    [Header("Status")]
    [SerializeField] private bool m_autoStart = true; // Si activ�, les vagues d�marrent automatiquement d�s le d�but du jeu.

    [SerializeField] private int m_initialWaveIndex = 0; // Indice de la premi�re vague � ex�cuter.

    [SerializeField] private float m_waveCooldownTime = 3f; // Temps d'attente (en secondes) entre deux vagues.
    public float WaveCooldownTime => m_waveCooldownTime; // Acc�s en lecture au temps de pause entre les vagues.

    private int m_currentWaveIndex = 0; // Indice de la vague actuelle.
    public int CurrentWaveIndex => m_currentWaveIndex; // Acc�s en lecture � l'indice de la vague actuelle.

    private WaveState m_waveState; // �tat actuel de la vague.
    public WaveState CurrentWaveState => m_waveState; // Acc�s en lecture � l'�tat actuel des vagues.

    public event Action<WaveState> WaveStatusChanged; // �v�nement d�clench� � chaque changement d'�tat des vagues.

    private float m_remainingCooldownTime; // Temps restant avant le d�but de la prochaine vague.
    public float RemainingCooldownTime => m_remainingCooldownTime; // Acc�s en lecture au temps restant avant la prochaine vague.

    private void Awake()
    {
        // Si aucun spawner n'est assign� depuis l'�diteur, on essaie d'en trouver un dans la sc�ne.
        if (m_enemySpawner == null)
            m_enemySpawner = FindObjectOfType<EnemySpawner>();

        // Affiche un message d'erreur si aucun spawner n'a �t� trouv�.
        Debug.Assert(m_enemySpawner != null, $"Impossible de trouver un objet de type {nameof(EnemySpawner)}", this);
    }

    private void Start()
    {
        // Initialise l'indice de la vague actuelle.
        m_currentWaveIndex = m_initialWaveIndex;

        // Si le d�marrage automatique est activ�, on commence le processus de pause entre vagues.
        if (m_autoStart)
        {
            StartCoroutine(nameof(WaveCooldown));
        }
        else
        {
            // Sinon, on met le syst�me en attente.
            m_waveState = WaveState.Idle;
            WaveStatusChanged?.Invoke(m_waveState);
        }
    }

    /// <summary>
    /// Coroutine pour g�rer le d�lai entre deux vagues.
    /// </summary>
    private IEnumerator WaveCooldown()
    {
        // Change l'�tat en "Cooldown" et informe les abonn�s via l'�v�nement.
        m_waveState = WaveState.Cooldown;
        WaveStatusChanged?.Invoke(m_waveState);

        // Initialise le temps restant.
        m_remainingCooldownTime = m_waveCooldownTime;

        // Boucle jusqu'� ce que le d�lai soit �coul�.
        while (m_remainingCooldownTime > 0f)
        {
            yield return null; // Attend une frame.
            m_remainingCooldownTime -= Time.deltaTime; // Diminue le temps restant.
        }

        // Une fois le d�lai �coul�, d�marre la prochaine vague.
        StartWave();
    }

    /// <summary>
    /// D�marre une nouvelle vague.
    /// </summary>
    private void StartWave()
    {
        // Change l'�tat en "InProgress" et informe les abonn�s via l'�v�nement.
        m_waveState = WaveState.InProgress;
        WaveStatusChanged?.Invoke(m_waveState);
    }

    /// <summary>
    /// Termine la vague actuelle et passe � la suivante apr�s un d�lai.
    /// </summary>
    public void CompleteWave()
    {
        // Passe � la vague suivante.
        m_currentWaveIndex++;

        // Change l'�tat en "Finished" et informe les abonn�s via l'�v�nement.
        m_waveState = WaveState.Finished;
        WaveStatusChanged?.Invoke(m_waveState);

        // Lance le processus de pause entre les vagues.
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