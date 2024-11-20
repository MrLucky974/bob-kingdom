using System;
using UnityEngine;
using UnityEngine.Events;

namespace LBB.BRA1NFvCK
{
    public abstract class UIMenuScreen<EMenuScreenType> : MonoBehaviour where EMenuScreenType : Enum
    {
        [SerializeField] private EMenuScreenType m_screenType; // Type of this screen
        [SerializeField] private CanvasGroup m_customCanvasGroup; // Custom CanvasGroup if any
        [SerializeField] private UnityEvent onScreenLoad; // Event triggered when screen is loaded

        private CanvasGroup m_canvasGroup; // CanvasGroup of this screen

        public EMenuScreenType ScreenType { get => m_screenType; } // Getter for screen type

        private void Awake()
        {
            m_canvasGroup = m_customCanvasGroup == null ? GetComponent<CanvasGroup>() : m_customCanvasGroup; // Assign CanvasGroup
            m_canvasGroup.alpha = 0f; // Set initial alpha to 0
            m_canvasGroup.enabled = false; // Disable CanvasGroup initially
            GetComponent<RectTransform>().localScale = Vector3.zero; // Set initial scale to zero
        }

        public void Show()
        {
            m_canvasGroup = m_customCanvasGroup == null ? GetComponent<CanvasGroup>() : m_customCanvasGroup; // Assign CanvasGroup
            m_canvasGroup.enabled = true; // Enable CanvasGroup for showing
            GetComponent<RectTransform>().localScale = Vector3.one; // Set scale to one
            m_canvasGroup.alpha = 1f; // Set final alpha
            onScreenLoad?.Invoke();
        }

        public void Hide()
        {
            m_canvasGroup = m_customCanvasGroup == null ? GetComponent<CanvasGroup>() : m_customCanvasGroup; // Assign CanvasGroup
            m_canvasGroup.enabled = false; // Enable CanvasGroup for showing
            GetComponent<RectTransform>().localScale = Vector3.zero; // Set scale to one
            m_canvasGroup.alpha = 0f; // Set final alpha
        }
    }
}
