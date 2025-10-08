using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Aimming : MonoBehaviour
{
    [SerializeField] float mouseSense = 3f;
    float yAxis;
    
    [SerializeField] Transform cameraFollowPos;

    Vector2 prevMousePos;

    void Start()
    {
        // Record initial position
        prevMousePos = Input.mousePosition;
    }

    void Update()
    {
        Vector2 cur = Input.mousePosition;
        Vector2 delta = cur - prevMousePos;
        prevMousePos = cur;

        yAxis += delta.x * mouseSense;

        // Limit the camera
        // only looks left and right
        if (cameraFollowPos != null)
            cameraFollowPos.localRotation = Quaternion.Euler(0f, yAxis, 0f);
   
    }

    
}