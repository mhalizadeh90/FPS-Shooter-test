using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseViewInput : MonoBehaviour
{
    [SerializeField] float mouseSensetivity = 100f;
    [SerializeField] Transform playerBody;
    [SerializeField] Transform Camera;
    float xRotation = 0f;

    void OnEnable()
    {
        AIWaveSpawner.OnAllAIsDied += EnableMouseCursor;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void EnableMouseCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensetivity * Time.deltaTime;
       
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensetivity * Time.deltaTime;
        xRotation -= mouseY;    // in order not to have inverted mouse look
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        Camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }



    void OnDisable()
    {
        AIWaveSpawner.OnAllAIsDied -= EnableMouseCursor;
    }
}
