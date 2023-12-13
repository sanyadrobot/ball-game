using Unity.Entities;

namespace DOTS.ComponentsAndTags
{
    public struct DeadObstacleProperty : IComponentData
    {
        public float LifeTime;
    }
}