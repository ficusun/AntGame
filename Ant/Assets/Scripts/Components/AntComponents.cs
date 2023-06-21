using Unity.Entities;
using Unity.Mathematics;

using UnityEngine;

public struct Ant : IComponentData
{
    public float Energy;

    public float CurrentLoad;

    public float3 TargetPosition;
}

[System.Serializable]
public struct AntStats : IComponentData
{
    
    public float MiningSpeed;
    public float MiningCost;
    public float CapacityLoad;
    public float MinTaskTime;
    public float LowEnergyAlarm;
}

public struct AntSpawner : IComponentData
{
    public float AntMaxCount;

    public float3 StartPosition;
    
    public Entity Prefab;

    public int AntPerTime;
}

[System.Serializable]
public struct AntMovementData : IComponentData, IEnableableComponent
{
    public float MoveCost;

    public float MaxSpeed;

    public float MaxForce;
}

[System.Serializable]
public struct AntSearching : IComponentData, IEnableableComponent
{
    public float AngleChange;

    public float AngleChangeRate;

    public Unity.Mathematics.Random Random;

    public float WanderRadius;
    // [HideInInspector] public float3 WanderPoint;
    [HideInInspector] public float angleChangeTimer;
    [HideInInspector] public float theta;
}

[System.Serializable]
public struct AntPheromoneData : IComponentData
{
    public float Distance;

    public bool ToSpawn;

    public float3 LastPheromonePosition;

    public Entity Prefab;
}