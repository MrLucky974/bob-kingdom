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
        public void QuitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}
