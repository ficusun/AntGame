using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Physics.Aspects;
using Unity.Transforms;

using UnityEngine;

namespace Systems
{
    public partial struct AntPheromoneSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.TempJob);
            new PheromoneLifeCycleJob
            {
                ecb = ecb,
                dt = SystemAPI.Time.DeltaTime,
            }.ScheduleParallel();

            new PheromoneLifeCycleJob
            {
                dt = SystemAPI.Time.DeltaTime
            }.ScheduleParallel();
        }

        [BurstCompile]
        public partial struct PheromoneLifeCycleJob : IJobEntity
        {
            
            public EntityCommandBuffer ecb;
            
            public float dt;

            public void Execute(RigidBodyAspect antRigidBody, ref AntPheromoneSpawnerData pheromoneSpawnerData)
            {
                if (Vector3.Distance(antRigidBody.Position, pheromoneSpawnerData.LastPheromonePosition) > pheromoneSpawnerData.Distance)
                {
                    var entity = ecb.Instantiate(pheromoneSpawnerData.Prefab);
                    ecb.SetComponent(entity, LocalTransform.FromPosition(antRigidBody.Position));
                }
            }
        }
        
        [BurstCompile]
        public partial struct PheromoneLifeCycleJob : IJobEntity
        {
            public float dt;

            public void Execute(ref PheromoneData pheromoneData)
            {
                pheromoneData.Power -= dt;
            }
        }
    }
}