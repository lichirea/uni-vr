using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellRing : MonoBehaviour
{
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    
    private void OnCollisionEnter()
    {
        source.Play();
    }
}
