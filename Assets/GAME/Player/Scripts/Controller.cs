using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class Controller : MonoBehaviour
{
    [Header("Character movement stats")]
    public float moveSpeed;
    public float rotateSpeed;

    [Header("Gravity handling")]
    private float _currentAttractionCharacter = 0;
    public float gravityForce = 20;

    private CharacterController _characterController;


    void Start()
    {
        _characterController = GetComponent <CharacterController>();
    }

    void Update()
    {
        GravityHandling();
    }


    public void MovePlayer(Vector3 moveDirection)
    {
        moveDirection = moveDirection * moveSpeed;
        moveDirection.y = _currentAttractionCharacter;
        _characterController.Move(moveDirection * Time.deltaTime);

    }

    public void RotatePlayer(Vector3 moveDirection)
    {
        if (_characterController.isGrounded)
        {
            if (Vector3.Angle(transform.forward, moveDirection) > 0)
            {
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, moveDirection, rotateSpeed, 0);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }


    private void GravityHandling()
    {
        if (!_characterController.isGrounded)
        {
            _currentAttractionCharacter -= gravityForce * Time.deltaTime;
        }
        else
        {
            _currentAttractionCharacter = 0;
        }
    }
}
