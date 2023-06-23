using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

using UnityEngine;

namespace Systems
{
    public partial struct AntSearchingSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new SearchingJob
            {
                dt = SystemAPI.Time.DeltaTime,

            }.ScheduleParallel();
        }

        [BurstCompile]
        public partial struct SearchingJob : IJobEntity
        {
            public float dt;

            public void Execute(ref Ant ant, AntSearchingAspect searchingAspect)
            {
                ant.TargetPosition = searchingAspect.Wander(dt);
            }
        }
    }
}