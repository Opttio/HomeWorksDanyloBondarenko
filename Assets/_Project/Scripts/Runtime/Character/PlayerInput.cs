using UnityEngine;

namespace _Project.Scripts.Runtime.Character
{
    public class PlayerInput : MonoBehaviour
    {
        private InputSystem_Actions inputSystemActions;

        private void Awake()
        {
            inputSystemActions = new InputSystem_Actions();
            inputSystemActions.Player.Enable();
        }

        public Vector2 GetMoveInputVector()
        {
            Vector2 inputVector = inputSystemActions.Player.Move.ReadValue<Vector2>();
            return inputVector;
        }
        
        public bool GetJumpButton()
        {
            if (inputSystemActions.Player.Jump.triggered) return true;
            else return false;
        }
    }
}
