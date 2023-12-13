using DOTS.Aspects;
using DOTS.ComponentsAndTags.Projectile;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace DOTS.Systems
{
  
    [UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(TransformSystemGroup))]
    [BurstCompile]
    public partial struct ProjectileSpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ProjectileSpawnerProperties>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entity = SystemAPI.GetSingletonEntity<ProjectileSpawnerProperties>();
            var projectileSpawnerAspect = SystemAPI.GetAspect<ProjectileSpawnerAspect>(entity);
            if(!projectileSpawnerAspect.ProjectileSpawnerProperties.Spawn) return;
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            var projectile = entityCommandBuffer.Instantiate(projectileSpawnerAspect.ProjectileSpawnerProperties.ProjectilePrefab);
            var physicsVelocity = new PhysicsVelocity();
            var entityForward = projectileSpawnerAspect.Transform.TransformDirection(math.forward());
            physicsVelocity.Linear = entityForward * projectileSpawnerAspect.ProjectileSpawnerProperties.ProjectileSpeed;
            var localTransform = projectileSpawnerAspect.Transform;
            localTransform.Scale = projectileSpawnerAspect.ProjectileSpawnerProperties.Scale;
            entityCommandBuffer.SetComponent(projectile, localTransform);
            entityCommandBuffer.SetComponent(projectile, physicsVelocity);
            var projectileSpawnerProperties = projectileSpawnerAspect.ProjectileSpawnerProperties;
            projectileSpawnerProperties.Spawn = false;
            entityCommandBuffer.SetComponent(entity, projectileSpawnerProperties);
            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}