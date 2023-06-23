using Unity.Entities;

using UnityEngine;

public class EntityTypeAuthoring : MonoBehaviour
{
    public TargetType Value;

    public class EntityTypeBaker : Baker<EntityTypeAuthoring>
    {
        public override void Bake(EntityTypeAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntityType
            {
                Value = authoring.Value
            });
        }
    }
}