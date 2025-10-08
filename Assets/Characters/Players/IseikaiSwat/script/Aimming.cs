using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Aimming : MonoBehaviour
{

    
    [SerializeField] float mouseSense = 1;
    float xAxis, yAxis;

    [SerializeField] Transform cameraFollowPos;
    void Start()
    {

    }

    void Update()
    {
        xAxis += Input.GetAxisRaw("Mouse X") * mouseSense;
        yAxis -= Input.GetAxisRaw("Mouse Y") * mouseSense;
        yAxis = Mathf.Clamp(yAxis, -80, 80);
    }

    private void LateUpdate()
    {
        cameraFollowPos.localEulerAngles = new Vector3(yAxis, cameraFollowPos.localEulerAngles.y, cameraFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }
    
}
