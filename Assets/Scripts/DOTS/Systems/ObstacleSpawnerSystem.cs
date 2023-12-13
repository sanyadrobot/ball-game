using DOTS.Aspects;
using DOTS.ComponentsAndTags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace DOTS.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct ObstacleSpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ObstaclesProperties>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var obstaclesEntity = SystemAPI.GetSingletonEntity<ObstaclesProperties>();
            var obstaclesAspect = SystemAPI.GetAspect<ObstacleSpawnerAspect>(obstaclesEntity);
            if(!obstaclesAspect.ObstaclesProperties.Spawn) return;
            var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);
            for (var i = 0; i < obstaclesAspect.NumberTombstonesToSpawn; i++)
            {
                var obstacle = entityCommandBuffer.Instantiate(obstaclesAspect.ObstaclePrefab);
                var obstacleTransform = obstaclesAspect.GetRandomObstacleTransform();
                entityCommandBuffer.SetComponent(obstacle, obstacleTransform);
            }

            var properties = SystemAPI.GetComponent<ObstaclesProperties>(obstaclesEntity);
            properties.Spawn = false;
            entityCommandBuffer.SetComponent(obstaclesEntity, properties);
            entityCommandBuffer.Playback(state.EntityManager);
        }
    }
}