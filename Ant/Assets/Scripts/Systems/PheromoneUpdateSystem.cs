using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Stateful;

public partial struct PheromoneUpdateSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //state.CompleteDependency();
        new PheromoneUpdateJob()
        {
            PheromoneLookup = SystemAPI.GetComponentLookup<PheromoneData>(),
        }.Schedule();
    }

    [BurstCompile, WithAll(typeof(Ant))]
    public partial struct PheromoneUpdateJob : IJobEntity
    {
        public ComponentLookup<PheromoneData> PheromoneLookup;
        public void Execute(Entity entity, in DynamicBuffer<StatefulTriggerEvent> triggerEvents)
        {
            //var test = triggerEvents.AsNativeArray();
            foreach (var evnt in triggerEvents)
            {
                if (evnt.State == StatefulEventState.Exit)
                {
                    PheromoneLookup.GetRefRW(evnt.GetOtherEntity(entity)).ValueRW.Power = 500;
                    //envt.GetOtherEntity(entity);
                }
            }
        }
    }
}