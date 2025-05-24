using UnityEngine;

namespace _Project.Scripts.Runtime
{
    public class Person : MonoBehaviour
    {
        private string _personName;
        private int _personAge;

        public string PersonName { get; private set; }

        public int PersonAge
        {
            get => _personAge;
            set
            {
                if (value > 0 || value < 100)
                    _personAge = value;
                else
                    Debug.Log ("Age must be between 0 and 100");
            }
        }

        public virtual void ShowStat()
        {
            Debug.Log($"Person Name = {_personName}");
        }
        
    }
}
