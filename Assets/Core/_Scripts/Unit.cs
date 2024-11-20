using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    [SerializeField] private DraggableItem m_draggableItemPrefab;
    [SerializeField] private Canvas m_canvas;

    [SerializeField] private ItemData m_heldItem;
    [SerializeField] private SpriteRenderer m_weaponSpriteRenderer;
    [SerializeField] private Inventory m_inventory;

    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;
        UpdateWeaponSprite();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (!droppedObject.TryGetComponent<DraggableItem>(out var item))
            return;

        var heldItem = m_heldItem;
        if (heldItem != null)
        {
            var instance = m_draggableItemPrefab.Create(heldItem);
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

        var slot = m_inventory.GetFirstAvailableSlot();
        if (slot != null)
        {
            var instance = m_draggableItemPrefab.Create(m_heldItem);
            instance.transform.SetParent(m_canvas.transform);
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
