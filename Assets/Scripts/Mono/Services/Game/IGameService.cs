using UnityEngine;

namespace Mono.Services.Game
{
    public interface IGameService
    {
        int ObstaclesToSpawnCount { get; set; }
        Vector3 ExitPosition { get; }
        Vector3 StartPosition { get; }
        void Fire(GameTrigger gameTrigger);
    }

    public enum GameTrigger
    {
        Play,
        MainMenu,
        Lost,
        Win
    }
}