using Mono.States;
using Mono.States.StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Mono.Services.Game
{
    public class GameService : StateMachine<GameTrigger>, IGameService, IInitializable
    {
        public int ObstaclesToSpawnCount { get; set; } = 1000;
        public Vector3 ExitPosition { get; } = new(10f, 0, 0);
        public Vector3 StartPosition { get; } = new(-3.55999994f, 0.5f, -25.1800003f);

        public GameService()
        {
            DefineState((() => new MainMenuState(this)));
            DefineState((() => new GameState(this)));
            DefineState((() => new LostState(this)));
            DefineState((() => new WinState(this)));
            
            DefineStartTransition<MainMenuState>(GameTrigger.MainMenu);
            DefineStartTransition<GameState>(GameTrigger.Play); 
            DefineTransition<MainMenuState, GameState>(GameTrigger.Play);
            DefineTransition<GameState, LostState>(GameTrigger.Lost);
            DefineTransition<GameState, WinState>(GameTrigger.Win);
            DefineTransition<WinState, MainMenuState>(GameTrigger.MainMenu);
            DefineTransition<LostState, MainMenuState>(GameTrigger.MainMenu);
        }

        public void Initialize()
        {
            var activeScene = SceneManager.GetActiveScene();

            if (activeScene.IsValid() && activeScene.name == "MenuScene")
            {
                Fire(GameTrigger.MainMenu);
            }
            else if (activeScene.IsValid() && activeScene.name == "GameScene")
            {
                Fire(GameTrigger.Play);
            }
            else
            {
                Debug.LogError("Unsupported first scene");
            }
        }
    }
}