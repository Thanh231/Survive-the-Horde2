using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Vector2 moveDir;

    public void OnPlayerGetInput(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>();
    }

    public Vector2 GetDirection()
    {
        return moveDir;
    }
}   

