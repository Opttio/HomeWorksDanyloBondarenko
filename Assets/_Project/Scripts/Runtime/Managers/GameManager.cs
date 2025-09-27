using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using _Project.Scripts.Core.EventBus;
using _Project.Scripts.Data;
using _Project.Scripts.Environment.Platforms;
using _Project.Scripts.Runtime.Bootstraps;
using _Project.Scripts.UI.Model;

namespace _Project.Scripts.Runtime.Managers
{
    public class GameManager : MonoBehaviour
    {
        // [SerializeField] private LocationGenerator _locationGenerator;
        [SerializeField] private LocationGeneratorFromPool _locationGenerator;

        private PlayerSaveData _playerSaveData = new PlayerSaveData();
        private CancellationTokenSource _autoSaveCts;

        private void OnEnable()
        {
            GameEventBus.OnPositionChanged += ChangePosition;
            GameEventBus.OnAttemptChanged += ChangeAttempt;
            GameEventBus.OnDistanceChanged += ChangeDistance;
            GameEventBus.OnHighestYChanged += ChangeHighestY;

            StartAutoSave().Forget();
        }

        private void OnDisable()
        {
            GameEventBus.OnPositionChanged -= ChangePosition;
            GameEventBus.OnAttemptChanged -= ChangeAttempt;
            GameEventBus.OnDistanceChanged -= ChangeDistance;
            GameEventBus.OnHighestYChanged -= ChangeHighestY;

            _autoSaveCts?.Cancel();
            _autoSaveCts?.Dispose();
            _autoSaveCts = null;
        }

        private async void Start()
        {
            await WaitForFirebaseReady();
            await LoadGame();
        }

        private void ChangePosition(Vector2 position)
        {
            _playerSaveData.posX = position.x;
            _playerSaveData.posY = position.y;
        }

        private void ChangeAttempt(int attempt)
        {
            _playerSaveData.attempt = attempt;
        }

        private void ChangeDistance(int distance)
        {
            _playerSaveData.distance = distance;
        }

        private void ChangeHighestY(float highestY)
        {
            _playerSaveData.highestY = highestY;
        }

        private async UniTaskVoid StartAutoSave()
        {
            _autoSaveCts = new CancellationTokenSource();
            var token = _autoSaveCts.Token;

            try
            {
                while (!token.IsCancellationRequested)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
                    await SaveGame(token);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("[GameManager] Автозбереження зупинено.");
            }
        }

        private async UniTask SaveGame(CancellationToken token)
        {
            if (!FirebaseBootstrap.IsReady)
            {
                Debug.LogWarning("[GameManager] Firebase не готовий для збереження.");
                return;
            }

            try
            {
                string path = $"users/{FirebaseBootstrap.Uid}/saveData";
                string json = JsonUtility.ToJson(_playerSaveData);
                await FirebaseBootstrap.Db.Child(path).SetRawJsonValueAsync(json).AsUniTask().AttachExternalCancellation(token);
                Debug.Log("[GameManager] Автозбереження успішне!");
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("[GameManager] Збереження скасовано.");
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameManager] Помилка збереження: {e}");
            }
        }

        public async UniTask LoadGame()
        {
            if (!FirebaseBootstrap.IsReady)
            {
                Debug.LogWarning("[GameManager] Firebase не готовий для завантаження.");
                SetDefaultCharacterData();
                CharacterModel.IsLoaded = true;
                return;
            }

            try
            {
                string path = $"users/{FirebaseBootstrap.Uid}/saveData";
                var dataSnapshot = await FirebaseBootstrap.Db.Child(path).GetValueAsync().AsUniTask();

                if (!dataSnapshot.Exists)
                {
                    Debug.Log("[GameManager] Немає збережених даних. Встановлюємо стандартні.");
                    SetDefaultCharacterData();
                    CharacterModel.IsLoaded = true;
                    return;
                }

                string json = dataSnapshot.GetRawJsonValue();
                var loadedData = JsonUtility.FromJson<PlayerSaveData>(json);
                _playerSaveData = loadedData;

                // Встановлення даних
                Vector2 loadedPosition = new Vector2(loadedData.posX, loadedData.posY);
                CharacterModel.Position = loadedPosition;
                CharacterModel.Attempt = loadedData.attempt;
                CharacterModel.Distance = loadedData.distance;
                CharacterModel.HighestY = loadedData.highestY;

                // Передача даних в інші системи
                _locationGenerator.LoadState(_playerSaveData);
                _locationGenerator.GenerateInitialPlatformsAround(CharacterModel.Position.y);
                GameEventBus.ChangePosition(loadedPosition);
                GameEventBus.ChangeAttempt(loadedData.attempt);
                GameEventBus.ChangeDistance(loadedData.distance);
                GameEventBus.ChangeHighestY(loadedData.highestY);

                CharacterModel.IsLoaded = true;
                Debug.Log("[GameManager] Дані гравця завантажено успішно.");
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameManager] Помилка при завантаженні: {e}");
                SetDefaultCharacterData();
                CharacterModel.IsLoaded = true;
            }
        }

        private async UniTask WaitForFirebaseReady()
        {
            float timeout = 10f;
            float elapsed = 0f;

            while (!FirebaseBootstrap.IsReady && elapsed < timeout)
            {
                await UniTask.Delay(100);
                elapsed += 0.1f;
            }

            if (!FirebaseBootstrap.IsReady)
                Debug.LogWarning("[GameManager] Firebase не готовий після 10 секунд очікування.");
            else
                Debug.Log("[GameManager] Firebase готовий.");
        }

        private void SetDefaultCharacterData()
        {
            CharacterModel.Position = Vector2.zero;
            CharacterModel.Attempt = 3;
            CharacterModel.Distance = 0;
            CharacterModel.HighestY = 0;

            GameEventBus.ChangePosition(Vector2.zero);
            GameEventBus.ChangeAttempt(3);
            GameEventBus.ChangeDistance(0);
            GameEventBus.ChangeHighestY(0);
        }
    }
}