using LuckiusDev.Utils;
using UnityEngine;

public class SceneReferences : Singleton<SceneReferences>
{
    [Header("Scene")]
    [SerializeField] private Canvas m_canvas;
    public static Canvas canvas => Instance.m_canvas;

    [SerializeField] private Wall m_wall;
    public static Wall wall => Instance.m_wall;

    [SerializeField] private EnemyWaveSystem m_enemyWaveSystem;
    public static EnemyWaveSystem enemyWaveSystem => Instance.m_enemyWaveSystem;

    [Header("Prefabs")]
    [SerializeField] private DraggableItem m_draggableItemPrefab;
    public static DraggableItem draggableItemPrefab => Instance.m_draggableItemPrefab;

    [SerializeField] private FX m_coinExplosionPrefab;
    public static FX coinExplosionPrefab => Instance.m_coinExplosionPrefab;

    [SerializeField] private DamageIndicator m_damageIndicatorPrefab;
    public static DamageIndicator damageIndicatorPrefab => Instance.m_damageIndicatorPrefab;

    [Header("Upgrades")]
    [SerializeField] private UpgradeData m_vampirismUpgradeData;
    public static UpgradeData vampirismUpgradeData => Instance.m_vampirismUpgradeData;

    [SerializeField] private UpgradeData m_gunpowderUpgradeData;
    public static UpgradeData gunpowderUpgradeData => Instance.m_gunpowderUpgradeData;

    [SerializeField] private UpgradeData m_midasTouchUpgradeData;
    public static UpgradeData midasTouchUpgradeData => Instance.m_midasTouchUpgradeData;

    [SerializeField] private UpgradeData m_godHandUpgradeData;
    public static UpgradeData godHandUpgradeData => Instance.m_godHandUpgradeData;
    
    [Header("VFX")]
    [SerializeField] private GameObject m_jet;
    public static GameObject Jet => Instance.m_jet;

    [SerializeField] private GameObject m_feu;
    public static GameObject Feu => Instance.m_feu;

    [SerializeField] private GameObject m_rail;
    public static GameObject Rail => Instance.m_rail;

    [SerializeField] private GameObject m_arrow;
    public static GameObject Arrow => Instance.m_arrow;

    [SerializeField] private GameObject m_impact;
    public static GameObject Impact => Instance.m_impact;

    [SerializeField] private GameObject m_explosion;
    public static GameObject Explosion => Instance.m_explosion;
}
