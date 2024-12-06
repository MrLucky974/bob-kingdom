using LuckiusDev.Experiments;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameOverUiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_waveScoreText;

    private void Awake()
    {
        SceneTransitionManager.Instance.TransitionEnded += OnTransitionEnded;
    }

    private void OnDestroy()
    {
        SceneTransitionManager.Instance.TransitionEnded -= OnTransitionEnded;
    }

    private void OnTransitionEnded()
    {
        StartCoroutine(nameof(DisplayScoreCoroutine));
    }

    private IEnumerator DisplayScoreCoroutine()
    {
        yield return null;

        int score = ScoreTracker.Instance.Score;
        int currentScore = ScoreTracker.CLEAR_SCORE;
        while (currentScore < score)
        {
            currentScore++;
            yield return new WaitForSecondsRealtime(0.05f);
            m_waveScoreText.SetText(currentScore.ToString("D3"));
            // TODO : Play sound here
        }

        // TODO : Play sound here
    }

    public void MainMenu()
    {
        SoundManager.Play(SoundBank.MenuButonsSFX, 0.1f, 0.1f);
        ScoreTracker.Instance.ClearScore(true);
        SceneTransitionManager.Load("MainMenuScene");
    }

    public void Restart()
    {
        SoundManager.Play(SoundBank.MenuButonsSFX, 0.1f, 0.1f);
        ScoreTracker.Instance.ClearScore(true);
        SceneTransitionManager.Load("Juan_GameplayScene");
    }
}
