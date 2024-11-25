#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
public class AutoRepaintingEditor : Editor
{
    private void OnEnable()
    {
        // Subscribe to the update event
        EditorApplication.update += OnEditorUpdate;
    }

    private void OnDisable()
    {
        // Unsubscribe from the update event
        EditorApplication.update -= OnEditorUpdate;
    }

    private void OnEditorUpdate()
    {
        // Force the inspector to repaint
        Repaint();
    }
}
#endif