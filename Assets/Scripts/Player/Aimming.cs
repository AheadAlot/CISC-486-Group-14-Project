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
        yAxis = Mathf.Clamp(yAxis, 10, 80);
    }

    private void LateUpdate()
    {
        // cameraFollowPos.localEulerAngles = new Vector3(yAxis, cameraFollowPos.localEulerAngles.y, cameraFollowPos.localEulerAngles.z);
        // transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
        transform.eulerAngles = new Vector3(0f, xAxis, 0f);

        Vector3 offset = new Vector3(0f, 0f, -Vector3.Distance(cameraFollowPos.transform.position, transform.position));
        Quaternion rotation = Quaternion.Euler(yAxis, xAxis, 0f);
        cameraFollowPos.position = transform.position + rotation * offset;
        cameraFollowPos.LookAt(transform.position + Vector3.up * 1.5f);
    }
    
}
