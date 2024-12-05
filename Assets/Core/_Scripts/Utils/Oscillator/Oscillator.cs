using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] protected GameObject m_target;
    [SerializeField] private bool m_useUnscaledDeltaTime = false;

    [Space]

    [SerializeField] protected float m_spring;
    [SerializeField] protected float m_damp;
    protected float m_displacement;
    protected float m_velocity;

    protected virtual void Update()
    {
        float deltaTime = m_useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;

        float force = -m_spring * m_displacement - m_damp * m_velocity;
        m_velocity += force * deltaTime;
        m_displacement += m_velocity * deltaTime;
    }

    private void Reset()
    {
        m_target = gameObject;
    }

    public void Play(float velocity)
    {
        m_velocity = velocity;
    }
}
