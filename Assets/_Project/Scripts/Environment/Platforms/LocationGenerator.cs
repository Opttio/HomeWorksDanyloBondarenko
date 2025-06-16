using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Environment.Platforms
{
    public class LocationGenerator : MonoBehaviour
    {
        //Код піддивився у Ярослава в туторіалі

        [SerializeField] private GameObject[] _platforms;
        [SerializeField] private Transform _platformsParent;
        [SerializeField] private int _initialPlatforms = 15;
        [SerializeField] private float _distanceBetweenPlatforms = 2f;
        [SerializeField] private float _removingPlatformsHight = 10f;
        [SerializeField] private Transform _playerTarget;
    
        private Queue<GameObject> _platformsQueue = new();

        private float _screenLeftX;
        private float _screenRightX;
        private float _highestY = 0f;

        private void Start()
        {
            var camera = Camera.main;
            Vector3 leftPoint = camera.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 rightPoint = camera.ViewportToWorldPoint(new Vector3(1, 0, 0));
            _screenLeftX = leftPoint.x;
            _screenRightX = rightPoint.x;

            GanerateInitialPlatforms();
        }

        private void Update()
        {
            if (_playerTarget.position.y + _removingPlatformsHight > _highestY)
            {
                GeneratePlatformRow();
                CleanPlatformRow();
            }
        }

        private void GanerateInitialPlatforms()
        {
            for (int i = 0; i < _initialPlatforms; i++)
            {
                GeneratePlatformRow();
            }
        }

        private void GeneratePlatformRow()
        {
            float x = Random.Range (_screenLeftX + 0.25f, _screenRightX - 0.25f);
            var position = new Vector3(x, _highestY, 0f);
            var platform = _platforms[Random.Range(0, _platforms.Length)];
            var platformInstance = Instantiate(platform, position, Quaternion.identity, _platformsParent);
            _platformsQueue.Enqueue(platformInstance);
        
            _highestY += _distanceBetweenPlatforms;
        }

        private void CleanPlatformRow()
        {
            if (_platformsQueue.Count > 0)
            {
                Destroy(_platformsQueue.Dequeue());
            }
        }
    }
}
