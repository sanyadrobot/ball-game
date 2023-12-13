using System;
using System.Linq;
using DOTS.Aspects;
using DOTS.ComponentsAndTags;
using Mono;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Utils;
using Zenject;

namespace DOTS.Systems
{
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem.Singleton))]
    public partial class DestroySystem : SystemBase
    {
        public static event Action OnEntitiesDestroy; // I know that this is bad
        
        protected override void OnCreate()
        {
            RequireForUpdate<ObstacleTag>();
            base.OnCreate();
        }

        protected override void OnUpdate()
        {
            var touchedEntities = Helper.GetSystemsManagedBurst<CollisionSystem.Touched>(EntityManager);
            if(touchedEntities.Length <= 0) return;
            
            var obstaclesEntity = SystemAPI.GetSingletonEntity<ObstaclesProperties>();
            var obstaclesAspect = SystemAPI.GetAspect<ObstacleSpawnerAspect>(obstaclesEntity);
            
            var touched = SystemAPI.GetComponent<CollisionSystem.Touched>(touchedEntities.Last());
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);  
            if(touched.CollisionEvent.EntityA == Entity.Null || touched.CollisionEvent.EntityB == Entity.Null) return;
            if(!SystemAPI.Exists(touched.CollisionEvent.EntityA) || !SystemAPI.Exists(touched.CollisionEvent.EntityB)) return;

            if(!SystemAPI.HasComponent<ObstacleTag>(touched.CollisionEvent.EntityB)) return;
            var deadEntity = entityCommandBuffer.Instantiate(obstaclesAspect.DeadObstaclePrefab);
            var ballTransform = SystemAPI.GetComponent<LocalTransform>(touched.CollisionEvent.EntityA);
            var obstacleTransform = SystemAPI.GetComponent<LocalTransform>(touched.CollisionEvent.EntityB);
            entityCommandBuffer.SetComponent(deadEntity, obstacleTransform);
            var entities = Helper.GetSystemsManagedBurst<ObstacleTag>(EntityManager);
            var radius =  math.pow(ballTransform.Scale, 2); 

            var nativeList = new NativeList<Entity>(Allocator.Persistent);

            foreach (var entity in entities)
            {
                var transform = SystemAPI.GetComponent<LocalTransform>(entity);
                
                if (math.distance(transform.Position, ballTransform.Position) <= radius || entity == touched.CollisionEvent.EntityB)
                {
                    var deadEntity1 = entityCommandBuffer.Instantiate(obstaclesAspect.DeadObstaclePrefab);
                    entityCommandBuffer.SetComponent(deadEntity1, transform);
                    nativeList.Add(entity);
                }
            }
           
            nativeList.Add(touched.CollisionEvent.EntityA);
            nativeList.AddRange(touchedEntities);
            entityCommandBuffer.DestroyEntity(nativeList.AsArray());
            entityCommandBuffer.Playback(EntityManager);
            nativeList.Dispose();
            OnEntitiesDestroy?.Invoke();
        }
    }
}