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

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

        new FindTargetJob
        {
            physicsWorld = physicsWorld

        }.ScheduleParallel();
    }

    [BurstCompile]
    partial struct FindTargetJob : IJobEntity
    {
        [ReadOnly] public PhysicsWorldSingleton physicsWorld;

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
                var visionArray = visionBuffer.Reinterpret<Entity>();
                
                foreach (var obj in result)
                {
                    visionArray.Add(obj.Entity);
                    obj.Material.CustomTags
                        
                }
                //seeker.ElementAt(i).target = closet.Entity;
            }

        }
    }
}