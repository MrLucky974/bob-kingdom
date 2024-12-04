using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WaveCountdownUI : MonoBehaviour
{
    private const string WAVE_TIMER_TEXT = "Wave {0} in {1} seconds...";

    [Header("References")]
    [SerializeField] private Image m_progressFillImage;
    [SerializeField] private TextMeshProUGUI m_waveTimerText;
    [SerializeField] private CanvasGroup m_canvasGroup;

    [Header("Settings")]
    [SerializeField] private float m_skipHoldDuration = 0.4f;

    private float m_progress;
    private bool m_isHolding = false;

    private void Start()
    {
        SceneReferences.enemyWaveSystem.WaveStatusChanged += HandleWaveStatusChanged;
    }

    private void OnDestroy()
    {
        SceneReferences.enemyWaveSystem.WaveStatusChanged -= HandleWaveStatusChanged;
    }

    private void Update()
    {
        int currentWaveIndex = SceneReferences.enemyWaveSystem.CurrentWaveIndex;
        float remainingCooldownTime = SceneReferences.enemyWaveSystem.RemainingCooldownTime;
        string cooldown = remainingCooldownTime > 1f ? remainingCooldownTime.ToString("F0") : remainingCooldownTime.ToString("F2");
        m_waveTimerText.SetText(string.Format(WAVE_TIMER_TEXT, currentWaveIndex + 1, cooldown));
    }

    private void LateUpdate()
    {
        if (m_isHolding)
        {
            m_progress += Time.unscaledDeltaTime;
            m_progressFillImage.fillAmount = m_progress / m_skipHoldDuration;
            if (m_progress >= m_skipHoldDuration)
            {
                m_progress = 0f;
                m_progressFillImage.fillAmount = 0f;
                m_isHolding = false;

                SceneReferences.enemyWaveSystem.SkipWave();
            }
        }
        else
        {
            m_progress = 0f;
            m_progressFillImage.fillAmount = 0f;
        }
    }

    private void HandleWaveStatusChanged(EnemyWaveSystem.WaveState state)
    {
        if (state == EnemyWaveSystem.WaveState.InProgress)
        {
            m_canvasGroup.alpha = 0f;
            m_canvasGroup.interactable = false;
            m_canvasGroup.blocksRaycasts = false;
        }
        else if (state == EnemyWaveSystem.WaveState.Cooldown)
        {
            m_canvasGroup.alpha = 1f;
            m_canvasGroup.interactable = true;
            m_canvasGroup.blocksRaycasts = true;
        }
    }

    public void OnPointerDown(BaseEventData data)
    {
        m_isHolding = true;
    }

    public void OnPointerUp(BaseEventData data)
    {
        m_isHolding = false;
    }
}
