using System.Linq;
using UnityEngine;


namespace _Project.Scripts.Environment.Data
{
    [CreateAssetMenu(fileName = "PlatformsGenerationPattern", menuName = "Environment/PlatformsGenerationPattern", order = 0)]
    public class PlatformsGenerationPattern : ScriptableObject
    {
        [SerializeField] private PlatformsGroupSpawnData[] _platformsPattern;
        
        [System.NonSerialized] private Transform _target;
        [System.NonSerialized] private Transform _parent;
        [System.NonSerialized] private Vector2 _screenWidth;

        public void Init(Transform target, Transform parent, Vector2 screenWidth)
        {
            _target = target;
            _parent = parent;
            _screenWidth = screenWidth;
        }

        public PlatformsGroupData.GroupSpawnResult SpawnNextGroup(float startSpawnHeight)
        {
            var randomGroup = GetRandomGroup();
            return randomGroup.SpawnGroup(_target, _parent, _screenWidth, startSpawnHeight);
        }

        private PlatformsGroupData GetRandomGroup()
        {
            var chanceSum = _platformsPattern.Sum(pattern => pattern.Chance);
            var randomValue = Random.Range(0, chanceSum);
            var currentChance = 0;

            for (var i = 0; i < _platformsPattern.Length; i++)
            {
                currentChance += _platformsPattern[i].Chance;
                if (randomValue < currentChance)
                {
                    return _platformsPattern[i].Group;
                }
            }
            return _platformsPattern.Last().Group;
        }
        
        [System.Serializable]
        public struct PlatformsGroupSpawnData
        {
            public PlatformsGroupData Group;
            public int Chance;
        }
    }
}