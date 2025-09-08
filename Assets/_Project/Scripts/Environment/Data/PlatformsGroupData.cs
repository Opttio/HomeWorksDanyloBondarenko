using System.Collections.Generic;
using _Project.Scripts.Environment.Platforms;
using UnityEngine;

namespace _Project.Scripts.Environment.Data
{
    [CreateAssetMenu(fileName = "PlatformsGroup", menuName = "Environment/PlatformsGroup", order = 1)]
    public class PlatformsGroupData : ScriptableObject
    {
        [SerializeField] private PlatformRow[] _platformsGroup;
        [SerializeField] private Vector2 _heightBounds;

        public GroupSpawnResult SpawnGroup(Transform target, Transform parent, Vector2 screenWidth, float startSpawnHeight)
        {
            
            var spawnedPlatforms = new List<Platform>();
            float lastSpawnedHeight = startSpawnHeight;

            for (int i = 0; i < _platformsGroup.Length; i++)
            {
                lastSpawnedHeight += Random.Range(_heightBounds.x, _heightBounds.y);

                var row = _platformsGroup[i];
                var spawnRow = row.SpawnRow(target, parent, screenWidth, lastSpawnedHeight);
                spawnedPlatforms.AddRange(spawnRow);
            }

            return new GroupSpawnResult(spawnedPlatforms, lastSpawnedHeight);
        }
        
        public struct GroupSpawnResult
        {
            public readonly List<Platform> SpawnedPlatforms;
            public readonly float LastSpawnedHeight;

            public GroupSpawnResult(List<Platform> spawnedPlatforms, float lastSpawnedHeight)
            {
                SpawnedPlatforms = spawnedPlatforms;
                LastSpawnedHeight = lastSpawnedHeight;
            }
        }
        [System.Serializable]
        public struct PlatformRow
        {
            public Platform[] Platforms;

            public List<Platform> SpawnRow(Transform target, Transform parent, Vector2 screenWidth, float spawnHeight)
            {
                var spawnedPlatforms = new List<Platform>();
                for (var i = 0; i < Platforms.Length; i++)
                {
                    var platformPositionX = Random.Range(screenWidth.x, screenWidth.y);
                    var platformPosition = new Vector3(platformPositionX, spawnHeight, parent.position.z);
                    var spawnedPlatform = Instantiate(Platforms[i], platformPosition, Quaternion.identity, parent);
                    spawnedPlatforms.Add(spawnedPlatform);
                }
                return spawnedPlatforms;
            }
        }
    }
}