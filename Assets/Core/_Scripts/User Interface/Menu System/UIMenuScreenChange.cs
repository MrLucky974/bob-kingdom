using System;
using UnityEngine;

namespace LBB.BRA1NFvCK
{
    public class UIMenuScreenChange<EMenuScreenType> : MonoBehaviour where EMenuScreenType : Enum
    {
        [SerializeField] private UIMenuManager<EMenuScreenType> m_menuManager;
        [SerializeField] private EMenuScreenType m_menuScreenType;

        public void ChangeScreen()
        {
            m_menuManager.ChangeScreen(m_menuScreenType); // Change to the specified screen
        }
    }
}
