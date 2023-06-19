using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    private int health = 7;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(health);
        health--;
        if (health == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
