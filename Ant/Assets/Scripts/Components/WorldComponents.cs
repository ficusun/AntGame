using Unity.Entities;

using UnityEngine;

public struct WorldComponent : IComponentData
{
    public Bounds Bounds;
}