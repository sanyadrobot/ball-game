using Mono.Services.Game;
using Mono.States.StateMachine;
using Mono.UI;
using UnityEngine;

namespace Mono.States
{
    public class LostState : IState
    {
        private readonly IGameService _gameService;
        private EndGameView _view;

        public LostState(IGameService gameService)
        {
            _gameService = gameService;
        }
        
        public void OnEnter()
        {
            var resource = Resources.Load<EndGameView>("LostView");
            _view = Object.Instantiate(resource);
            _view.OnMenuButtonClick += ViewOnOnMenuButtonClick;
        }

        private async void ViewOnOnMenuButtonClick()
        {
            await Utils.Helper.LoadScene("MenuScene");
            _gameService.Fire(GameTrigger.MainMenu);
        }

        public void OnExit()
        {
            _view.OnMenuButtonClick -= ViewOnOnMenuButtonClick;
        }
    }
}