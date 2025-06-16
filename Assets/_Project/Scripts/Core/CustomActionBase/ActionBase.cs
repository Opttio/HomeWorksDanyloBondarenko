using System;
using UnityEngine;

namespace _Project.Scripts.Core.CustomActionBase
{
    public abstract class ActionBase : MonoBehaviour
    {
        [SerializeField] protected bool _executeOnStart;
        [SerializeField] protected bool _executeOnce;
        protected bool _isExecutedOnce = false;
        private void Start()
        {
            if (_executeOnStart)
            {
                Execute();
            }
        }

        public void Execute()
        {
            if (_executeOnce && _isExecutedOnce)
            {
                return;
            }
            _isExecutedOnce = true;
            ExecuteInternal();
        }
        
        protected abstract void ExecuteInternal();
    }
}