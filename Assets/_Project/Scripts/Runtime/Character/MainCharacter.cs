using System;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Runtime.Character
{
    public class MainCharacter : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _playerRB;
        [SerializeField] private float _jumpPower;
        private bool _isGrounded = false;
        private bool _isDoubleJump = false;
        private bool _pushSpace = false;
        private bool _isBounce = false;
        [SerializeField] private float _groundCheckDistance = 0.2f;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _bouncePower;
        [SerializeField] private LayerMask _bounceLayer;
        
        public bool IsGrounded => _isGrounded;
        public Rigidbody2D PlayerRB => _playerRB;

        /*private void OnEnable()
        {
            _playerInput = GameObject.FindFirstObjectByType<PlayerInput>();
        }*/
        private void Update()
        {
            CanJump(out _isGrounded);
            PushSpace();
            CanBounce(out _isBounce);
        }

        private void FixedUpdate()
        {
            Move();
            if (_isBounce) Bounce();
            if (_isGrounded) Jump();
            
            if (_pushSpace)
            {
                if (_isDoubleJump)
                    DoubleJump();
            }
        }

        private void PushSpace()
        {
            if (_isDoubleJump == false)
                return;
            if (_playerInput.GetJumpButton())
                _pushSpace = true;
        }
        private void DoubleJump()
        {
            _playerRB.linearVelocity = new Vector2(_playerRB.linearVelocity.x, _jumpPower);
            _isDoubleJump = false;
            _pushSpace = false;
        }

        private void Move()
        {
            Vector2 inputVector = _playerInput.GetMoveInputVector();
            Vector3 moveDirection = new Vector3(inputVector.x, 0, 0);
            transform.position += moveDirection * (Time.fixedDeltaTime * _moveSpeed);
            if (inputVector.x > 0) transform.localScale = new Vector3(-1, 1, 1);
            else if (inputVector.x < 0) transform.localScale = new Vector3(1, 1, 1);
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
                _isGrounded = Physics2D.Raycast(_playerRB.position, Vector2.down, _groundCheckDistance, _groundLayer);
        }

        public void Bounce()
        {
            _playerRB.linearVelocity = new Vector2(_playerRB.linearVelocity.x, _bouncePower);
            _isBounce = false;
            _isDoubleJump = true;
            _pushSpace = false;
            _isGrounded = false;
        }

        private void CanBounce(out bool _isBounce)
        {
            if (_playerRB.linearVelocity.y > 0) _isBounce = false;
            else
                _isBounce = Physics2D.Raycast(_playerRB.position, Vector2.down, _groundCheckDistance, _bounceLayer);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                Destroy(this.gameObject);
            }
        }
        
    }
}
