using Unity.Entities;
using Unity.Mathematics;

namespace DOTS.ComponentsAndTags
{
    public struct ObstaclesRandom : IComponentData
    {
        public Random Value;
    }
}
