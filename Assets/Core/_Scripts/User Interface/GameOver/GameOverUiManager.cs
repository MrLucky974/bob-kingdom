using LuckiusDev.Experiments;
using UnityEngine;

public class GameOverUiManager : MonoBehaviour
{
    public void MainMenu()
    {
        SoundManager.Play(SoundBank.MenuButonsSFX, 0.1f, 0.1f);
        SceneTransitionManager.Load("MainMenuScene");
    }

    public void Restart()
    {
        SoundManager.Play(SoundBank.MenuButonsSFX, 0.1f, 0.1f);
        SceneTransitionManager.Load("Juan_GameplayScene");
    }
}
