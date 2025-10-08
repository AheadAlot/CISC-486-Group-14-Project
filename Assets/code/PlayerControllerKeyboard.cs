using UnityEngine;

public class PlayerControllerKeyboard : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float shootMoveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float rotationSmooth = 10f;

    public string pSpeed = "speed";
    public string pMoveX = "moveX";
    public string pMoveZ = "moveZ";
    public string pSingleShot = "SingleShot";
    public string pBurstShot = "BurstShot";
    public string pAutoShot = "AutoShot";
    public string pJump = "Jump";
    public string pDie = "Die";
    public string pIsFiring = "IsFiring";

    public float singleShotExit = 0.35f;
    public float burstShotExit = 0.60f;
    public float autoFireRate = 8f;

    private CharacterController cc;
    private Animator anim;
    private Vector3 velocity;
    private float nextAutoTime;
    private float shootSlowUntil;

    void Start()
    {
        // Get the CharacterController component from the character itself
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        // Clear all triggers
        anim.ResetTrigger(pSingleShot);
        anim.ResetTrigger(pBurstShot);
        anim.ResetTrigger(pAutoShot);
        anim.ResetTrigger(pJump);
        anim.ResetTrigger(pDie);
        anim.SetBool(pIsFiring, false);
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
        HandleJumpAndDie();
    }

    void HandleMovement()
    {
        bool grounded = cc.isGrounded;
        if (grounded && velocity.y < 0f) velocity.y = -2f;

        // input
        // w/a/s/d or ↑/↓/←/→
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0f, v);
        if (input.sqrMagnitude > 1f) input.Normalize();

        // Calculate player coordinates
        Vector3 moveDir = transform.forward * input.z + transform.right * input.x;

        // Slows down while shooting
        bool slowForShot = Time.time < shootSlowUntil || Input.GetMouseButton(0);
        float maxSpeed = (slowForShot && input.sqrMagnitude > 0.001f) ? shootMoveSpeed : moveSpeed;
        float targetSpeed = input.sqrMagnitude > 0.001f ? maxSpeed : 0f;

        if (targetSpeed > 0f && moveDir.sqrMagnitude > 0.0001f)
        {
            // Make the character move in a certain direction
            cc.Move(moveDir.normalized * targetSpeed * Time.deltaTime);

            // The direction the character is moving towards
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSmooth * Time.deltaTime);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        // Animator Parameters
        anim.SetFloat(pSpeed, targetSpeed);
        anim.SetFloat(pMoveX, input.x);
        anim.SetFloat(pMoveZ, input.z);

        // Jump
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.ResetTrigger(pJump);
            anim.SetTrigger(pJump);
        }
    }

    // Shooting
    void HandleShooting()
    {
        bool firedThisFrame = false;

        // SingleShoot
        if (Input.GetMouseButtonDown(0))
        {
            anim.ResetTrigger(pSingleShot);
            anim.SetTrigger(pSingleShot);
            firedThisFrame = true;
        }

        // Burstshoot
        if (Input.GetMouseButtonDown(1))
        {
            anim.ResetTrigger(pBurstShot);
            anim.SetTrigger(pBurstShot);
            firedThisFrame = true;
        }

        // AutoShoot
        if (Input.GetMouseButton(0) && Time.time >= nextAutoTime)
        {
            anim.ResetTrigger(pAutoShot);
            anim.SetTrigger(pAutoShot);
            nextAutoTime = Time.time + 1f / Mathf.Max(1f, autoFireRate);
            firedThisFrame = true;
        }

        // IsFiring
        bool isFiringNow = firedThisFrame || Input.GetMouseButton(0) || (Time.time < shootSlowUntil);
        anim.SetBool(pIsFiring, isFiringNow);
    }

    // Dead
    // k
    void HandleJumpAndDie()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.ResetTrigger(pDie);
            anim.SetTrigger(pDie);
            anim.SetBool(pIsFiring, false);
        }
    }
}
