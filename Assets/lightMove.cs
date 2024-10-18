using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class lightMove : MonoBehaviour
{
    Vector2 moveInput;

    // Update is called once per frame
    void Update()
    {
        moveInput = moveInput.normalized;
        transform.Translate(moveInput.x* Vector3.right * Time.deltaTime*5);
        transform.Translate(moveInput.y * Vector3.forward * Time.deltaTime * 5);

    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
