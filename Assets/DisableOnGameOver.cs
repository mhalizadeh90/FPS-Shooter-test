using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnGameOver : MonoBehaviour
{
    void OnEnable()
    {
        PlayerHealth.OnPlayerDied += DisableObject;
    }

    void DisableObject()
    {
        gameObject.SetActive(false);
    }
    void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= DisableObject;
    }
}
