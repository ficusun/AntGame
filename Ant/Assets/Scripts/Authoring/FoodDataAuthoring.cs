using Unity.Entities;

using UnityEngine;

public class FoodDataAuthoring : MonoBehaviour
{
    public float Energy;

    public class FoodDataBaker : Baker<FoodDataAuthoring>
    {
        public override void Bake(FoodDataAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new FoodData
            {
                Energy = authoring.Energy
            });
        }
    }
}