using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.Extensions;
using _Project.Scripts.Environment.Data;
using UnityEngine;

namespace _Project.Scripts.Environment.Platforms
{
    public class PlatformPoolManager : MonoBehaviour
    {
        [SerializeField] private List<MyPlatformsData> _platformDataList;
        [SerializeField] private int _poolSize = 32;
        [SerializeField] private Transform _platformsParent;
        private readonly List<GameObject> _platformPool = new List<GameObject>();

        private void Awake()
        {
            for (var i = 0; i < _poolSize; i++)
            {
                var selected = _platformDataList.GetRandomByWeight(p => p.Chance);
                var instance = Instantiate(selected.PlatformPrefab, _platformsParent);
                ReturnPlatform(instance);
                _platformPool.Add(instance);
            }
        }
        
        public void ReturnPlatform(GameObject platform)
        {
            platform.SetActive(false);
        }

        public GameObject GetFreePlatform()
        {
            var inactivePlatforms = _platformPool.Where(p => !p.activeInHierarchy).ToList();
            if (inactivePlatforms.Count == 0)
                return null;

            return inactivePlatforms[Random.Range(0, inactivePlatforms.Count)];
        }
    }
}