using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float mouseSensetivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;
    bool isGameFinished = false;

    void OnEnable()
    {
        AIWaveSpawner.OnAllAIsDied += EnableMouseCursor;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void EnableMouseCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        print("Set Mouse To Default");
    }

    void Update()
    {
        if (isGameFinished)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensetivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensetivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void OnDisable()
    {
        AIWaveSpawner.OnAllAIsDied -= EnableMouseCursor;
    }
}
