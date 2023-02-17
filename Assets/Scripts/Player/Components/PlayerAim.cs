using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private float verticalRotationLimit = 80f;
    private float verticalRotation;
    private Transform _mainCamera;
    
    void Awake()
    {
        _mainCamera = Camera.main.transform;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        verticalRotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerLook();
    }


    private void CheckPlayerLook()
    {
        if (_mainCamera == null) return;
        if (ManagerPause.IsPaused) return;

        float horizontalRotation = Input.GetAxis("Mouse X");
        transform.Rotate(0, horizontalRotation, 0);
        verticalRotation -= Input.GetAxis("Mouse Y");
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
        _mainCamera.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
