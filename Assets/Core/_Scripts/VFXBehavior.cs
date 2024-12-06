using System.Collections;
using UnityEngine;

public class VFXBehavior : MonoBehaviour
{
    [SerializeField] private float m_lifeTime;
    
    [SerializeField] private bool m_explo;
    [SerializeField] private float m_range;

    [SerializeField] private bool m_impact;
    [SerializeField] private float m_varianteIntensity;
    private bool m_Ionce;

    [SerializeField] private bool m_shootFire;
    [SerializeField] private float m_FscaleVariante;
    [SerializeField] private float m_FposeVariante;
    private bool m_Fonce;

    [SerializeField] private bool m_shootRail;
    [SerializeField] private float m_RscaleVariante;
    [SerializeField] private float m_RposeVariante;
    private bool m_Ronce;

    [SerializeField] private bool m_shootJet;
    [SerializeField] private float m_JscaleVariante;
    [SerializeField] private float m_JposeVariante;
    private bool m_Jonce;

    [SerializeField] private bool m_shootArrow;
    [SerializeField] private float m_AscaleVariante;
    [SerializeField] private float m_AposeVariante;
    private bool m_Aonce;
    void Start()
    {
        StartCoroutine(SelfDestruct());    
        m_Ionce = false;
    }

    private void Update()
    {
        if(m_explo == true)
        {
            transform.localScale = transform.localScale + new Vector3(m_range * Time.deltaTime, m_range * Time.deltaTime, m_range * Time.deltaTime) ;
        }
        else if(m_impact == true && m_Ionce == false)
        {
            var randomRota = Random.Range(0,m_varianteIntensity);
            transform.rotation = new Quaternion(Quaternion.identity.x, Quaternion.identity.y, randomRota , Quaternion.identity.w) ;
            m_Ionce = true;
        }
        else if (m_shootFire == true && m_Fonce == false)
        {
            var randoScale = Random.Range(m_FscaleVariante, 1);
            GetComponentInChildren<SpriteRenderer>().gameObject.transform.localScale = new Vector3(randoScale,randoScale,randoScale);

            var randoPoseX = Random.Range(0, m_FposeVariante);
            var randoPoseY = Random.Range(0, m_FposeVariante);
            GetComponentInChildren<SpriteRenderer>().gameObject.transform.localPosition = new Vector3(randoPoseX, randoPoseY, transform.position.z);

            GetComponentInChildren<SpriteRenderer>().flipY = Random.value < 0.5f;
            m_Fonce = true;
        }
        else if (m_shootRail == true && m_Ronce == false)
        {
            var randoScale = Random.Range(m_RscaleVariante, 1);
            GetComponentInChildren<SpriteRenderer>().gameObject.transform.localScale = new Vector3(randoScale, randoScale, randoScale);

            var randoPoseX = Random.Range(0, m_RposeVariante);
            var randoPoseY = Random.Range(0, m_RposeVariante);
            GetComponentInChildren<SpriteRenderer>().gameObject.transform.localPosition = new Vector3(randoPoseX, randoPoseY, transform.position.z);

            GetComponentInChildren<SpriteRenderer>().flipY = Random.value < 0.5f;
            m_Ronce = true;
        }
        else if (m_shootJet == true && m_Jonce == false)
        {
            var randoScale = Random.Range(m_JscaleVariante, 1);
            GetComponentInChildren<SpriteRenderer>().gameObject.transform.localScale = new Vector3(randoScale, randoScale, randoScale);

            var randoPoseX = Random.Range(0, m_JposeVariante);
            var randoPoseY = Random.Range(0, m_JposeVariante);
            GetComponentInChildren<SpriteRenderer>().gameObject.transform.localPosition = new Vector3(randoPoseX, randoPoseY, transform.position.z);
            
            GetComponentInChildren<SpriteRenderer>().flipY = Random.value < 0.5f;
            m_Jonce = true;
        }
        else if (m_shootArrow == true && m_Aonce == false)
        {
            var randoScale = Random.Range(m_AscaleVariante, 1);
            GetComponentInChildren<SpriteRenderer>().gameObject.transform.localScale = new Vector3(randoScale, randoScale, randoScale);

            var randoPoseX = Random.Range(0, m_AposeVariante);
            var randoPoseY = Random.Range(0, m_AposeVariante);
            GetComponentInChildren<SpriteRenderer>().gameObject.transform.localPosition = new Vector3(randoPoseX, randoPoseY, transform.position.z);
            m_Aonce = true;
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(m_lifeTime);
        Destroy(gameObject);
    }
}
