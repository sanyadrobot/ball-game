using DOTS.ComponentsAndTags;
using Unity.Entities;
using UnityEngine;

namespace DOTS.AuthoringAndMono
{
    public class ObstacleMono : MonoBehaviour
    {
    }

    public class ObstacleBaker : Baker<ObstacleMono>
    {
        public override void Bake(ObstacleMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ObstacleTag());
        }
    }
}