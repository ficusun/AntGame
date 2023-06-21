using Unity.Entities;
using UnityEngine;

public class PheromoneDataAuthoring : MonoBehaviour
{
    public float Power;

    public class PheromoneDataBaker : Baker<PheromoneDataAuthoring>
    {
        public override void Bake(PheromoneDataAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PheromoneData
            {
                Power = authoring.Power
            });
        }
    }
}