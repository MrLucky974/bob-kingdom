using UnityEngine;

public class WallUIHandler : MonoBehaviour
{
    [SerializeField] private Wall m_wall;
    [SerializeField] private HealthBar m_healthBar;

    private void Awake()
    {
        if (m_wall == null)
        {
            m_wall = FindObjectOfType<Wall>();
        }

        Debug.Assert(m_wall != null, "No wall component was found on the scene!", this);

        if (m_healthBar == null)
        {
            m_healthBar = GetComponentInChildren<HealthBar>();
        }

        Debug.Assert(m_healthBar != null, "No health bar was found on the scene canvas!", this);

        m_wall.HealthChanged += HandleWallHealthChanged;
    }

    private void OnDestroy()
    {
        m_wall.HealthChanged -= HandleWallHealthChanged;
    }

    private void HandleWallHealthChanged()
    {
        var ratio = m_wall.HealthRatio;
        m_healthBar.SetFillAmount(ratio);
    }
}
