using _Project.Scripts.Runtime.Character;
using UnityEngine;

namespace _Project.Scripts.Environment.Platforms
{
    public class FadePlatform : MonoBehaviour
    {
        [SerializeField] private LayerMask _characterMask;
        [SerializeField] private MainCharacter _character;
        [SerializeField] private float _playerCheckDistance;
        [SerializeField] private GameObject[] _fadePlatforms;
        private bool _isChange = false;
        private int _counter = 1;

        private void OnEnable()
        {
            _character = GameObject.FindFirstObjectByType<MainCharacter>();
        }

        private void Update()
        {
            CanChangePlatform(out _isChange);
            if (_counter == _fadePlatforms.Length)
            {
                Destroy(this.gameObject);
            }
        }

        private void FixedUpdate()
        {
            if (_isChange)
            {
                ChangeFadePlatform(_counter);
            }
        }

        private void ChangeFadePlatform(int counter)
        {
            TirnOffAllFadePlatforms();
            _fadePlatforms[_counter].SetActive(true);
            _counter++;
        }

        private void CanChangePlatform(out bool _isChange)
        {
            Debug.DrawRay(transform.position, Vector2.up * _playerCheckDistance, Color.red);
            if (_character.PlayerRb.linearVelocity.y > 0) _isChange = false;
            else
            {
                _isChange = Physics2D.Raycast(transform.position, Vector2.up, _playerCheckDistance, _characterMask);
            }
        }

        private void TirnOffAllFadePlatforms()
        {
            foreach (var fadePlatform in _fadePlatforms) 
                fadePlatform.SetActive(false);
        }
    }
}
