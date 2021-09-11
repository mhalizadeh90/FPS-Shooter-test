using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableThisObjectOnGameOver : MonoBehaviour
{
    void OnEnable()
    {
        PlayerHealth.OnPlayerDied += DisableGameObject;
    }

    void DisableGameObject()
    {
        gameObject.SetActive(false);
    }


    void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= DisableGameObject;
    }
}
