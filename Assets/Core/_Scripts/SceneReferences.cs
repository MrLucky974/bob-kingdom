using LuckiusDev.Utils;
using UnityEngine;

public class SceneReferences : Singleton<SceneReferences>
{
    [Header("Scene")]
    [SerializeField] private Canvas m_canvas;
    public static Canvas canvas => Instance.m_canvas;

    [Header("Prefabs")]
    [SerializeField] private DraggableItem m_draggableItemPrefab;
    public static DraggableItem draggableItemPrefab => Instance.m_draggableItemPrefab;
}
