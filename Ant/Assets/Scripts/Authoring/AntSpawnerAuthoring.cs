using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class AntSpawnerAuthoring : MonoBehaviour
{
    public float AntMaxCount;

    public GameObject Prefab;

    public float3 StartPos;

    public int AntPerTime;

    public class AntSpawnerBaker : Baker<AntSpawnerAuthoring>
    {
        public override void Bake(AntSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new AntSpawner
            {
                AntMaxCount = authoring.AntMaxCount,
                Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                StartPosition = authoring.StartPos,
                AntPerTime = authoring.AntPerTime,
            });
        }
    }
}