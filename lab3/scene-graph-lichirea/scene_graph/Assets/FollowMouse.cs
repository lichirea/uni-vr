using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FollowMouse : MonoBehaviour
{
    private Camera _camera;

    // Use this for initialization
    private void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        var mouse = Input.mousePosition;
        var fieldOfView = _camera.fieldOfView;
        var numerator = (float)Math.Sin(Math.PI / 180 * (90 - fieldOfView / 16));
        var denominator = (float)Math.Sin(Math.PI / 180 * (fieldOfView / 16));
        mouse.z = 0.5f * numerator / denominator;
        var world = _camera.ScreenToWorldPoint(mouse);
        transform.LookAt(world);
    }
}
