using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickForMovement : Joystick
{

    [SerializeField] private Controller playerController;

    private void Update()
    {
        if(_inputVector.x != 0 || _inputVector.y != 0)
        {
            playerController.MovePlayer(new Vector3(_inputVector.x, 0, _inputVector.y));
            playerController.RotatePlayer(new Vector3(_inputVector.x, 0, _inputVector.y));
        }
        else
        {
            playerController.MovePlayer(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
            playerController.RotatePlayer(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
        }
    }
}
