using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseForcePush : MonoBehaviour
{
    private Rigidbody guy;
    // Start is called before the first frame update
    void Start()
    {
        guy = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(guy.position.z > 5) {
            guy.AddForce(new Vector3(0, 0, -5) * 100, ForceMode.Force);
        }   
    }
}
