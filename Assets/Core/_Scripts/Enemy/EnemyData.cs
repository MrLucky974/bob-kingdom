using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Bob's Kingdom/New Enemy Data")]
public class EnemyData : ScriptableObject
{
    public int maxHealth;
    public int damage;
    public float attackCooldown;
    public float speed;
    public ulong GoldOnDeath;
}
