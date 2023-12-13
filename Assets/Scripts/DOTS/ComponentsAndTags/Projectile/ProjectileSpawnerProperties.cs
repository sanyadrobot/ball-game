using Unity.Entities;

namespace DOTS.ComponentsAndTags.Projectile
{
    public struct ProjectileSpawnerProperties : IComponentData
    {
        public Entity ProjectilePrefab;
        public float Scale;
        public bool Spawn;
        public float ProjectileSpeed;
    }
}