using LuckiusDev.Utils;
using UnityEngine;

public class SoundManager : PersistentSingleton<SoundManager>
{
    [SerializeField] private AudioSource m_sfxSource;

    private void Start()
    {
        Debug.Assert(m_sfxSource != null, "SFX Audio Source is non existent!", Instance);
    }

    private void Reset()
    {
        m_sfxSource = GetComponentInChildren<AudioSource>();
    }

    public static void Play(AudioClip clip, float randomizedVolumeScale = 0f, float randomizedPitchScale = 0f)
    {
        Debug.Assert(clip != null, "Clip is null!", Instance);

        if (randomizedVolumeScale < 0f)
        {
            Debug.LogWarning("Volume scale randomization cannot be negative, using the absolute value!");
            randomizedVolumeScale = Mathf.Abs(randomizedVolumeScale);
        }

        if (randomizedVolumeScale > 1f)
        {
            Debug.LogWarning("Volume scale randomization cannot be greater than one, clamping the value!");
            randomizedVolumeScale = Mathf.Clamp01(randomizedVolumeScale);
        }

        float volumeScale = 1f + Random.Range(-randomizedVolumeScale, randomizedVolumeScale);
        float pitch = 1f + Random.Range(-randomizedVolumeScale, randomizedVolumeScale);

        var source = Instance.m_sfxSource;
        source.pitch = pitch;
        source.PlayOneShot(clip, volumeScale);
    }

    public static void Play(AudioClip clip)
    {
        Debug.Assert(clip != null, "Clip is null!", Instance);

        var source = Instance.m_sfxSource;
        source.PlayOneShot(clip);
    }
}
