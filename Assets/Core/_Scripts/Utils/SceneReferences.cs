using LuckiusDev.Utils;
using UnityEngine;

public class SceneReferences : Singleton<SceneReferences>
{
    [Header("Scene")]
    [SerializeField] private Canvas m_canvas;
    public static Canvas canvas => Instance.m_canvas;

    [SerializeField] private Wall m_wall;
    public static Wall wall => Instance.m_wall;

    [Header("Prefabs")]
    [SerializeField] private DraggableItem m_draggableItemPrefab;
    public static DraggableItem draggableItemPrefab => Instance.m_draggableItemPrefab;

    [SerializeField] private FX m_coinExplosionPrefab;
    public static FX coinExplosionPrefab => Instance.m_coinExplosionPrefab;

    [Header("Upgrades")]
    [SerializeField] private UpgradeData m_vampirismUpgradeData;
    public static UpgradeData vampirismUpgradeData => Instance.m_vampirismUpgradeData;

    [SerializeField] private UpgradeData m_gunpowderUpgradeData;
    public static UpgradeData gunpowderUpgradeData => Instance.m_gunpowderUpgradeData;

    [SerializeField] private UpgradeData m_midasTouchUpgradeData;
    public static UpgradeData midasTouchUpgradeData => Instance.m_midasTouchUpgradeData;
}
