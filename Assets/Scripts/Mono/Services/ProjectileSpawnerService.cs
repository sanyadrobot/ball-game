using System.Collections;
using DOTS.ComponentsAndTags.Projectile;
using Mono.Services.Game;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Utils;

namespace Mono.Services
{
    public class ProjectileSpawnerService : IProjectileSpawnerService
    {
        private Entity _spawnerEntity = Entity.Null;

        public ProjectileSpawnerService(UnityMainThreadDispatcher unityMainThreadDispatcher)
        {
            unityMainThreadDispatcher.Enqueue(InitSpawnerCoroutine());
        }

        public void SetSpawnerPosition(Transform t)
        {
            if (_spawnerEntity == Entity.Null) return;
            var localTransform = LocalTransform.FromPositionRotationScale(t.position, t.rotation, t.localScale.x);
            _spawnerEntity.SetComponent(localTransform);
        }

        private IEnumerator InitSpawnerCoroutine()
        {
            while (_spawnerEntity == Entity.Null)
            {
                _spawnerEntity = Helper.GetSingletonEntity<ProjectileSpawnerProperties>();
                yield return null;
            }
        }
        
        public void SpawnProjectile(float scale)
        {
            if (_spawnerEntity == Entity.Null) return;
            var componentData = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<ProjectileSpawnerProperties>(
                _spawnerEntity);
            componentData.Scale = scale;
            componentData.Spawn = true;
            componentData.ProjectileSpeed = 5f;
            _spawnerEntity.SetComponent(componentData);
        }
    }
}