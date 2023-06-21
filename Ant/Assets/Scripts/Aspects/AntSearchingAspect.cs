using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Aspects;
using Unity.Transforms;
using UnityEngine;

using Random = UnityEngine.Random;

public readonly partial struct AntSearchingAspect : IAspect
{   
    readonly RefRW<AntSearching> antSearching;
    // readonly RefRW<LocalTransform> antTransform;
    readonly RigidBodyAspect antRigidBody;
    // readonly RefRW<Ant> ant;
    public ref AntSearching AntSearching => ref antSearching.ValueRW;
    
    public float3 Wander(float dt)
    {
        AntSearching.angleChangeTimer += dt;

        if (AntSearching.angleChangeTimer >= AntSearching.AngleChangeRate)
        {
            // var rd = new Unity.Mathematics.Random(3);
            AntSearching.theta += AntSearching.Random.NextFloat(-AntSearching.AngleChange, AntSearching.AngleChange);
            AntSearching.angleChangeTimer = 0;
        }

        var wanderPoint = MathUtility.SetMagnitude(antRigidBody.LinearVelocity, 1);
        wanderPoint += antRigidBody.Position;
        wanderPoint = CalculateWanderPoint(wanderPoint);

        return wanderPoint;
    }

    public float3 CalculateWanderPoint(float3 center)
    {
        var wanderPoint = center;

        var angle = AntSearching.theta * Mathf.Deg2Rad;

        var x = AntSearching.WanderRadius * Mathf.Cos(angle);
        var z = AntSearching.WanderRadius * Mathf.Sin(angle);
        wanderPoint += new float3(x, 0, z);

        return wanderPoint;
    }

    // public void ReverseDirection()
    // {
    //     AntSearching.theta = antRigidBody transform.eulerAngles.z;
    // }
}