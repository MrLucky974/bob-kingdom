using LuckiusDev.Experiments;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    [Header("Pause Menu")]
    [SerializeField] private CanvasGroup m_pauseMenu;
    [SerializeField] private CanvasGroup m_settingsMenu;

    private void Start()
    {
        SoundManager.Play(SoundBank.MenuButonsSFX, 0.1f, 0.1f);
        m_pauseMenu.alpha = 0f;
        m_pauseMenu.interactable = false;
        m_pauseMenu.blocksRaycasts = false;
        m_pauseMenu.GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void OpenSettingsMenu()
    {
        SoundManager.Play(SoundBank.MenuButonsSFX, 0.1f, 0.1f);
        m_settingsMenu.alpha = 1f;
        m_settingsMenu.interactable = true;
        m_settingsMenu.blocksRaycasts = true;
        m_settingsMenu.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public void CloseSettingsMenu()
    {
        SoundManager.Play(SoundBank.MenuButonsSFX, 0.1f, 0.1f);
        m_settingsMenu.alpha = 0f;
        m_settingsMenu.interactable = false;
        m_settingsMenu.blocksRaycasts = false;
        m_settingsMenu.GetComponent<RectTransform>().localScale = Vector3.zero;
    }

    public void OpenPauseMenu()
    {
        SoundManager.Play(SoundBank.MenuButonsSFX, 0.1f, 0.1f);
        m_pauseMenu.alpha = 1f;
        m_pauseMenu.interactable = true;
        m_pauseMenu.blocksRaycasts = true;
        m_pauseMenu.GetComponent<RectTransform>().localScale = Vector3.one;

        Time.timeScale = 0f;
    }

    public void ClosePauseMenu()
    {
        SoundManager.Play(SoundBank.MenuButonsSFX, 0.1f, 0.1f);
        m_pauseMenu.alpha = 0f;
        m_pauseMenu.interactable = false;
        m_pauseMenu.blocksRaycasts = false;
        m_pauseMenu.GetComponent<RectTransform>().localScale = Vector3.zero;

        Time.timeScale = 1f;
    }

    public void Quit()
    {
        ClosePauseMenu();
        Time.timeScale = 1f;

        SceneTransitionManager.Load("MainMenuScene");
    }
}
