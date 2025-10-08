using UnityEngine;

public class MouseSteerMove : MonoBehaviour
{
    [SerializeField] float mouseSense = 2f;
    [SerializeField] float minPitch = -70f;
    [SerializeField] float maxPitch = 70f;

    [SerializeField] Transform cameraFollowPos;
    CharacterController cc;

    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float gravity = -20f;

    float pitch;                                     
    float velY;                                     

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        if (!cameraFollowPos) Debug.LogWarning("[MouseSteerMove] cameraFollowPos ");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mx = Input.GetAxisRaw("Mouse X") * mouseSense;
        float my = Input.GetAxisRaw("Mouse Y") * mouseSense;

        transform.Rotate(0f, mx, 0f, Space.Self);

        pitch = Mathf.Clamp(pitch - my, minPitch, maxPitch);
        if (cameraFollowPos)
            cameraFollowPos.localRotation = Quaternion.Euler(pitch, 0f, 0f);


        if (cc.isGrounded && velY < 0f) velY = -2f;
        velY += gravity * Time.deltaTime;

        Vector3 motion = moveDir * moveSpeed + Vector3.up * velY;
        cc.Move(motion * Time.deltaTime);
    }
}
