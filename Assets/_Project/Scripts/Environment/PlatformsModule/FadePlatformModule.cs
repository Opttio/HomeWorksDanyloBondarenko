using System;
using _Project.Scripts.Core.CustomActionBase;
using UnityEngine;

namespace _Project.Scripts.Environment.PlatformsModule
{
    public class FadePlatformModule : ActionBase
    {
        [SerializeField] private GameObject[] _fadeObjectPull;
        private int _counter = 1;

        private void Update()
        {
            if (_counter == _fadeObjectPull.Length)
            {
                Destroy(this.gameObject);
            }
        }

        protected override void ExecuteInternal()
        {
            ChangeFadePlatform(_counter);
            Debug.Log("ChangeFadePlatform");
        }

        private void ChangeFadePlatform(int counter)
        {
            if (_fadeObjectPull.Length == 0)
            {
                Debug.LogError("FadeObjectPull is empty");
                return;
            }
            TirnOffAllFadePlatforms();
            _fadeObjectPull[counter].SetActive(true);
            _counter++;
        }

        private void TirnOffAllFadePlatforms()
        {
            foreach (var fadeObject in _fadeObjectPull) 
                fadeObject.SetActive(false);
        }
    }
}