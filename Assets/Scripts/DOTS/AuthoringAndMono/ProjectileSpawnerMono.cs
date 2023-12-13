using DOTS.ComponentsAndTags.Projectile;
using Unity.Entities;
using UnityEngine;

namespace DOTS.AuthoringAndMono
{
    public class ProjectileSpawnerMono: MonoBehaviour
    {
        public GameObject projectilePrefab;
        public float scale;
        public bool spawn;
        public float projectileSpeed;
    }
    
    public class ProjectileSpawnerBaker : Baker<ProjectileSpawnerMono>
    {
        public override void Bake(ProjectileSpawnerMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ProjectileSpawnerProperties()
            {
                ProjectilePrefab = GetEntity(authoring.projectilePrefab,TransformUsageFlags.Dynamic),
                Scale = authoring.scale,
                Spawn = authoring.spawn,
                ProjectileSpeed = authoring.projectileSpeed
            });
        }
    }
}