using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternativeShoulderSpin : MonoBehaviour
{
    private int counter = 0;
    private bool direction = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(counter > 100 || counter < -100 ) {
            direction = !direction;
        }
        if(direction) {
            transform.Rotate(Vector3.down, 0.5f);
            counter++;
        }
        else {
            transform.Rotate(Vector3.up, 0.5f);
            counter--;
        }
    }
}
