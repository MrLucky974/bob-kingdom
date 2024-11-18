using UnityEngine;

[CreateAssetMenu(menuName = "Bob's Kingdom/New Item", fileName = "New Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] private Sprite m_sprite;
    public Sprite Sprite => m_sprite;

    [SerializeField] private int m_attackDamage;
    public int AttackDamage => m_attackDamage;

    [SerializeField] private float m_range;
    public float Range => m_range;

    [SerializeField] private float m_fireRate;
    public float FireRate => m_fireRate;

    [SerializeField] private ItemData m_nextItemTier;
    public ItemData NextItemTier => m_nextItemTier;
}
