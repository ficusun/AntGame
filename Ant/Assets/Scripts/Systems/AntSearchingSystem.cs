using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

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

            public void Execute(ref Ant ant, AntSearchingAspect searchingAspect, in DynamicBuffer<VisionBuffer> visionBuffer)
            {
                if (visionBuffer.IsEmpty)
                {
                    ant.TargetPosition = searchingAspect.Wander(dt);
                }
                else
                {
                    var float power = 0;
                    foreach (var obj in visionBuffer)
                    {
                        if obj.typeOf == 0
                        {   
                            // call obj.Entity to have Power
                            if obj.Entity.power > power
                            {
                                power = obj.Entity.power;
                                ant.TargetPosition = obj.pos
                            }
                        }
                    }
                }
                
            }
        }
    }
}