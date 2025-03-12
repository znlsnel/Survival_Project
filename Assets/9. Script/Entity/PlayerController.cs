using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector3 moveDir;
    void Start()
    {
        InputManager.Instance.Move.action.performed += MoveInput;
	}


    void Update()
    {
        transform.position += moveDir * Time.deltaTime * 3.0f;
    }

    void MoveInput(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        moveDir = new Vector3(dir.x, 0f, dir.y);
	}

}
