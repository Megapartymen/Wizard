using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    private float _moveMouseX;
    private float _moveMouseY;
    [SerializeField] private float _mouseSensetivity;

    private void Update()
    {
        _moveMouseX += Input.GetAxis("Mouse X") * _mouseSensetivity;
        _moveMouseY -= Input.GetAxis("Mouse Y") * _mouseSensetivity;

        transform.rotation = Quaternion.Euler(_moveMouseY, _moveMouseX, 0);
    }
}
