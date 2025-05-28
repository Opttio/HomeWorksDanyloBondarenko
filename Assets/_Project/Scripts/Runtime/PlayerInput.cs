using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Runtime
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
            /*Vector2 inputVector = Vector2.zero;
            if (Input.GetKey(KeyCode.A))
                inputVector.x = -1;
            if (Input.GetKey(KeyCode.D))
                inputVector.x = 1;
            inputVector = inputVector.normalized;
            return inputVector;*/
        }

        
        //Якось я дійшов до того як змусити PlayerInput контролювати і передавати Player чи натис я пробіл.
        public bool GetJumpButton()
        {
            if (inputSystemActions.Player.Jump.triggered) return true;
            else return false;
        }
    }
}
