using Unity.Burst;
using Unity.Entities;

    public partial struct FoodLifeCycleSystem : ISystem
    {
        // [BurstCompile]
        // public void OnCreate(ref SystemState state)
        // {   
        //     state.RequireForUpdate<FoodSpawner>();
        // }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {   
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
           
            new FoodLifeCycleJob
            {
                ecb = ecb.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                dt = SystemAPI.Time.DeltaTime,
            }.ScheduleParallel();
        }

        [BurstCompile]
        public partial struct FoodLifeCycleJob : IJobEntity
        {
            public EntityCommandBuffer.ParallelWriter ecb;

            public float dt;

            public void Execute([ChunkIndexInQuery] int chunkIndex, Entity currentEntity, ref FoodData foodData)
            {
                foodData.Energy -= dt;
                if (foodData.Energy <= 0f)
                {
                    ecb.DestroyEntity(chunkIndex, currentEntity);
                }
            }
        }
    }
