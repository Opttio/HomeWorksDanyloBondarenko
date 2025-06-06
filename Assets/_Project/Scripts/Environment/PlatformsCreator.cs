using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlatformsCreator : MonoBehaviour
{
    [Header("Global Settings:")]
    [Space]
    [SerializeField] private Transform _target;
    [FormerlySerializedAs("_platformPrefab")]
    [Space]
    [Header("Spawn Settings:")]
    [Space]
    [SerializeField] private GameObject _platformPrefabs;
    [SerializeField] private int _stepsCountToSpawn;
    [SerializeField] private float _stepsCountToDelete;
    [SerializeField] private float _stepHeight;
    [SerializeField] private Vector2 _bounds;
    private Queue<GameObject> _spawnedPlatforms;
    private float _lastPlatformsSpawnedOnPlayerPosition;
    private float _lastPlatformsDeletedOnPlayerPosition;
    private void Awake()
    {
        _spawnedPlatforms = new Queue<GameObject>();
        _lastPlatformsDeletedOnPlayerPosition = _lastPlatformsSpawnedOnPlayerPosition = _target.position.y;
        for (int i = 0; i < _stepsCountToSpawn; i++)
        {
            SpawnPlatform(i + 1);
        }
    }
    private void Update()
    {
        if (_target.position.y - _lastPlatformsSpawnedOnPlayerPosition > _stepHeight)
        {
            SpawnPlatform(_stepsCountToSpawn);
            _lastPlatformsSpawnedOnPlayerPosition += _stepHeight;
        }
        if (_target.position.y - _lastPlatformsDeletedOnPlayerPosition > _stepHeight * _stepsCountToDelete)
        {
            var platformToDelete = _spawnedPlatforms.Dequeue();
            if (platformToDelete && platformToDelete.gameObject)
            {
                Destroy(platformToDelete.gameObject);
            }
            _lastPlatformsDeletedOnPlayerPosition += _stepHeight;
        }
    }
    private void SpawnPlatform(int stepsCount)
    {
        var platformPositionX = Random.Range(_bounds.x, _bounds.y);
        var platformPositionY = _target.position.y + stepsCount * _stepHeight;
        Vector3 platformPosition = new Vector3(platformPositionX, platformPositionY, transform.position.z);
        GameObject spawnedPlatform = Instantiate(_platformPrefabs, platformPosition, Quaternion.identity, this.transform);
        _spawnedPlatforms.Enqueue(spawnedPlatform);
    }
}