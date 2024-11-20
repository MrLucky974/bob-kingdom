using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeHandler : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup m_group;

    [SerializeField] private Slider m_volumeSlider;
    [SerializeField] private TextMeshProUGUI m_volumeLabel;

    private AudioMixer m_mixer;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (m_group == null)
            return;

        m_mixer = m_group.audioMixer;

        if (m_volumeSlider == null)
            return;

        m_mixer.GetFloat(m_group.name, out float volume);
        m_volumeSlider.onValueChanged.AddListener(SetVolume);
        m_volumeSlider.value = JUtils.DbToNormalized(volume);
    }

    public void SetVolume(float value)
    {
        m_mixer.SetFloat(m_group.name, JUtils.NormalizedToDb(value));
        UpdateLabel(value);
    }

    private void UpdateLabel(float value)
    {
        m_volumeLabel.SetText((value * 100).ToString("0.0") + "%");
    }
}
