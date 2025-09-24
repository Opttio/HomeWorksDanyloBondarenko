using _Project.Scripts.Core.Extensions;
using UnityEngine;

namespace _Project.Scripts.Runtime.Character
{
    public class PlayerScreenTransit : MonoBehaviour
    {
        [SerializeField] private float _screenOffset = 0.5f;

        private void Update()
        {
            TransitPlayer();
        }

        private void TransitPlayer()
        {
            Vector3 bufferPosition = transform.position;
            var screenBounds = Camera.main.GetScreenHorizontalBounds(_screenOffset);
            if (bufferPosition.x < screenBounds.Left)
                bufferPosition.x = screenBounds.Right;
            else if (bufferPosition.x > screenBounds.Right)
                bufferPosition.x = screenBounds.Left;
            transform.position = bufferPosition;
        }
    }
}