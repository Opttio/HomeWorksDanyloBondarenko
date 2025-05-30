using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Runtime
{
    public class BackgroundChanger : MonoBehaviour
    {
        [SerializeField] private GameObject[] _backgrounds;
        [SerializeField] private Player _player;

        private void Update()
        {
            if (_player.IsGrounded)
            {
                OffAllBackgrounds();
                SetBackground(RandomInt());
            }
        }

        public int RandomInt()
        {
            return Random.Range(0, _backgrounds.Length);
        }

        public void SetBackground(int i)
        {
            _backgrounds[i].SetActive(true);
        }

        public void OffAllBackgrounds()
        {
            foreach (var item in _backgrounds)
            {
                item.SetActive(false);
            }
        }
    }
}
