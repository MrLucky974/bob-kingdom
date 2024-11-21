using UnityEngine;
using UnityEngine.SceneManagement;

namespace RapidPrototyping.TicTacMix
{
    public static class Bootstrapper
    {
        private const string BootstrapSceneName = "BootstrapScene";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute()
        {
            // Traverse the currently loaded scenes
            for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                var candidate = SceneManager.GetSceneAt(sceneIndex);

                // Early out if already loaded
                if (candidate.name == BootstrapSceneName)
                    return;
            }

            Debug.Log("Loading Bootstrap scene: " + BootstrapSceneName);

            SceneManager.LoadScene(BootstrapSceneName, LoadSceneMode.Additive);
        }
    }
}
