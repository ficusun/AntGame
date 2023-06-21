using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    public partial struct AntPheromoneSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {

        }
    }
}