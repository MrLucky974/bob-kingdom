using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    [Header("Equipment")]
    [SerializeField] private ItemData m_heldItem;

    [Header("Rendering")]
    [SerializeField] private SpriteRenderer m_weaponSpriteRenderer;
    [SerializeField] private SpriteRenderer m_outfitSpriteRenderer;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField] private Transform m_target;
#endif

    private EventSystem eventSystem;

    private float m_nextAttackTime;

    private void Start()
    {
        eventSystem = EventSystem.current;

        UpdateWeaponSprite();
        UpdateOutfitSprite();
    }

    private void Update()
    {
        if (m_heldItem == null)
            return;

        if (m_target == null)
            return;

        if (Time.time > m_nextAttackTime)
        {
            Debug.Log($"[{name}]: Shoot", this);

            var projectile = Instantiate(m_heldItem.ProjectilePrefab, transform.position, Quaternion.identity);
            var data = new Projectile.ProjectileData()
            {
                target = m_target,
                maxSpeed = 10f,

                trajectoryMaxHeight = m_heldItem.TrajectoryMaxHeight,
                trajectoryAnimationCurve = m_heldItem.TrajectoryAnimationCurve,
                axisCorrectionAnimationCurve = m_heldItem.AxisCorrectionAnimationCurve,
                speedAnimationCurve = m_heldItem.SpeedAnimationCurve,
            };
            projectile.Initialize(data);
            m_nextAttackTime = Time.time + (1f / m_heldItem.FireRate);
        }
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
        UpdateOutfitSprite();
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
            UpdateOutfitSprite();
        }
    }

    private void UpdateWeaponSprite()
    {
        m_weaponSpriteRenderer.sprite = m_heldItem != null ? m_heldItem.Sprite : null;
    }

    private void UpdateOutfitSprite()
    {
        m_outfitSpriteRenderer.sprite = m_heldItem != null ? m_heldItem.Outfit : null;
    }
}
