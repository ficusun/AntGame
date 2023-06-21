using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;

using Random = Unity.Mathematics.Random;

public partial struct AntSpawnSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<AntSpawner>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        
        var antSpawner = SystemAPI.GetSingleton<AntSpawner>();
        var count = SystemAPI.QueryBuilder().WithAll<AntStats>().Build().CalculateEntityCount();
        
        if (count >= antSpawner.AntMaxCount) return;
        
        var entities = state.EntityManager.Instantiate(antSpawner.Prefab, antSpawner.AntPerTime, Allocator.Temp);
        
        foreach (var entity in entities)
        {
            SystemAPI.GetComponentRW<AntSearching>(entity).ValueRW.Random = Random.CreateFromIndex((uint)entity.Index); // new Random((uint)SystemAPI.Time.ElapsedTime + 1);
            state.EntityManager.SetComponentData(entity, LocalTransform.FromPosition(antSpawner.StartPosition));
        }
    }
}

