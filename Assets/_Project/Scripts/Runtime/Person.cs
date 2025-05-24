using UnityEngine;

namespace _Project.Scripts.Runtime
{
    public abstract class Person : MonoBehaviour
    {
        private string _personName = "Bobby";
        private int _personAge;

        public string PersonName
        {
            get => _personName;
            private set => _personName = value;
        }

        public int PersonAge
        {
            get => _personAge;
            set
            {
                if (value > 0 && value < 100)
                    _personAge = value;
                else
                    Debug.Log ("Age must be between 0 and 100");
            }
        }

        public virtual void ShowStat()
        {
            Debug.Log($"Person Name = {PersonName}");
        }

        public abstract void TakeDamage(int damageValue);
    }
}
