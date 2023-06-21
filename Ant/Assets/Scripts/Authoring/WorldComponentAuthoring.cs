using System;

using Unity.Entities;

using UnityEngine;
using UnityEngine.Serialization;

public class WorldComponentAuthoring : MonoBehaviour
{
    public Bounds Bounds;

    public Transform Ground;

    public Transform[] Walls;
    private void OnValidate()
    {
        if (Ground == null) return;
        Ground.localScale = Bounds.extents / 5;
        
        Walls[0].localScale = new Vector3(Bounds.size.x, 5, 1);
        Walls[1].localScale = new Vector3(Bounds.size.x, 5, 1);
        Walls[2].localScale = new Vector3(1, 5, Bounds.size.z);
        Walls[3].localScale = new Vector3(1, 5, Bounds.size.z);

        Walls[0].position = new Vector3(0, 2.4f, Bounds.extents.z);
        Walls[1].position = new Vector3(0, 2.4f, -Bounds.extents.z);
        Walls[2].position = new Vector3(Bounds.extents.x, 2.4f, 0);
        Walls[3].position = new Vector3(-Bounds.extents.x, 2.4f, 0);
    }

    public class WorldComponentBaker : Baker<WorldComponentAuthoring>
    {
        public override void Bake(WorldComponentAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new WorldComponent
            {
                Bounds = authoring.Bounds
            });
        }
    }
}