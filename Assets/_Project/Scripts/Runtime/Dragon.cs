using UnityEngine;

namespace _Project.Scripts.Runtime
{
    public class Dragon : Person
    {
        private void Start()
        {
            TakeDamage(100);
        }
        
        public override void TakeDamage(int damageValue)
        {
            Debug.Log($"I am the mighty dragon, have lost {damageValue} hit points.");
        }
    }
}
