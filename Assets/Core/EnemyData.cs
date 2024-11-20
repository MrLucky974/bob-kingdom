using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemies/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public GameObject _sprit;
    public float _health;
    public float _damage;
    public float _attacCooldown;
    public float _speed;
}
