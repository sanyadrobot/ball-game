using DOTS.ComponentsAndTags.Projectile;
using Unity.Entities;
using Unity.Transforms;

namespace DOTS.Aspects
{
    public readonly partial struct ProjectileSpawnerAspect: IAspect
    {
        public readonly Entity Entity;
        private readonly RefRO<LocalTransform> _transform;
        public LocalTransform Transform => _transform.ValueRO;
        private readonly RefRO<ProjectileSpawnerProperties> _projectileSpawnerProperties;
        public ProjectileSpawnerProperties ProjectileSpawnerProperties => _projectileSpawnerProperties.ValueRO;
    }
}