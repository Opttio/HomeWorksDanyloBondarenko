using _Project.Scripts.Core.EventBus;
using _Project.Scripts.UI.Model;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.UI.View
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _distanceText; //Узагальнений варіант
        [SerializeField] private TextMeshProUGUI _attemptText; //Беспосередньо до тексту у канвасі
        [SerializeField] private Button _toMenuButton;
        
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
            GameEventBus.OnDistanceChanged += OnDistanceChanged;
            GameEventBus.OnAttemptChanged += OnAttemptChanged;
            
            _toMenuButton.onClick.AddListener(SignalToChangeCanvasToMenu);
            _toMenuButton.onClick.AddListener(ResetScene);
        }

        private void UnSubscribeToEvents()
        {
            GameEventBus.OnDistanceChanged -= OnDistanceChanged;
            GameEventBus.OnAttemptChanged -= OnAttemptChanged;
            
            _toMenuButton.onClick.RemoveListener(SignalToChangeCanvasToMenu);
            _toMenuButton.onClick.RemoveListener(ResetScene);
        }

        private void OnDistanceChanged(int distance)
        {
            _distanceText.text = distance.ToString();
        }

        private void OnAttemptChanged(int attempt)
        {
            _attemptText.text = attempt.ToString();
        }

        private void SignalToChangeCanvasToMenu()
        {
            // CharacterModel.Attempt = 0;
            // GameEventBus.ChangeAttempt(CharacterModel.Attempt);
            // CharacterModel.Distance = 0;
            // GameEventBus.ChangeDistance(CharacterModel.Distance);
            ViewModel.ViewId = 0;
            GameUIBus.ChangeViewId(ViewModel.ViewId);
            CharacterModel.GameSpeed = 0f;
            GameEventBus.ChangeGameSpeed(CharacterModel.GameSpeed);
        
        }
        private void ResetScene()
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}