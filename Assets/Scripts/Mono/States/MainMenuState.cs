using Mono.Services.Game;
using Mono.States.StateMachine;
using Mono.UI;
using UnityEngine;

namespace Mono.States
{
    public class MainMenuState : IState
    {
        private readonly IGameService _gameService;

        public MainMenuState(IGameService gameService)
        {
            _gameService = gameService;
        }
        
        private MainMenuView _view;
        public void OnEnter()
        {
           var resource = Resources.Load<MainMenuView>("MainMenuView");
           _view = Object.Instantiate(resource);
           _view.OnStartButtonClick += ViewOnOnStartButtonClick;
           _view.OnSliderChange += ViewOnSliderChange;
           ViewOnSliderChange(_view.GetSliderValue());
        }

        private void ViewOnSliderChange(float obj)
        {
            _gameService.ObstaclesToSpawnCount = (int)(obj * 10000);
            _view.SetAmountText(_gameService.ObstaclesToSpawnCount);
        }

        private async void ViewOnOnStartButtonClick()
        {
            await Utils.Helper.LoadScene("GameScene");
            _gameService.Fire(GameTrigger.Play);
        }

        public void OnExit()
        {
            _view.OnStartButtonClick -= ViewOnOnStartButtonClick;
            _view.OnSliderChange -= ViewOnSliderChange;
        }
    }
}