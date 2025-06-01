using UnityEngine;
using UnityEngine.InputSystem.HID;

namespace _Project.Scripts.Runtime
{
    public class MainCharacter : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _playerRB;
        [SerializeField] private float _jumpPower;
        private bool _isGrounded = false;
        private bool _isDoubleJump = false;
        private bool _pushSpace = false;
        [SerializeField] private float _groundCheckDistance = 0.2f;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private float _moveSpeed;
        
        public bool IsGrounded => _isGrounded;
        public Rigidbody2D PlayerRB => _playerRB;

        private void Update()
        {
            CanJump(out _isGrounded);
            PushSpace();
            
            //Тут я спочатку не розумів як зробити так, щоб мій PlayerInput контролював чи натискаю я пробіл, тому робив прямим зчитуванням.
            // if (_isDoubleJump && Input.GetKeyDown(KeyCode.Space)) DoubleJump();
            
            // if (_isDoubleJump && _playerInput.GetJumpButton()) DoubleJump();
            //Треба ставити DoubleJump в Update бо в FixedUpdate воно спрацьовує через раз, а не в той момент коли я натис кнопку.
        }

        private void FixedUpdate()
        {
            Move();
            if (_isGrounded) Jump();
            if (_pushSpace)
            {
                if (_isDoubleJump)
                    DoubleJump();
            }
        }

        private void PushSpace()
        {
            if (_playerInput.GetJumpButton())
                _pushSpace = true;
        }
        private void DoubleJump()
        {
            _playerRB.linearVelocity = new Vector2(_playerRB.linearVelocity.x, _jumpPower);
            // _playerRB.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            _isDoubleJump = false;
            _pushSpace = false;
        }

        private void Move()
        {
            Vector2 inputVector = _playerInput.GetMoveInputVector();
            Vector3 moveDirection = new Vector3(inputVector.x, 0, 0);
            transform.position += moveDirection * (Time.deltaTime * _moveSpeed);
        }
        public void Jump()
        {
            // _playerRB.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            _playerRB.linearVelocity = new Vector2(_playerRB.linearVelocity.x, _jumpPower);
            _isGrounded = false;
            _isDoubleJump = true;
        }
    
        private void CanJump(out bool _isGrounded)
        {
            Debug.DrawRay(_playerRB.position, Vector2.down * _groundCheckDistance, Color.red);
            
            if (_playerRB.linearVelocity.y > 0) _isGrounded = false;
            else 
            {
                _isGrounded = Physics2D.Raycast(_playerRB.position, Vector2.down, _groundCheckDistance,
                    _groundLayer);
            }
        }
    }
}
