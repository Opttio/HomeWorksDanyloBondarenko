using _Project.Scripts.Core.EventBus;
using _Project.Scripts.UI.Model;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.View
{
    public class FunMenuView : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        
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
        }

        private void UnSubscribeToEvents()
        {
            _playButton.onClick.RemoveListener(SignalToChangeCanvasToGame);
        }
        
        private void SignalToChangeCanvasToGame()
        {
            ViewModel.ViewId = 0;
            GameUIBus.ChangeViewId(ViewModel.ViewId);
        }
    }
}