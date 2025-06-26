using _Project.Scripts.Core.EventBus;
using _Project.Scripts.UI.Model;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.View
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        
        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void OnDisable()
        {
            UnSubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            _continueButton.onClick.AddListener(SignalToContinue);
        }

        private void UnSubscribeToEvents()
        {
            _continueButton.onClick.RemoveListener(SignalToContinue);
        }

        private void SignalToContinue()
        {
            if (CharacterModel.Attempt == 0)
                return;
            ViewModel.ViewId = 1;
            GameUIBus.ChangeViewId(ViewModel.ViewId);
            CharacterModel.Attempt--;
            GameEventBus.ChangeAttempt(CharacterModel.Attempt);
            CharacterModel.GameSpeed = 1f;
            GameEventBus.ChangeGameSpeed(CharacterModel.GameSpeed);
        }
    }
}