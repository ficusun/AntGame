using Unity.Entities;

using UnityEngine;

public struct FoodData : IComponentData
{
    public float Energy;
}

public struct FoodSpawner : IComponentData
{
    public int MaxCount;

    public float SpawnInterval;
    
    public Entity Prefab;

    public float FoodMinEnergy;

    public float FoodYPos;

    public double LastSpawnTime;
    
    public Unity.Mathematics.Random Random;

    public float FoodMaxEnergy;
}



