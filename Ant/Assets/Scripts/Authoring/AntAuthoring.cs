using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

using UnityEngine.Serialization;

using Random = Unity.Mathematics.Random;

public class AntAuthoring : MonoBehaviour
{
    public float Energy;

    public float CurrentLoad;
    
    public float3 targetPos;
    
    public AntStats Stats;

    public AntMovementData MovementStat;

    public AntSearching antSearching;
    
    public GameObject PheromonePrefab;
    
    [FormerlySerializedAs("antPheromoneData")] public AntPheromoneSpawnerData antPheromoneSpawnerData;
    public class AntBaker : Baker<AntAuthoring>
    {
        public override void Bake(AntAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Ant
            {
                Energy = authoring.Energy,
                CurrentLoad = authoring.CurrentLoad,
                TargetPosition = authoring.targetPos,
            });
            
            AddComponent(entity, authoring.Stats);
            AddComponent(entity, authoring.MovementStat);
            
            authoring.antSearching.Random = new Unity.Mathematics.Random(3);
            AddComponent(entity, authoring.antSearching);
            
            authoring.antPheromoneSpawnerData.Prefab = GetEntity(authoring.PheromonePrefab, TransformUsageFlags.Dynamic);
            AddComponent(entity, authoring.antPheromoneSpawnerData);
        }
    }
}