using Unity.Entities;

using UnityEngine;

public struct EntityType : IComponentData
{
    public TargetType Value;
}

public enum TargetType
{   
    None,
    Ant,
    Food,
    Pheromone,
}