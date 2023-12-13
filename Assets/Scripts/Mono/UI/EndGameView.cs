using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mono.UI
{
    public class EndGameView : MonoBehaviour
    {
        [SerializeField] private Button menuButton;
    
        public event Action OnMenuButtonClick;
        private void OnEnable()
        {
            menuButton.onClick.AddListener((() => OnMenuButtonClick?.Invoke()));
        }
    
        private void OnDisable()
        {
            menuButton.onClick.RemoveAllListeners();
        }
    }
}
