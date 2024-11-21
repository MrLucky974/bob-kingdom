using LuckiusDev.Utils;
using System.Collections.Generic;
using UnityEngine;

public class DecorationScatterer : MonoBehaviour
{
    [SerializeField] private Transform m_parent;
    [SerializeField] private List<GameObject> m_prefabs = new();
    [SerializeField] private float m_radius = 1f;
    [SerializeField] private Vector2 m_regionSize;

    private void Start()
    {
        var size = new Vector2(Mathf.Abs(m_regionSize.x), Mathf.Abs(m_regionSize.y));
        var points = PoissonDiscSampling.GeneratePoints(m_radius, size);
        foreach (var point in points)
        {
            var prefabIndex = Random.Range(0, m_prefabs.Count);
            var prefab = m_prefabs[prefabIndex];
            var instance = Instantiate(prefab, point - m_regionSize * 0.5f, Quaternion.identity);
            instance.transform.SetParent(m_parent != null ? m_parent : transform);
        }
    }
}
