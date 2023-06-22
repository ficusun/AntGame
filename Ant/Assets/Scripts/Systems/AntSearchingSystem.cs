using Unity.Burst;
using Unity.Collections;
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
                pheromonLookup = SystemAPI.GetComponentLookup<PheromoneData>(true),
                dt = SystemAPI.Time.DeltaTime,

            }.ScheduleParallel();
        }

        [BurstCompile]
        public partial struct SearchingJob : IJobEntity
        {
            [ReadOnly]
            public ComponentLookup<PheromoneData> pheromonLookup;

            public float dt;

            public void Execute(ref Ant ant, AntSearchingAspect searchingAspect, in DynamicBuffer<VisionBuffer> visionBuffer)
            {
                if (visionBuffer.IsEmpty)
                {
                    ant.TargetPosition = searchingAspect.Wander(dt);
                }
                else
                {
                    var pheromonePower = 0f;

                    foreach (var targetData in visionBuffer)
                    {
                        switch (targetData.targetType)
                        {
                            case TargetType.Ant: break;
                            case TargetType.Food: break;
                            case TargetType.Pheromone:

                                FindPheromone(targetData, ref ant, ref pheromonePower);
                                break;
                        }
                    }
                }

            }

            private void FindPheromone(in VisionBuffer targetData, ref Ant ant, ref float maxPowerSoFar)
            {
                var pheromonePower = pheromonLookup[targetData.Entity].Power;

                if (pheromonePower > maxPowerSoFar)
                {
                    maxPowerSoFar = pheromonePower;
                    ant.TargetPosition = targetData.TargetPosition;
                    ant.targetEntity = targetData.Entity;
                }
            }
        }
    }
}