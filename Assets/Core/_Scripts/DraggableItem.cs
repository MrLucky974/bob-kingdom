using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image m_image;

    private Transform m_parentAfterDrag;

    public void SetParentAfterDrag(Transform parentAfterDrag)
    {
        m_parentAfterDrag = parentAfterDrag;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_parentAfterDrag = transform.parent;

        var root = transform.root;
        var parent = root.GetComponentInChildren<Canvas>().transform;
        transform.SetParent(parent);
        transform.SetAsLastSibling();

        m_image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(m_parentAfterDrag);
        m_image.raycastTarget = true;
    }
}
