using System;
using _Project.Scripts.Core.CustomActionBase;
using UnityEngine;

namespace _Project.Scripts.Environment.PlatformsModule
{
    public class FadePlatformModule : ActionBase
    {
        [SerializeField] private GameObject[] _fadeObjectPull;
        private int _counter;

        private void OnEnable()
        {
            _counter = 0;
            ChangeFadePlatform(_counter);
        }

        private void Update()
        {
            if (_counter >= _fadeObjectPull.Length)
            {
                gameObject.SetActive(false);
            }
        }

        protected override void ExecuteInternal()
        {
            if (_counter < _fadeObjectPull.Length) 
                ChangeFadePlatform(_counter);
        }

        private void ChangeFadePlatform(int counter)
        {
            if (_fadeObjectPull.Length == 0)
            {
                Debug.LogError("FadeObjectPull is empty");
                return;
            }
            TurnOffAllFadePlatforms();
            if (counter < _fadeObjectPull.Length)
            {
                _fadeObjectPull[counter].SetActive(true);
                _counter++;
            }
        }

        private void TurnOffAllFadePlatforms()
        {
            foreach (var fadeObject in _fadeObjectPull) 
                fadeObject.SetActive(false);
        }
    }
}