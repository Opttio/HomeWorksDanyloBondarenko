using System.Collections.Generic;
using _Project.Scripts.Core.EventBus;
using _Project.Scripts.Core.Extensions;
using _Project.Scripts.Data;
using UnityEngine;

namespace _Project.Scripts.Environment.Platforms
{
    public class LocationGeneratorFromPool : MonoBehaviour
    {
        [SerializeField] private PlatformPoolManager _pool;
        [SerializeField] private int _initialPlatforms = 15;
        [SerializeField] private float _removingPlatformsHeight = 10f;
        [SerializeField] private float _heightBounds = 2f;
        [SerializeField] private Transform _playerTarget;
    
        private float _highestY = 0f;
        private bool _isLoaded = false;
        private float _lastSignaledHighestY = 0f;
        private Vector2 _screenWidth;
        private Queue<GameObject> _platformsQueue = new();

        private void Start()
        {
            _screenWidth = GetScreenWidth();
            if (!_isLoaded)
            {
                GenerateInitialPlatforms();
            }
        }

        private void Update()
        {
            if (_playerTarget.position.y + _removingPlatformsHeight > _highestY)
            {
                SpawnNextPlatform();
                CleanOldPlatform();
            }
            SignalWhenHighestYIsChanged();
        }

        private void GenerateInitialPlatforms()
        {
            for (int i = 0; i < _initialPlatforms; i++)
            {
                SpawnNextPlatform();
            }
        }

        private void SpawnNextPlatform()
        {
            var platform = _pool.GetFreePlatform();
            if (!platform)
                return;
            platform.transform.position = GetNextPlatformPosition();
            platform.SetActive(true);
            _platformsQueue.Enqueue(platform);
        }

        private Vector3 GetNextPlatformPosition()
        {
            _highestY += _heightBounds;
            var point = new Vector3(Random.Range(_screenWidth.x, _screenWidth.y), _highestY, 0f);
            return point;
        }

        private Vector2 GetScreenWidth()
        {
            var screenWidth = Camera.main.GetScreenHorizontalBounds(0f);
            return new Vector2(screenWidth.Left, screenWidth.Right);
        }

        private void CleanOldPlatform()
        {
            var oldPlatform = _platformsQueue.Dequeue();
            _pool.ReturnPlatform(oldPlatform);
        }

        private void ClearPlatforms()
        {
            while (_platformsQueue.Count > 0)
            {
                var platform = _platformsQueue.Dequeue();
                _pool.ReturnPlatform(platform);
            }
        }

        private void SignalWhenHighestYIsChanged()
        {
            if (!Mathf.Approximately(_lastSignaledHighestY, _highestY))
            {
                _lastSignaledHighestY = _highestY;
                GameEventBus.ChangeHighestY(_highestY);
            }
        }
        
        public void LoadState(PlayerSaveData data)
        {
            _highestY = data.highestY;
            _isLoaded = true;
            ClearPlatforms();
            SpawnNextPlatform();
        }
        public void GenerateInitialPlatformsAround(float playerY)
        {
            ClearPlatforms();
            _highestY = playerY;

            for (int i = 0; i < _initialPlatforms; i++)
            {
                SpawnNextPlatform();
            }
        }
    }
}