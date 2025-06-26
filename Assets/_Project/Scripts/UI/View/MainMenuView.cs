using _Project.Scripts.Core.EventBus;
using _Project.Scripts.UI.Model;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.View
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _exitButton;
        
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
            _playButton.onClick.AddListener(SignalToChangeCanvasToGame);
            _exitButton.onClick.AddListener(SignalToChangeCanvasToFunView);
        }

        private void UnSubscribeToEvents()
        {
            _playButton.onClick.RemoveListener(SignalToChangeCanvasToGame);
            _exitButton.onClick.RemoveListener(SignalToChangeCanvasToFunView);
        }
        
        private void SignalToChangeCanvasToGame()
        {
            ViewModel.ViewId = 1;
            GameUIBus.ChangeViewId(ViewModel.ViewId);
            CharacterModel.GameSpeed = 1f;
            GameEventBus.ChangeGameSpeed(CharacterModel.GameSpeed);
        }

        private void SignalToChangeCanvasToFunView()
        {
            ViewModel.ViewId = 3;
            GameUIBus.ChangeViewId(ViewModel.ViewId);
        }
    }
}