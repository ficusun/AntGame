using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine.EventSystems;

public partial struct VisionSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldSingleton>();

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        //var lookUp = SystemAPI.GetComponentLookup<EntityType>();
        
        new FindTargetJob
        {
            physicsWorld = physicsWorld,
            EntityTypeLookUp = SystemAPI.GetComponentLookup<EntityType>(),
            

        }.ScheduleParallel();
    }

    [BurstCompile]
    partial struct FindTargetJob : IJobEntity
    {
        [ReadOnly] public PhysicsWorldSingleton physicsWorld;

        [ReadOnly] public ComponentLookup<EntityType> EntityTypeLookUp;

        public void Execute(in LocalTransform transform, in Vision vision, ref DynamicBuffer<VisionBuffer> visionBuffer)
        {
            visionBuffer.Clear();

            var input = new PointDistanceInput
            {
                Filter = vision.Layers,
                MaxDistance = vision.Radius,
                Position = transform.Position + math.forward(transform.Rotation) * vision.Offset,
            };

            var result = new NativeList<DistanceHit>(Allocator.Temp);
            if (physicsWorld.CalculateDistance(input, ref result))
            {
                foreach (var hit in result)
                {
                    if (EntityTypeLookUp.HasComponent(hit.Entity))
                    {
                        visionBuffer.Add(new VisionBuffer
                        {
                            Entity = hit.Entity,
                            TargetPosition = hit.Position,
                            targetType = EntityTypeLookUp[hit.Entity].Value,
                        });
                    }
                }
            }

        }
    }
}