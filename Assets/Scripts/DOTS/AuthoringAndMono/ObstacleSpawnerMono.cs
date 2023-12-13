using DOTS.ComponentsAndTags;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace DOTS.AuthoringAndMono
{
    public class ObstacleSpawnerMono : MonoBehaviour
    {
        public float2 fieldDimensions;
        public int obstaclesCount;
        public GameObject obstaclePrefab;
        public GameObject deadObstaclePrefab;
        public uint randomSeed;
        public float3 exitPosition;
        public bool spawn;
    }

    public class ObstaclesBaker : Baker<ObstacleSpawnerMono>
    {
        public override void Bake(ObstacleSpawnerMono authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ObstaclesProperties
            {
                FieldDimensions = authoring.fieldDimensions,
                ObstaclesCount = authoring.obstaclesCount,
                ObstaclePrefab = GetEntity(authoring.obstaclePrefab,TransformUsageFlags.Dynamic),
                DeadObstaclePrefab = GetEntity(authoring.deadObstaclePrefab,TransformUsageFlags.Dynamic),
                ExitPosition = authoring.exitPosition,
                Spawn = authoring.spawn
            });
            AddComponent(entity, new ObstaclesRandom
            {
                Value = Random.CreateFromIndex(authoring.randomSeed)
            });
        }
    }
}