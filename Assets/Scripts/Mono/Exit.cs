using Mono.Services.Game;
using UnityEngine;
using Zenject;

namespace Mono
{
    public class Exit : MonoBehaviour
    {
        [Inject] private IGameService _gameService;

        [SerializeField] private TriggerController doorOpenerTriggerArea;
        [SerializeField] private TriggerController winTriggerArea;
        [SerializeField] private Animator animator;

        private bool _gameIsWon = false;
        private void Awake()
        {
            doorOpenerTriggerArea.OnTrigger += OpenDoor;
            winTriggerArea.OnTrigger += WinGame;
            SetLookRotation(_gameService.StartPosition);
        }

        private void WinGame()
        {
            if(_gameIsWon) return;
            _gameIsWon = true;
            _gameService.Fire(GameTrigger.Win); 
        }

        private void OnDisable()
        {
            doorOpenerTriggerArea.OnTrigger -= OpenDoor;
            winTriggerArea.OnTrigger -= OpenDoor;
        }

        private void OpenDoor()
        {
            animator.enabled = true;
        }

        private void SetLookRotation(Vector3 lookPosition)
        {
            var targetPosition = new Vector3(lookPosition.x, transform.position.y, lookPosition.z);
            transform.LookAt(targetPosition);
        }
    }
}