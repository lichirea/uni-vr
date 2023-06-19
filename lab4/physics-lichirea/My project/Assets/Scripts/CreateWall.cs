using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 144;

        var block = Resources.Load("Block") as GameObject;
        for (var i = -5; i < 5; i++)
        {
            for (var j = 0; j < 10; j++)
            {
                var instantiatedBlock = Instantiate(block);
                var transformPosition = transform.position;
                instantiatedBlock.transform.position = new Vector3(transformPosition.x + i * 1.05f, transformPosition.y + j * 1.05f,
                    transformPosition.z * 1.05f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
