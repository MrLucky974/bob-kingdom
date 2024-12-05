using UnityEngine;

public class OscillatorScale : Oscillator
{
    [SerializeField] private float m_displacementMultiplierX = 1f;
    [SerializeField] private float m_displacementMultiplierY = -1f;
    [SerializeField] private float m_displacementMultiplierZ = 0f;

    private Vector3 m_baseScale;

    private void Start()
    {
        m_baseScale = m_target.transform.localScale;
    }

    protected override void Update()
    {
        base.Update();

        Vector3 targetScale = m_baseScale + new Vector3(m_displacement * m_displacementMultiplierX,
            m_displacement * m_displacementMultiplierY,
            m_displacement * m_displacementMultiplierZ
        );

        targetScale.x = Mathf.Clamp(targetScale.x, 0, 1000);
        targetScale.y = Mathf.Clamp(targetScale.y, 0, 1000);
        targetScale.z = Mathf.Clamp(targetScale.z, 0, 1000);

        m_target.transform.localScale = targetScale;
    }
}
