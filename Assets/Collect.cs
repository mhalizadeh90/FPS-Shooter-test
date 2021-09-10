using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        //TODO: USE COLLECTIBLE
        collision.gameObject.GetComponent<ICollectable>()?.Use();
        print("Use");
    }

}
