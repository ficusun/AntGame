using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Aspects;
using Unity.Transforms;

using UnityEngine;

public readonly partial struct AntMovementAspect : IAspect
{
    readonly RefRW<AntMovementData> antMovement;
    readonly RefRW<LocalTransform> antTransform;
    readonly RigidBodyAspect antRigidBody;
    public ref AntMovementData AntMovement => ref antMovement.ValueRW;

    public void Move(float3 targetPos, float dt)
    {
        var desired = targetPos - antRigidBody.Position;
        desired = MathUtility.SetMagnitude(desired, AntMovement.MaxSpeed);

        var steeringForce = desired - antRigidBody.LinearVelocity;
        steeringForce = Vector3.ClampMagnitude(steeringForce, AntMovement.MaxForce);
        antRigidBody.ApplyLinearImpulseWorldSpace(steeringForce * dt);
        antRigidBody.LinearVelocity = Vector3.ClampMagnitude(antRigidBody.LinearVelocity, AntMovement.MaxSpeed);
        var direction = antRigidBody.LinearVelocity;
        direction.y = 0f;
        antTransform.ValueRW.Rotation = quaternion.LookRotationSafe(direction, new float3(0,1f,0));
    }
}