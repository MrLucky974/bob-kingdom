using System;
using System.Collections;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Système de gestion des vagues d'ennemis dans un jeu.
/// Ce script contrôle l'état des vagues (en attente, en progression, terminées, etc.),
/// déclenche les vagues et applique un délai entre celles-ci.
/// </summary>
public class EnemyWaveSystem : MonoBehaviour
{
    /// <summary>
    /// États possibles d'une vague.
    /// </summary>
    public enum WaveState
    {
        Idle,        // En attente, aucune vague active.
        Cooldown,    // Délai avant le début de la prochaine vague.
        InProgress,  // Une vague est en cours.
        Finished,    // La vague actuelle est terminée.
    }

    [Header("References")]
    [SerializeField] private EnemySpawner m_enemySpawner; // Spawner responsable de générer les ennemis dans le jeu.

    [Header("Status")]
    [SerializeField] private bool m_autoStart = true; // Si activé, les vagues démarrent automatiquement dès le début du jeu.

    [SerializeField] private int m_initialWaveIndex = 0; // Indice de la première vague à exécuter.

    [SerializeField] private float m_waveCooldownTime = 3f; // Temps d'attente (en secondes) entre deux vagues.
    public float WaveCooldownTime => m_waveCooldownTime; // Accès en lecture au temps de pause entre les vagues.

    private int m_currentWaveIndex = 0; // Indice de la vague actuelle.
    public int CurrentWaveIndex => m_currentWaveIndex; // Accès en lecture à l'indice de la vague actuelle.

    private WaveState m_waveState; // État actuel de la vague.
    public WaveState CurrentWaveState => m_waveState; // Accès en lecture à l'état actuel des vagues.

    public event Action<WaveState> WaveStatusChanged; // Événement déclenché à chaque changement d'état des vagues.

    private float m_remainingCooldownTime; // Temps restant avant le début de la prochaine vague.
    public float RemainingCooldownTime => m_remainingCooldownTime; // Accès en lecture au temps restant avant la prochaine vague.

    private void Awake()
    {
        // Si aucun spawner n'est assigné depuis l'éditeur, on essaie d'en trouver un dans la scène.
        if (m_enemySpawner == null)
            m_enemySpawner = FindObjectOfType<EnemySpawner>();

        // Affiche un message d'erreur si aucun spawner n'a été trouvé.
        Debug.Assert(m_enemySpawner != null, $"Impossible de trouver un objet de type {nameof(EnemySpawner)}", this);
    }

    private void Start()
    {
        // Initialise l'indice de la vague actuelle.
        m_currentWaveIndex = m_initialWaveIndex;

        // Si le démarrage automatique est activé, on commence le processus de pause entre vagues.
        if (m_autoStart)
        {
            StartCoroutine(nameof(WaveCooldown));
        }
        else
        {
            // Sinon, on met le système en attente.
            m_waveState = WaveState.Idle;
            WaveStatusChanged?.Invoke(m_waveState);
        }
    }

    /// <summary>
    /// Coroutine pour gérer le délai entre deux vagues.
    /// </summary>
    private IEnumerator WaveCooldown()
    {
        // Change l'état en "Cooldown" et informe les abonnés via l'événement.
        m_waveState = WaveState.Cooldown;
        WaveStatusChanged?.Invoke(m_waveState);

        // Initialise le temps restant.
        m_remainingCooldownTime = m_waveCooldownTime;

        // Boucle jusqu'à ce que le délai soit écoulé.
        while (m_remainingCooldownTime > 0f)
        {
            yield return null; // Attend une frame.
            m_remainingCooldownTime -= Time.deltaTime; // Diminue le temps restant.
        }

        // Une fois le délai écoulé, démarre la prochaine vague.
        StartWave();
    }

    /// <summary>
    /// Démarre une nouvelle vague.
    /// </summary>
    private void StartWave()
    {
        // Change l'état en "InProgress" et informe les abonnés via l'événement.
        m_waveState = WaveState.InProgress;
        WaveStatusChanged?.Invoke(m_waveState);
    }

    /// <summary>
    /// Termine la vague actuelle et passe à la suivante après un délai.
    /// </summary>
    public void CompleteWave()
    {
        // Passe à la vague suivante.
        m_currentWaveIndex++;

        // Change l'état en "Finished" et informe les abonnés via l'événement.
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