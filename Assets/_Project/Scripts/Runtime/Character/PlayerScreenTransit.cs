using UnityEngine;

namespace _Project.Scripts.Runtime.Character
{
    public class PlayerScreenTransit : MonoBehaviour
    {
        [SerializeField] private float _screenOffset = 0.5f;
        
        private float _screenLeftX;
        private float _screenRightX;

        private void Start()
        {
            DetermineTransformScreenSize();
        }

        private void Update()
        {
            TransitPlayer();
        }

        private void TransitPlayer()
        {
            Vector3 bufferPosition = transform.position;
            if (bufferPosition.x < _screenLeftX)
                bufferPosition.x = _screenRightX;
            else if (bufferPosition.x > _screenRightX)
                bufferPosition.x = _screenLeftX;
            transform.position = bufferPosition;
        }

        private void DetermineTransformScreenSize()
        {
            var camera = Camera.main;
            Vector3 leftPoint = camera.ViewportToWorldPoint(new Vector3(0, 0, 0));
            Vector3 rightPoint = camera.ViewportToWorldPoint(new Vector3(1, 0, 0));
            _screenLeftX = leftPoint.x - _screenOffset;
            _screenRightX = rightPoint.x + _screenOffset;
        }
    }
}