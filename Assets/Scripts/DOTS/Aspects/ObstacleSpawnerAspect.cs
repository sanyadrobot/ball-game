using DOTS.ComponentsAndTags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DOTS.Aspects
{
    public readonly partial struct ObstacleSpawnerAspect: IAspect
    {
        public int NumberTombstonesToSpawn => _obstaclesProperties.ValueRO.ObstaclesCount;
        public Entity ObstaclePrefab => _obstaclesProperties.ValueRO.ObstaclePrefab;
        public Entity DeadObstaclePrefab => _obstaclesProperties.ValueRO.DeadObstaclePrefab;
        
        private readonly RefRO<LocalTransform> _transform;
        private LocalTransform Transform => _transform.ValueRO;
        public ObstaclesProperties ObstaclesProperties => _obstaclesProperties.ValueRO;
        private readonly RefRO<ObstaclesProperties> _obstaclesProperties;
        private readonly RefRW<ObstaclesRandom> _obstaclesRandom;
        
        private float3 MinCorner => Transform.Position - HalfDimensions;
        private float3 MaxCorner => Transform.Position + HalfDimensions;
        private float3 HalfDimensions => new()
        {
            x = _obstaclesProperties.ValueRO.FieldDimensions.x * 0.5f,
            y = 0f,
            z = _obstaclesProperties.ValueRO.FieldDimensions.y * 0.5f
        };
        private const float EXIT_RADIUS = 10;
        
        public LocalTransform GetRandomObstacleTransform()
        {
            return new LocalTransform
            {
                Position = GetRandomPosition(),
                Rotation = quaternion.identity,
                Scale = 1f
            };
        }
        
        private float3 GetRandomPosition()
        {
            float3 randomPosition;
            do
            {
                randomPosition = _obstaclesRandom.ValueRW.Value.NextFloat3(MinCorner, MaxCorner);
            } while (math.distancesq(ObstaclesProperties.ExitPosition, randomPosition) <= EXIT_RADIUS);

            return randomPosition;
        }
        
    }
}