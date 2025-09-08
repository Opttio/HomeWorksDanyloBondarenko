using System.Collections.Generic;
using _Project.Scripts.Environment.Data;
using UnityEngine;

namespace _Project.Scripts.Environment.Platforms
{
    public class LocationGenerator : MonoBehaviour
    {
        // [SerializeField] private GameObject[] _platforms;
        [SerializeField] private PlatformsGenerationPattern _generationPattern;
        [SerializeField] private Transform _platformsParent;
        [SerializeField] private int _initialPlatforms = 15;
        // [SerializeField] private float _distanceBetweenPlatforms = 2f;
        [SerializeField] private float _removingPlatformsHeight = 10f;
        [SerializeField] private Transform _playerTarget;
    
        // private Queue<GameObject> _platformsQueue = new();
        private Queue<PlatformsGroupData.GroupSpawnResult> _platformsQueue = new();

        private float _screenLeftX;
        private float _screenRightX;
        private float _highestY = 0f;

        private void Start()
        {
            DetermineTheScreenSize();
            _generationPattern.Init(_playerTarget, _platformsParent, GetScreenWidth());
            GenerateInitialPlatforms();
        }

        private void Update()
        {
            if (_playerTarget.position.y + _removingPlatformsHeight > _highestY)
            {
                // GeneratePlatformRow();
                SpawnNextGroup();
                CleanPlatformGroup();
            }
        }

        public Vector2 GetScreenWidth()
        {
            return new Vector2(_screenLeftX, _screenRightX);
        }

        private void DetermineTheScreenSize()
        {
            var camera = Camera.main;
            Vector3 leftPoint = camera.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 rightPoint = camera.ViewportToWorldPoint(new Vector3(1, 0, 0));
            _screenLeftX = leftPoint.x;
            _screenRightX = rightPoint.x;
        }

        private void GenerateInitialPlatforms()
        {
            for (int i = 0; i < _initialPlatforms; i++)
            {
                // GeneratePlatformRow();
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

        /*private void GeneratePlatformRow()
        {
            float x = Random.Range (_screenLeftX + 0.25f, _screenRightX - 0.25f);
            var position = new Vector3(x, _highestY, 0f);
            var platform = _platforms[Random.Range(0, _platforms.Length)];
            var platformInstance = Instantiate(platform, position, Quaternion.identity, _platformsParent);
            _platformsQueue.Enqueue(platformInstance);
        
            _highestY += _distanceBetweenPlatforms;
        }*/

        /*private void CleanPlatformRow()
        {
            if (_platformsQueue.Count > 0)
            {
                Destroy(_platformsQueue.Dequeue());
            }
        }*/
    }
}
