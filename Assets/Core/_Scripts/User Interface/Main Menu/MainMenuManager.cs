using LuckiusDev.Experiments;
using UnityEditor;
using UnityEngine;

namespace LBB.BRA1NFvCK
{
    public enum MainMenuScreenType
    {
        MainMenu,
        Settings,
        Credits
    }

    public class MainMenuManager : UIMenuManager<MainMenuScreenType>
    {
        public void Play()
        {
            SoundManager.Play(SoundBank.MenuButonsSFX, 0.1f, 0.1f);
            SceneTransitionManager.Load("Juan_GameplayScene");
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            SoundManager.Play(SoundBank.MenuButonsSFX, 0.1f, 0.1f);
            EditorApplication.isPlaying = false;
#endif
            SoundManager.Play(SoundBank.MenuButonsSFX, 0.1f, 0.1f);
            Application.Quit();
        }
    }
}
