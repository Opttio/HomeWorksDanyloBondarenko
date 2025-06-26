using _Project.Scripts.Core.CustomActionBase;
using UnityEngine;

namespace _Project.Scripts.Environment.PlatformsModule
{
    public class ObjOnPlatformsSpawnModule : ActionBase
    {
        [SerializeField] private GameObject[] _objOnPlatforms;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _spawnChance = 0.4f;
        [SerializeField] private float _spawnAttemptChance = 0.8f;
        protected override void ExecuteInternal()
        {
            SpawnRandomObjFromPull();
        }

        private void SpawnRandomObjFromPull()
        {
            if (_objOnPlatforms == null || _objOnPlatforms.Length == 0)
                return;
            
            if (Random.value > _spawnChance)
                return;
            var randomObj = _objOnPlatforms[Random.Range(0, _objOnPlatforms.Length)];
            if (randomObj.tag == "Enemy")
            {
                Vector3 randomSpawnPosition = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z);
                Instantiate(randomObj, randomSpawnPosition, Quaternion.identity, _spawnPoint);
                return;
            }

            if (randomObj.tag == "Attempt" && Random.value > _spawnAttemptChance)
            {
                Instantiate(randomObj, _spawnPoint.position, Quaternion.identity, _spawnPoint);
                return;
            }

            Instantiate(randomObj, _spawnPoint.position, Quaternion.identity, _spawnPoint);
        }
    }
}