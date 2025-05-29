using System;
using UnityEngine;

namespace _Project.Scripts.Runtime
{
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        private bool _isJump = false;
        [SerializeField] private float _jumpForce;
        private bool _isGrounded = false;
        [SerializeField] private float _groundCheckDistance;
        [SerializeField] private LayerMask _groundLayer;

        private void Update()
        {
            CalculateJump();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Jump in Update");
            }
        }

        private void FixedUpdate()
        {
            if (_isGrounded)
            {
                if (_isJump)
                {
                    _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                    _isJump = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Jump in FixUpdate");
            }
        }

        private void CalculateJump()
        {
            _isGrounded = Physics2D.Raycast(_rigidbody2D.position, Vector2.down, _groundCheckDistance, _groundLayer);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isJump = true;
            }
        }
    }
}
