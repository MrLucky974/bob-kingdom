using UnityEngine;

[CreateAssetMenu(menuName = "Bob's Kingdom/New Item", fileName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Visuals")]
    [SerializeField] private Sprite m_sprite;
    public Sprite Sprite => m_sprite;

    [SerializeField] private Sprite m_outfit;
    public Sprite Outfit => m_outfit;

    [Header("Projectile")]
    [SerializeField] private Projectile m_projectilePrefab;
    public Projectile ProjectilePrefab => m_projectilePrefab;

    [SerializeField] private float m_trajectoryMaxHeight = 1f;
    public float TrajectoryMaxHeight => m_trajectoryMaxHeight;

    [SerializeField] private AnimationCurve m_trajectoryAnimationCurve;
    public AnimationCurve TrajectoryAnimationCurve => m_trajectoryAnimationCurve;

    [SerializeField] private AnimationCurve m_axisCorrectionAnimationCurve;
    public AnimationCurve AxisCorrectionAnimationCurve => m_axisCorrectionAnimationCurve;

    [SerializeField] private AnimationCurve m_speedAnimationCurve;
    public AnimationCurve SpeedAnimationCurve => m_speedAnimationCurve;

    [Header("Stats")]
    [SerializeField] private int m_attackDamage;
    public int AttackDamage => m_attackDamage;

    [SerializeField] private float m_range;
    public float Range => m_range;

    [SerializeField] private float m_fireRate;
    public float FireRate => m_fireRate;

    [SerializeField] private ItemData m_nextItemTier;
    public ItemData NextItemTier => m_nextItemTier;
}
