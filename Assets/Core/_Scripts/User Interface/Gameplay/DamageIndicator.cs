using LuckiusDev.Utils;
using System.Collections;
using TMPro;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    private const float OSCILLATOR_SCALE_VELOCITY = 10f;
    private static readonly Vector3[] DIRECTIONS = { new Vector3(1, 1), new Vector3(-1, -1), new Vector3(-1, 1), new Vector3(1, -1) };

    [SerializeField] private OscillatorScale m_oscillatorScale;
    [SerializeField] private TextMeshPro m_text;
    [SerializeField] private float m_lifetime = 1f;

    private void Start()
    {
        m_oscillatorScale.Play(OSCILLATOR_SCALE_VELOCITY);

        StartCoroutine(nameof(DisplacementCoroutine));
        Destroy(gameObject, m_lifetime);
    }

    private IEnumerator DisplacementCoroutine()
    {
        var originPosition = transform.position;
        var randomDirectionIndex = Random.Range(0, DIRECTIONS.Length);
        var direction = DIRECTIONS[randomDirectionIndex];
        const float distance = 0.5f;
        var targetPosition = originPosition + direction * distance;
        const float arcHeight = 2f;

        float currentTime = 0f;
        while (currentTime < m_lifetime)
        {
            yield return null;
            currentTime += Time.deltaTime;
            float t = currentTime / m_lifetime;

            Vector3 position = ArcPosition(originPosition, targetPosition, arcHeight, t);
            transform.position = position;
        }
    }

    private Vector3 ArcPosition(Vector3 start, Vector3 end, float height, float t)
    {
        // Linear interpolation
        Vector3 linearPos = Vector3.Lerp(start, end, t);

        // Calculate the height offset (parabolic)
        float parabola = 4 * height * t * (1 - t);

        // Add the height offset to the linear position
        return new Vector3(linearPos.x, linearPos.y + parabola, linearPos.z);
    }

    public DamageIndicator Create(int damageValue, Transform origin = null)
    {
        return Create(damageValue, Color.white, origin);
    }

    public DamageIndicator Create(int damageValue, Color color, Transform origin = null)
    {
        var instance = Instantiate(this);

        instance.m_text.color = color;
        instance.m_text.SetText(NumberFormatter.FormatNumberWithSuffix(damageValue));
        if (origin != null)
        {
            instance.transform.position = origin.position;
        }

        return instance;
    }
}
