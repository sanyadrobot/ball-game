using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mono.UI
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Button startButton;
        [SerializeField] private TMP_Text amountText;

        public event Action OnStartButtonClick;
        public event Action<float> OnSliderChange;
        private void OnEnable()
        {
            startButton.onClick.AddListener((() => OnStartButtonClick?.Invoke()));
            slider.onValueChanged.AddListener((SliderChange));
        }

        public void SetAmountText(int amount)
        {
            amountText.text = amount.ToString();
        }

        public float GetSliderValue()
        {
            return slider.value;
        }

        private void SliderChange(float arg)
        {
            OnSliderChange?.Invoke(arg);
        }

        private void OnDisable()
        {
            startButton.onClick.RemoveAllListeners();
            slider.onValueChanged.RemoveAllListeners();
        }
    }
}
