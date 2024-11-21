using LuckiusDev.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LuckiusDev.Experiments
{
    public class SceneTransitionManager : PersistentSingleton<SceneTransitionManager>
    {
        [SerializeField] private Material m_transitionMaterial;
        [SerializeField] private float m_transitionDuration = 0.6f;

        private bool m_isActive = false;

        public static void Load(string sceneName)
        {
            if (Instance.m_isActive)
                return;
            
            Instance.StartCoroutine(Instance.LoadScene(sceneName));
        }

        private IEnumerator LoadScene(string sceneName)
        {
            m_isActive = true;
            m_transitionMaterial.SetFloat("_Amount", 0f); // Set the material property to zero on start

            // Animate the transition material from zero to one
            float currentTime = 0f;
            while (currentTime < m_transitionDuration)
            {
                currentTime += Time.unscaledDeltaTime;
                yield return null;
                m_transitionMaterial.SetFloat("_Amount", currentTime / m_transitionDuration);
            }
            m_transitionMaterial.SetFloat("_Amount", 1f); // set the material property to one

            // Wait for the scene to load
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            while (!operation.isDone)
            {
                yield return null;
            }

            yield return new WaitForEndOfFrame();

            // Animate the transition material from one to zero
            currentTime = 0f;
            while (currentTime < m_transitionDuration)
            {
                currentTime += Time.unscaledDeltaTime;
                yield return null;
                m_transitionMaterial.SetFloat("_Amount", 1f - (currentTime / m_transitionDuration));
            }

            m_transitionMaterial.SetFloat("_Amount", 0f); // Set the material property to zero
            m_isActive = false;
        }
    }
}
