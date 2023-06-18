using System.Collections.Generic;

[System.Serializable]
public class AntView
{
    private Dictionary<VisibleObject, VisibleObject> objectInView;

    public AntView()
    {
        objectInView = new Dictionary<VisibleObject, VisibleObject>();
    }

    public void AddObjectToView(VisibleObject visibleObject)
    {
        objectInView.Add(visibleObject, visibleObject);
    }

    public void RemoveObjectFromView(VisibleObject visibleObject)
    {
        objectInView.Remove(visibleObject);
    }
}