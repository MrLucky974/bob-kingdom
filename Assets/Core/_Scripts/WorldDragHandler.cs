using UnityEngine;

public class WorldDragHandler : MonoBehaviour
{
    public static GameObject draggedItem;

    private void Update()
    {
        if (draggedItem == null)
            return;

        var position = Input.mousePosition;
        UpdatePosition(position);
    }

    private void UpdatePosition(Vector3 position)
    {
        draggedItem.transform.position = position;
    }
}
