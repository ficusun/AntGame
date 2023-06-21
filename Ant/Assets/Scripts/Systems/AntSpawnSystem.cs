using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;

public partial struct AntSpawnSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        
        var antSpawner = SystemAPI.GetSingleton<AntSpawner>();
        var count = SystemAPI.QueryBuilder().WithAll<AntStats>().Build().CalculateEntityCount();
        
        if (count >= antSpawner.AntMaxCount) return;
        
        for (int i = 0; i < antSpawner.AntPerTime; i++)
        {
            var entity = state.EntityManager.Instantiate(antSpawner.Prefab);
            state.EntityManager.SetComponentData(entity, LocalTransform.FromPosition(antSpawner.StartPosition));
        }
    }
}

