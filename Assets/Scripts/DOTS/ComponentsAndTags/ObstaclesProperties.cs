using Unity.Entities;
using Unity.Mathematics;

namespace DOTS.ComponentsAndTags
{
    public struct ObstaclesProperties : IComponentData
    {
        public float2 FieldDimensions;
        public int ObstaclesCount;
        public Entity ObstaclePrefab;
        public Entity DeadObstaclePrefab;
        public float3 ExitPosition;
        public bool Spawn;
    }
}
