using Unity.Entities;

using UnityEngine;

public class FoodSpawnerAuthoring : MonoBehaviour
{
    public int MaxCount;

    public float SpawnInterval;

    public float FoodMinEnergy;
    
    public GameObject Prefab;

    public float FoodMaxEnergy;
    
    [Min(1)]
    public uint RandomSeed = 1;
    
    public float FoodYPos;

    public class FoodSpawnerBaker : Baker<FoodSpawnerAuthoring>
    {
        public override void Bake(FoodSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new FoodSpawner
            {
                MaxCount = authoring.MaxCount,
                SpawnInterval = authoring.SpawnInterval,
                FoodMinEnergy = authoring.FoodMinEnergy,
                Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                FoodYPos = authoring.FoodYPos,
                FoodMaxEnergy = authoring.FoodMaxEnergy,
                Random = new Unity.Mathematics.Random(authoring.RandomSeed),
            });
        }
    }
}