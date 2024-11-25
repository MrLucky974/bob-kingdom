using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image m_fill;

    public void SetFillAmount(float amount)
    {
        m_fill.fillAmount = amount;
    }
}
