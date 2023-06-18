using UnityEngine;

public abstract class VisibleObject : MonoBehaviour
{
    public ObjectType objectType;
}

public enum ObjectType
{
    Food,
    Pheromone,
}