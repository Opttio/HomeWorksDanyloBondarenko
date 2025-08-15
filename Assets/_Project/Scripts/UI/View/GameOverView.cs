using _Project.Scripts.Core.EventBus;
using _Project.Scripts.Runtime.Controllers;
using _Project.Scripts.Runtime.Interfaces;
using _Project.Scripts.UI.Model;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.UI.View
{
    public class GameOverView : MonoBehaviour, IView
    {
        [SerializeField] private TextMeshProUGUI _distanceText;
        [SerializeField] private TextMeshProUGUI _attemptText;
        [SerializeField] private Button _toMenuButton;
        [SerializeField] private Button _continueButton;
        [Space]
        [SerializeField] private AudioClip _musicGameOver;
        [SerializeField] private AudioClip _uiClick;
        
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
            _continueButton.onClick.AddListener(SignalToContinue);
            _toMenuButton.onClick.AddListener(ResetScene);
        }

        private void UnSubscribeToEvents()
        {
            GameEventBus.OnDistanceChanged -= OnDistanceChanged;
            GameEventBus.OnAttemptChanged -= OnAttemptChanged;
            _continueButton.onClick.RemoveListener(SignalToContinue);
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
            PlayUiClick();
        }
        private void ResetScene()
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
        private void PlayUiClick()
        {
            AudioController.Instance.PlayUi(_uiClick);
        }

        public void OnViewActivated()
        {
            SetViewMusic();
        }

        private void SetViewMusic()
        {
            AudioController.Instance.StopAllAudioSources();
            AudioController.Instance.PlaySfx(_musicGameOver);
        }
    }
}