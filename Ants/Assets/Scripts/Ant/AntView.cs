using System.Collections.Generic;

[System.Serializable]
public class AntView
{
    public HashSet<VisibleObject> objectInView;
    // public Dictionary<VisibleObject, VisibleObject> objectInView;

    public AntView()
    {
        objectInView = new HashSet<VisibleObject>();
    }

    public void AddObjectToView(VisibleObject visibleObject)
    {
        objectInView.Add(visibleObject);
    }

    public void RemoveObjectFromView(VisibleObject visibleObject)
    {
        objectInView.Remove(visibleObject);
    }
}