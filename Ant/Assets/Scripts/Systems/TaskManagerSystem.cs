using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Physics.Stateful;
using Unity.VisualScripting;

public partial struct TaskManagerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        new TaskJob
        {
            Lookup = SystemAPI.GetEntityStorageInfoLookup(),
            pheromonLookup = SystemAPI.GetComponentLookup<PheromoneData>(true),

        }.ScheduleParallel();

    }


    [BurstCompile, WithOptions(EntityQueryOptions.IgnoreComponentEnabledState)]
    public partial struct TaskJob : IJobEntity
    {

        [ReadOnly] public EntityStorageInfoLookup Lookup;
        [ReadOnly] public ComponentLookup<PheromoneData> pheromonLookup;

        public void Execute(ref Ant ant, AntSearchingAspect searchingAspect, in DynamicBuffer<VisionBuffer> visionBuffer, EnabledRefRW<AntPheromoneSpawnerData> antPheromoneSpawner)
        {
            antPheromoneSpawner.ValueRW = ant.TargetType == TargetType.None;

            if (visionBuffer.IsEmpty)
            {
                searchingAspect.IsActive.ValueRW = true;
                ant.TargetType = TargetType.None;
            }
            else
            {
                var rate = 0f;
                foreach (var targetData in visionBuffer)
                {
                    if (!Lookup.Exists(targetData.Entity))
                    {
                        continue;
                    }
                    switch (targetData.targetType)
                    {
                        case TargetType.Ant: break;
                        case TargetType.Food:
                            ant.TargetPosition = targetData.TargetPosition;
                            ant.targetEntity = targetData.Entity;
                            ant.TargetType = targetData.targetType;
                            
                            searchingAspect.IsActive.ValueRW = false;
                            return;
                        case TargetType.Pheromone:
                            FindPheromone(targetData, ref ant, ref rate, searchingAspect.Position, searchingAspect.AntSearching.Random.NextFloat(1f, 10f));
                            searchingAspect.IsActive.ValueRW = false;
                            break;
                    }
                }
            }
        }

        private void FindPheromone(in VisionBuffer targetData, ref Ant ant, ref float maxPowerSoFar, in float3 antPos, float randomOffset) // in float3 antPos, float randomOffset
        {
            if (ant.targetEntity == targetData.Entity)
            {
                return;
            }
            //var pheromonePower = pheromonLookup[targetData.Entity].Power;
            var rate = pheromonLookup[targetData.Entity].Power / math.distance(targetData.TargetPosition, antPos) + randomOffset;

            if (rate > maxPowerSoFar)
            {
                maxPowerSoFar = rate;
                ant.TargetPosition = targetData.TargetPosition;
                ant.targetEntity = targetData.Entity;
                ant.TargetType = targetData.targetType;
            }
        }

    }
}