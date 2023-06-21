using Unity.Burst;
using Unity.Entities;

public partial struct PheromoneLifeCycleSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        new PheromoneLifeCycleJob
        {
            dt = SystemAPI.Time.DeltaTime
        }.ScheduleParallel();
    }

    [BurstCompile]
    public partial struct PheromoneLifeCycleJob : IJobEntity
    {
        public float dt;

        public void Execute(ref PheromoneData pheromoneData)
        {
            pheromoneData.Power -= dt;
            // if (pheromoneData.power <= 0f) Destroy(gameObject);
        }
    }
}