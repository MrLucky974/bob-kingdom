using System;
using UnityEngine;

/// <summary>
/// Monitors a ParticleSystem and destroys the GameObject once the system has finished playing.
/// </summary>
public class FX : MonoBehaviour
{
    [SerializeField, Tooltip("Reference to the ParticleSystem component.")]
    private ParticleSystem m_particleSystem;

    [SerializeField]
    private TimedAudioEvent[] m_timedAudioEvents;
    private bool[] m_audioPlayedFlags;

    private void Reset()
    {
        // Automatically assign the ParticleSystem if available on the same GameObject.
        m_particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        // Initialize flags to ensure audio events are played only once.
        m_audioPlayedFlags = new bool[m_timedAudioEvents.Length];
    }

    private void Update()
    {
        // Check if the ParticleSystem has finished playing.
        if (m_particleSystem != null && !m_particleSystem.IsAlive())
        {
            Destroy(gameObject); // Destroy the GameObject when the ParticleSystem is done.
        }

        // Get the playback progress of the particle system.
        float playbackProgress = GetPlaybackProgress();

        // Trigger audio events based on progress.
        for (int i = 0; i < m_timedAudioEvents.Length; i++)
        {
            if (!m_audioPlayedFlags[i] && playbackProgress >= m_timedAudioEvents[i].triggerTime)
            {
                SoundManager.Play(m_timedAudioEvents[i].clip);
                m_audioPlayedFlags[i] = true; // Mark this audio event as played.
            }
        }
    }

    /// <summary>
    /// Gets the normalized playback progress of the particle system.
    /// </summary>
    /// <returns>A value between 0 and 1 representing the playback progress.</returns>
    public float GetPlaybackProgress()
    {
        if (m_particleSystem == null)
        {
            Debug.LogWarning("ParticleSystem reference is missing!");
            return 0f;
        }

        // Access the ParticleSystem's Main module
        var main = m_particleSystem.main;

        // Ensure the particle system has a non-zero duration
        if (main.duration <= 0)
        {
            Debug.LogWarning("ParticleSystem has invalid duration!");
            return 0f;
        }

        // Calculate normalized progress (time / duration)
        return Mathf.Clamp01(m_particleSystem.time / main.duration);
    }

    [Serializable]
    public struct TimedAudioEvent
    {
        public AudioClip clip;
        [Range(0f, 1f)] public float triggerTime;
    }
}

