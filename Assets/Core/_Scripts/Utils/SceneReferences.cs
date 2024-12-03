using LuckiusDev.Utils;
using UnityEngine;

public class SceneReferences : Singleton<SceneReferences>
{
    [Header("Scene")]
    [SerializeField] private Canvas m_canvas;
    public static Canvas canvas => Instance.m_canvas;

    [SerializeField] private Wall m_wall;
    public static Wall Wall => Instance.m_wall;

    [Header("Prefabs")]
    [SerializeField] private DraggableItem m_draggableItemPrefab;
    public static DraggableItem draggableItemPrefab => Instance.m_draggableItemPrefab;

    [Header("Upgrades")]
    [SerializeField] private UpgradeData m_vampirismUpgradeData;
    public static UpgradeData VampirismUpgradeData => Instance.m_vampirismUpgradeData;
}
