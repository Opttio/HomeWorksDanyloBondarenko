using UnityEngine;

namespace _Project.Scripts.Environment.Data
{
    [CreateAssetMenu(fileName = "MyPlatform", menuName = "Environment/MyPlatformsData", order = 0)]
    public class MyPlatformsData : ScriptableObject
    {
        public GameObject PlatformPrefab;
        public int Chance;
    }
}