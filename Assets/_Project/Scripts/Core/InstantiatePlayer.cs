using _Project.Scripts.Runtime;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class InstantiatePlayer : MonoBehaviour
    {
        [SerializeField] private MainCharacter _mainCharacter;
        // [SerializeField] private PlayerInput _playerInput;
        private void OnEnable()
        {
            var player = Instantiate(_mainCharacter, transform.position, Quaternion.identity);
            // var playerInput = Instantiate(_playerInput, transform.position, Quaternion.identity);
            
        }

    }
}
