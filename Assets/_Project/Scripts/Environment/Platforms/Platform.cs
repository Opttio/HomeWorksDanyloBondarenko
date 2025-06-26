using System.Collections.Generic;
using UnityEngine;
using _Project.Scripts.Core.CustomActionBase;
using _Project.Scripts.Runtime.Character;

namespace _Project.Scripts.Environment.Platforms
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private List<ActionBase> _executeWhenPlayerTouch;
        [SerializeField] private float _characterCheckDistance;
        [SerializeField] private LayerMask _characterLayer;
        private bool _isChange = true;

        private void Update()
        {
            CanChange(out _isChange);
        }

        private void CanChange(out bool _isChange)
        {
            Vector3 leftPoint = new Vector3(transform.position.x - 0.17f, transform.position.y, transform.position.z);
            Vector3 rightPoint = new Vector3(transform.position.x + 0.17f, transform.position.y, transform.position.z);
            Debug.DrawRay(rightPoint, Vector2.up * _characterCheckDistance, Color.magenta);
            Debug.DrawRay(leftPoint, Vector2.up * _characterCheckDistance, Color.magenta);
            bool _isChangeLeft = Physics2D.Raycast(leftPoint, Vector2.up, _characterCheckDistance, _characterLayer);
            bool _isChangeRight = Physics2D.Raycast(rightPoint, Vector2.up, _characterCheckDistance, _characterLayer);
            _isChange = _isChangeLeft || _isChangeRight;
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent<MainCharacter>(out _))
            {
                if (_isChange)
                {
                    foreach (var actionBase in _executeWhenPlayerTouch)
                    {
                        actionBase.Execute();
                    }
                }
            }
        }
    }
}