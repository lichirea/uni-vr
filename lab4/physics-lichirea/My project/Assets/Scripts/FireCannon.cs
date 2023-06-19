using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCannon : MonoBehaviour
{

    public float startingX;
    public float startingY;
    public float startingZ;

    private Rigidbody cannonball;
    private GameObject bell;

    // Start is called before the first frame update
    void Start()
    {
        cannonball = GetComponent<Rigidbody>();
        bell = GameObject.Find("BellBody");
    }

    // Update is called once per frame
    void Update()
    {
        var direction = Vector3.zero;
        if (Input.GetKeyDown("space"))
        {
            cannonball.position = new Vector3(startingX, startingY, startingZ);
            var velocity = (bell.transform.position - cannonball.position) * 3.5f;
            cannonball.velocity = velocity;
        }
    }
}
