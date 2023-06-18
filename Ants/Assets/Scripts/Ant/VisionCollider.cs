using UnityEngine;

public class VisionCollider : MonoBehaviour
{
    private AntView antView;

    public void Init(AntView antView)
    {
        this.antView = antView;
    }

    private void OnTriggerEnter2D(Collider2D worldObject)
    {
        if (worldObject.TryGetComponent<VisibleObject>(out var visibleObject))
        {
            antView.AddObjectToView(visibleObject);
        }
    }

    private void OnTriggerExit2D(Collider2D worldObject)
    {
        if (worldObject.TryGetComponent<VisibleObject>(out var visibleObject))
        {
            antView.RemoveObjectFromView(visibleObject);
        }
    }
}