using System.Collections;
using UnityEngine;

public class VFXBehavior : MonoBehaviour
{
    [SerializeField] private float m_lifeTime;
    void Start()
    {
        StartCoroutine(SelfDestruct());       
    }
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(m_lifeTime);
        Destroy(gameObject);
    }
}
