using UnityEngine;

/// <summary>
/// Monitors a ParticleSystem and destroys the GameObject once the system has finished playing.
/// </summary>
public class FX : MonoBehaviour
{
    [SerializeField, Tooltip("Reference to the ParticleSystem component.")]
    private ParticleSystem m_particleSystem;

    private void Reset()
    {
        // Automatically assign the ParticleSystem if available on the same GameObject.
        m_particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        // Check if the ParticleSystem has finished playing.
        if (m_particleSystem != null && !m_particleSystem.IsAlive())
        {
            Destroy(gameObject); // Destroy the GameObject when the ParticleSystem is done.
        }
    }
}

