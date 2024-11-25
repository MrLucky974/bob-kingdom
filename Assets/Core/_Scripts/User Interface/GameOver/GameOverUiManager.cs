using LuckiusDev.Experiments;
using UnityEngine;

public class GameOverUiManager : MonoBehaviour
{
    public void MainMenu()
    {
        SceneTransitionManager.Load("MainMenuScene");
    }

    public void Restart()
    {
        SceneTransitionManager.Load("Juan_GameplayScene");
    }
}
