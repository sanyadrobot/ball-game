using DOTS.ComponentsAndTags;
using Unity.Entities;
using UnityEngine;

namespace DOTS.AuthoringAndMono
{
    public class DeadObstacleMono : MonoBehaviour
    {
        public float lifeTime;
    }

    public class DeadObstacleBaker : Baker<DeadObstacleMono>
    {
        public override void Bake(DeadObstacleMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new DeadObstacleProperty()
            {
                LifeTime = authoring.lifeTime
            });
        }
    }
}