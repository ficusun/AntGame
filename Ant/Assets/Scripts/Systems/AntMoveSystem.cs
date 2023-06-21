using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;


public partial struct AntMoveSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        new MoveJob
        {
            dt = SystemAPI.Time.DeltaTime,
        }.ScheduleParallel();
    }

    [BurstCompile]
    public partial struct MoveJob : IJobEntity
    {
        public float dt;

        public void Execute(ref Ant ant, AntMovementAspect antMovement)
        {   
            if (ant.Energy < antMovement.AntMovement.MoveCost) return;

            ant.Energy -= antMovement.AntMovement.MoveCost * dt;
            
            antMovement.Move(ant.TargetPosition, dt);
        }
    }

}