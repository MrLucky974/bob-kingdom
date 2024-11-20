using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    [Header("Equipment")]
    [SerializeField] private ItemData m_heldItem;

    [Header("Rendering")]
    [SerializeField] private SpriteRenderer m_weaponSpriteRenderer;

    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;
        UpdateWeaponSprite();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (SceneReferences.draggableItemPrefab == null)
        {
            Debug.LogWarning("No prefab specified, make sure to reference in the inspector!");
            return;
        }

        GameObject droppedObject = eventData.pointerDrag;

        if (!droppedObject.TryGetComponent<DraggableItem>(out var item))
            return;

        var heldItem = m_heldItem;
        if (heldItem != null)
        {
            var instance = SceneReferences.draggableItemPrefab.Create(heldItem);
            instance.transform.SetParent(item.ParentAfterDrag);
        }

        Destroy(droppedObject);
        m_heldItem = item.ItemData;
        UpdateWeaponSprite();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_heldItem == null)
            return;

        if (SceneReferences.canvas == null)
        {
            Debug.LogWarning("No canvas specified, make sure to reference in the inspector!", SceneReferences.Instance);
            return;
        }

        if (SceneReferences.draggableItemPrefab == null)
        {
            Debug.LogWarning("No prefab specified, make sure to reference in the inspector!", SceneReferences.Instance);
            return;
        }

        var slot = Player.Instance.Inventory.GetFirstAvailableSlot();
        if (slot != null)
        {
            var instance = SceneReferences.draggableItemPrefab.Create(m_heldItem);
            instance.transform.SetParent(SceneReferences.canvas.transform);
            var gameObject = instance.gameObject;
            gameObject.transform.SetParent(slot.transform);
            gameObject.transform.localPosition = Vector3.zero;

            m_heldItem = null;
            UpdateWeaponSprite();
        }
    }

    private void UpdateWeaponSprite()
    {
        m_weaponSpriteRenderer.sprite = m_heldItem != null ? m_heldItem.Sprite : null;
    }
}
