using _Project.Scripts.Core.EventBus;
using _Project.Scripts.Runtime.Controllers;
using _Project.Scripts.UI.Model;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Runtime.Character
{
    public class MainCharacter : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _playerRb;
        [SerializeField] private float _jumpPower;
        [SerializeField] private float _groundCheckDistance = 0.2f;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _bouncePower;
        [SerializeField] private LayerMask _bounceLayer;
        [SerializeField] private GameObject _startPlatforms;
        [Header("Audio")]
        [SerializeField] private AudioClip _jumpClip;
        [SerializeField] private AudioClip _doubleJumpClip;
        [SerializeField] private AudioClip _bounceClip;
        [SerializeField] private AudioClip _attemptPickUpClip;
        private bool _isGrounded = false;
        private bool _isDoubleJump = false;
        private bool _pushSpace = false;
        private bool _isBounce = false;
        private Vector2 _lastPosition;

        public bool IsGrounded => _isGrounded;
        public Rigidbody2D PlayerRb => _playerRb;

        private async void Start()
        {
            Time.timeScale = 0f;
            await UniTask.WaitUntil(() => CharacterModel.IsLoaded);
            InsertInPosition();
        }

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void Update()
        {
            CanJump(out _isGrounded);
            PushSpace();
            CanBounce(out _isBounce);
            SignalWhenDistanceIsChange();
            SignalWhenPositionIsChanged();
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

        private void OnDisable()
        {
            UnSubscribeToEvents();
        }

        private void InsertInPosition()
        {
            var pos = CharacterModel.Position;
            transform.position = new Vector3(pos.x, pos.y, transform.position.z);
            var startPlatformPosition = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
            Instantiate(_startPlatforms, startPlatformPosition, Quaternion.identity);
            Debug.Log($"Позицію гравця завантажено у {pos.x} та {pos.y}");
        }

        private void Move()
        {
            Vector2 inputVector = _playerInput.GetMoveInputVector();
            Vector3 moveDirection = new Vector3(inputVector.x, 0, 0);
            transform.position += moveDirection * (Time.fixedDeltaTime * _moveSpeed);
            if (inputVector.x > 0) transform.localScale = new Vector3(-1, 1, 1);
            else if (inputVector.x < 0) transform.localScale = new Vector3(1, 1, 1);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                ViewModel.ViewId = 2;
                Time.timeScale = 0f;
                GameUIBus.ChangeViewId(ViewModel.ViewId);
                Destroy(other.gameObject);
            }

            if (other.CompareTag("Attempt"))
            {
                SignalWhenAttemptIsChange();
                Destroy(other.gameObject);
                AudioController.Instance.PlaySfx(_attemptPickUpClip);
            }

            if (other.CompareTag("BottomEnds"))
            {
                Jump();
                ViewModel.ViewId = 2;
                Time.timeScale = 0f;
                GameUIBus.ChangeViewId(ViewModel.ViewId);
            }
        }

        #region Double Jump

        private void PushSpace()
        {
            if (_isDoubleJump == false)
                return;
            if (_playerInput.GetJumpButton())
                _pushSpace = true;
        }

        private void DoubleJump()
        {
            _playerRb.linearVelocity = new Vector2(_playerRb.linearVelocity.x, _jumpPower);
            _isDoubleJump = false;
            _pushSpace = false;
            AudioController.Instance.PlaySfx(_doubleJumpClip, Random.Range(0.8f, 1.2f));
        }

        #endregion

        #region Jump

        public void Jump()
        {
            _playerRb.linearVelocity = new Vector2(_playerRb.linearVelocity.x, _jumpPower);
            _isGrounded = false;
            _isDoubleJump = true;
            AudioController.Instance.PlaySfx(_jumpClip, Random.Range(0.8f, 1.2f));
        }

        private void CanJump(out bool isGrounded)
        {
            Debug.DrawRay(_playerRb.position, Vector2.down * _groundCheckDistance, Color.red);
            
            if (_playerRb.linearVelocity.y > 0) isGrounded = false;
            else
                isGrounded = Physics2D.Raycast(_playerRb.position, Vector2.down, _groundCheckDistance, _groundLayer);
        }

        #endregion

        #region Bounce

        public void Bounce()
        {
            _playerRb.linearVelocity = new Vector2(_playerRb.linearVelocity.x, _bouncePower);
            _isBounce = false;
            _isDoubleJump = true;
            _pushSpace = false;
            _isGrounded = false;
            AudioController.Instance.PlaySfx(_bounceClip);
        }

        private void CanBounce(out bool isBounce)
        {
            if (_playerRb.linearVelocity.y > 0) isBounce = false;
            else
                isBounce = Physics2D.Raycast(_playerRb.position, Vector2.down, _groundCheckDistance, _bounceLayer);
        }

        #endregion

        private int CalculateDistance(float currentDistance, int characterDistance)
        {
            if (characterDistance < currentDistance)
                return (int) currentDistance;
            return characterDistance;
        }

        private void SignalWhenDistanceIsChange()
        {
            CharacterModel.Distance = CalculateDistance(transform.position.y, CharacterModel.Distance);
            GameEventBus.ChangeDistance(CharacterModel.Distance);
        }

        private void SignalWhenPositionIsChanged()
        {
            Vector2 current = transform.position;
            if (current != _lastPosition)
            {
                _lastPosition = current;
                CharacterModel.Position = current;
                GameEventBus.ChangePosition(current);
            }
        }

        private void SignalWhenAttemptIsChange()
        {
            if (CharacterModel.Attempt >= 99)
                return;
            CharacterModel.Attempt++;
            GameEventBus.ChangeAttempt(CharacterModel.Attempt);
        }

        private void SubscribeToEvents()
        {
            GameEventBus.OnGameSpeed += OnGameSpeed;
        }

        private void UnSubscribeToEvents()
        {
            GameEventBus.OnGameSpeed -= OnGameSpeed;
        }

        private void OnGameSpeed(float speed)
        {
            Time.timeScale = speed;
        }
    }
}
