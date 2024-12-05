using UnityEngine;

public class OscillatorRotation : Oscillator
{
    [SerializeField] private float m_displacementMultiplierX = 0f;
    [SerializeField] private float m_displacementMultiplierY = 0f;
    [SerializeField] private float m_displacementMultiplierZ = 1f;

    private Quaternion m_baseRotation;

    private void Start()
    {
        m_baseRotation = m_target.transform.localRotation;
    }

    protected override void Update()
    {
        base.Update();

        m_target.transform.localRotation = m_baseRotation * Quaternion.Euler(m_displacement * m_displacementMultiplierX,
            m_displacement * m_displacementMultiplierY, m_displacement * m_displacementMultiplierZ);
    }
}
