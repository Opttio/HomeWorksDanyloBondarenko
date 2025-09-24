using System.Collections.Generic;
using _Project.Scripts.Core.EventBus;
using _Project.Scripts.Core.Extensions;
using _Project.Scripts.Data;
using _Project.Scripts.Environment.Data;
using UnityEngine;

namespace _Project.Scripts.Environment.Platforms
{
    public class LocationGenerator : MonoBehaviour
    {
        [SerializeField] private PlatformsGenerationPattern _generationPattern;
        [SerializeField] private Transform _platformsParent;
        [SerializeField] private int _initialPlatforms = 15;
        [SerializeField] private float _removingPlatformsHeight = 10f;
        [SerializeField] private Transform _playerTarget;
    
        private Queue<PlatformsGroupData.GroupSpawnResult> _platformsQueue = new();
        private float _highestY = 0f;
        private bool _isLoaded = false;
        private float _previousHighestY = 0f;

        private void Start()
        {
            _generationPattern.Init(_playerTarget, _platformsParent, GetScreenWidth());
            if (!_isLoaded)
            {
                GenerateInitialPlatforms();
            }
        }

        private void Update()
        {
            if (_playerTarget.position.y + _removingPlatformsHeight > _highestY)
            {
                SpawnNextGroup();
                CleanPlatformGroup();
            }

            SignalWhenHighestYIsChanged();
        }

        private Vector2 GetScreenWidth()
        {
            var screenWidth = Camera.main.GetScreenHorizontalBounds(0f);
            return new Vector2(screenWidth.Left, screenWidth.Right);
        }

        private void GenerateInitialPlatforms()
        {
            for (int i = 0; i < _initialPlatforms; i++)
            {
                SpawnNextGroup();
            }
        }

        private void SpawnNextGroup()
        {
            var result = _generationPattern.SpawnNextGroup(_highestY);
            _highestY = result.LastSpawnedHeight;
            _platformsQueue.Enqueue(result);
        }

        private void CleanPlatformGroup()
        {
            if (_platformsQueue.Count == 0)
                return;

            var nextGroup = _platformsQueue.Peek();
            bool shouldRemove = true;

            foreach (var platform in nextGroup.SpawnedPlatforms)
            {
                if (!platform) continue;

                if (platform.transform.position.y > _playerTarget.position.y - _removingPlatformsHeight)
                {
                    shouldRemove = false;
                    break;
                }
            }

            if (shouldRemove)
            {
                var result = _platformsQueue.Dequeue();
                foreach (var platform in result.SpawnedPlatforms)
                {
                    if (platform)
                    {
                        Destroy(platform.gameObject);
                    }
                }
            }
        }
        
        private void ClearPlatforms()
        {
            while (_platformsQueue.Count > 0)
            {
                var group = _platformsQueue.Dequeue();
                foreach (var platform in group.SpawnedPlatforms)
                {
                    if (platform) Destroy(platform.gameObject);
                }
            }
        }

        private void SignalWhenHighestYIsChanged()
        {
            if (!Mathf.Approximately(_previousHighestY, _highestY))
            {
                _previousHighestY = _highestY;
                GameEventBus.ChangeHighestY(_highestY);
            }
        }
        
        public float GetCurrentHighestY() => _highestY;
        public void SetHighestY(float value) => _highestY = value;
        public void LoadState(PlayerSaveData data)
        {
            _highestY = data.highestY;
            _isLoaded = true;
            ClearPlatforms();
            SpawnNextGroup();
        }
        public void GenerateInitialPlatformsAround(float playerY)
        {
            ClearPlatforms();
            _highestY = playerY;

            for (int i = 0; i < _initialPlatforms; i++)
            {
                SpawnNextGroup();
            }
        }
    }
}
