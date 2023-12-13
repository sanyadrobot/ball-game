using Mono.Services.Game;
using Mono.States.StateMachine;

namespace Mono.States
{
    public class GameState : IState 
    {
        private readonly IGameService _gameService;

        public GameState(IGameService gameService)
        {
            _gameService = gameService;
        }

        public void OnEnter()
        {
            
        }
        

        public void OnExit()
        {
          
        }
    }
}