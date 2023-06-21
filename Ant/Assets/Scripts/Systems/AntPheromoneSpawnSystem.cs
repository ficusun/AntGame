using Unity.Burst;
using Unity.Entities;
using Unity.Physics.Aspects;
using Unity.Transforms;
using UnityEngine;

public partial struct AntPheromoneSpawnSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                  .CreateCommandBuffer(state.WorldUnmanaged);

        new PheromoneLifeCycleJob
        {
            ecb = ecb.AsParallelWriter(),
            dt = SystemAPI.Time.DeltaTime,

        }.ScheduleParallel();
    }

    [BurstCompile]
    public partial struct PheromoneLifeCycleJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;

        public float dt;

        public void Execute([ChunkIndexInQuery] int chunkIndex, RigidBodyAspect antRigidBody, ref AntPheromoneSpawnerData pheromoneSpawnerData)
        {
            if (Vector3.Distance(antRigidBody.Position, pheromoneSpawnerData.LastPheromonePosition) > pheromoneSpawnerData.Distance)
            {
                pheromoneSpawnerData.LastPheromonePosition = antRigidBody.Position;

                var entity = ecb.Instantiate(chunkIndex, pheromoneSpawnerData.Prefab);
                ecb.SetComponent(chunkIndex, entity, LocalTransform.FromPosition(antRigidBody.Position));
            }
        }
    }
}