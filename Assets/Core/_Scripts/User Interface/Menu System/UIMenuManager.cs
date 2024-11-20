using System;
using UnityEngine;

namespace LBB.BRA1NFvCK
{
    public abstract class UIMenuManager<EMenuScreenType> : MonoBehaviour where EMenuScreenType : Enum
    {
        [SerializeField] protected EMenuScreenType m_initialScreenType;
        [SerializeField] protected UIMenuScreen<EMenuScreenType>[] m_screens; // Array of all screens

        private void Start()
        {
            ChangeScreen(m_initialScreenType); // Change to starter screen
        }

        public void ChangeScreen(EMenuScreenType screen)
        {
            for (int i = 0; i < m_screens.Length; i++)
            {
                if (m_screens[i].ScreenType.Equals(screen))
                {
                    m_screens[i].Show(); // Show target screen
                }
                else
                {
                    m_screens[i].Hide(); // Hide other screens
                }
            }
        }
    }
}
