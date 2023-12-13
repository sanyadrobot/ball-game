using System.Collections;
using DOTS.ComponentsAndTags;
using Mono.Services.Game;
using Unity.Entities;
using Utils;

namespace Mono.Services
{
    public class ObstacleSpawnerService
    {
        private readonly IGameService _gameService;
        private Entity _spawnerEntity = Entity.Null;

        private ObstacleSpawnerService(IGameService gameService, UnityMainThreadDispatcher unityMainThreadDispatcher)
        {
            _gameService = gameService;
            unityMainThreadDispatcher.Enqueue(SpawnObstacles());
        }

        private IEnumerator SpawnObstacles()
        {
            while (_spawnerEntity == Entity.Null)
            {
                _spawnerEntity = Helper.GetSingletonEntity<ObstaclesProperties>();
                yield return null;
            }
            var componentData = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<ObstaclesProperties>(
                _spawnerEntity);
            componentData.ObstaclesCount = _gameService.ObstaclesToSpawnCount;
            componentData.ExitPosition = _gameService.ExitPosition;
            componentData.Spawn = true;
            _spawnerEntity.SetComponent(componentData);
        }
    }
}