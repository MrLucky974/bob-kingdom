using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public EnemyData _enemyData;
    
    [SerializeField] GameObject _sprit;
    public float _health;
    [SerializeField] float _damage;
    [SerializeField] float _attacCooldown;
    [SerializeField] float _speed;
    
    [SerializeField] bool _wallContact;
    [SerializeField] Wall _wall;
    [SerializeField] bool _canAttac;

    EnemySpawner _spawner;


    private void Start()
    {
        _spawner = FindObjectOfType<EnemySpawner>();
        _canAttac = true;
        _wallContact = false;
        if (_enemyData != null)
        {
            LoadEnemyData();
        }
    }


    private void Update()
    {
        if (_enemyData != null)
        {
            if (_health <= 0)
            {
                _spawner._enemyKilled++;
                Destroy(gameObject);
            }
        }
        transform.Translate(new Vector3(-_speed * Time.deltaTime/2, 0,0));
        if(_wallContact == true)
        {
            if(_canAttac == true)
            {
                _wall.TakeDamage(_damage);
                _canAttac = false;
                StartCoroutine(AttacCooldown());
            }
        }
    }

    IEnumerator AttacCooldown()
    {
        yield return new WaitForSeconds(_attacCooldown);
        _canAttac = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.tag == "Wall")
            {
                _speed = 0;
                _wallContact = true;
                _wall = collision.GetComponent<Wall>();
            }
        }
    }

    private void LoadEnemyData()
    {
        _sprit = _enemyData._sprit;
        _health = _enemyData._health;
        _damage = _enemyData._damage;
        _attacCooldown = _enemyData._attacCooldown;
        _speed = _enemyData._speed;

        Instantiate(_sprit, transform);
        gameObject.AddComponent<CapsuleCollider2D>();
        GetComponent<CapsuleCollider2D>().size = new Vector2(1, 1.5f);
        GetComponent<CapsuleCollider2D>().isTrigger = true;
    }
}
