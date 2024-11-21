using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Bob's Kingdom/New Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float maxHealth;
    public float damage;
    public float attackCooldown;
    public float speed;
}
