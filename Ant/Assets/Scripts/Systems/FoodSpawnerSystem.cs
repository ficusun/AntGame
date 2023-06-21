using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct FoodSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<WorldComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var count = SystemAPI.QueryBuilder().WithAll<FoodData>().Build().CalculateEntityCount();
        //.CreateCommandBuffer(state.WorldUnmanaged);
        // var dt = SystemAPI.Time.DeltaTime;

        new SpawnFoodJob
        {
            ecb = ecb.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            elapsedTime = SystemAPI.Time.ElapsedTime,
            count = count,
            worldComponent = SystemAPI.GetSingleton<WorldComponent>(),
        }.ScheduleParallel();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    public partial struct SpawnFoodJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;

        public double elapsedTime;

        public int count;

        public WorldComponent worldComponent;

        public void Execute([ChunkIndexInQuery] int chunkIndex, ref FoodSpawner foodSpawner)
        {
            if (count >= foodSpawner.MaxCount) return;
            if (foodSpawner.LastSpawnTime + foodSpawner.SpawnInterval < elapsedTime)
            {
                foodSpawner.LastSpawnTime = elapsedTime;
                var entity = ecb.Instantiate(chunkIndex, foodSpawner.Prefab);
                var pos = foodSpawner.Random.NextFloat3(worldComponent.Bounds.max, worldComponent.Bounds.min);
                pos.y = foodSpawner.FoodYPos;
                ecb.SetComponent(chunkIndex, entity, LocalTransform.FromPosition(pos));
                ecb.SetComponent(chunkIndex, entity,
                    new FoodData
                    {
                        Energy = foodSpawner.Random.NextFloat(foodSpawner.FoodMinEnergy, foodSpawner.FoodMaxEnergy)
                    });
            }
        }
    }
}