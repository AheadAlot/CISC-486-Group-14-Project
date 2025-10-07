using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerControllerKeyboard : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Normal run speed (m/s).")]
    public float moveSpeed = 10f;

    [Tooltip("Speed cap while moving AND shooting.")]
    public float shootMoveSpeed = 5f;

    [Tooltip("Jump apex height (meters).")]
    public float jumpHeight = 1.5f;

    [Tooltip("Gravity (negative value).")]
    public float gravity = -9.81f;

    [Tooltip("How quickly the character turns to face the move direction.")]
    public float rotationSmooth = 10f;

    [Header("Animator Parameter Names (must match your Animator)")]
    public string pSpeed = "speed";      // float
    public string pMoveX = "moveX";      // float (-1..1)
    public string pMoveZ = "moveZ";      // float (-1..1)
    public string pSingleShot = "SingleShot"; // trigger
    public string pBurstShot = "BurstShot";  // trigger
    public string pAutoShot = "AutoShot";   // trigger
    public string pJump = "Jump";       // trigger (Space)
    public string pDie = "Die";        // trigger (K)
    public string pIsFiring = "IsFiring";   // bool

    //Shooting timing
    [Header("Shooting")]
    [Tooltip("Duration used to keep slowdown window for single shot.")]
    public float singleShotExit = 0.35f;

    [Tooltip("Duration used to keep slowdown window for burst shot.")]
    public float burstShotExit = 0.60f;

    [Tooltip("Auto fire rate (shots per second) while holding LMB.")]
    public float autoFireRate = 8f;

    private CharacterController cc;
    private Animator anim;
    private Camera cam;
    private Vector3 velocity;       // Y handled by gravity/jump
    private float nextAutoTime;     // timer for auto fire cadence
    private float shootSlowUntil;   // slowdown window for single/burst

    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        cam = Camera.main;

        // Clear any latent triggers so we never auto transition on Play
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

    // Camera-aligned movement that continuously feeds
    // speed/moveX/moveZ to the Animator.
    // While moving & shooting, speed is clamped to shootMoveSpeed.
    void HandleMovement()
    {
        bool grounded = cc.isGrounded;

        // Small downward bias to keep the controller grounded
        if (grounded && velocity.y < 0f) velocity.y = -2f;

        // WASD input (-1..1)
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Normalize so diagonals aren’t faster
        Vector3 input = new Vector3(h, 0f, v);
        if (input.sqrMagnitude > 1f) input.Normalize();

        // Camera-aligned axes
        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;
        if (cam)
        {
            forward = Vector3.Scale(cam.transform.forward, new Vector3(1, 0, 1)).normalized;
            right = cam.transform.right;
        }

        // Desired world move direction
        Vector3 moveDir = forward * input.z + right * input.x;

        // Slowdown window: true during single/burst clip,
        // or while holding LMB for auto fire
        bool slowForShot = Time.time < shootSlowUntil || Input.GetMouseButton(0);
        float maxSpeed = (slowForShot && input.sqrMagnitude > 0.001f) ? shootMoveSpeed : moveSpeed;

        float targetSpeed = input.sqrMagnitude > 0.001f ? maxSpeed : 0f;

        // Translate
        if (targetSpeed > 0f)
        {
            cc.Move(moveDir.normalized * targetSpeed * Time.deltaTime);

            // Rotate to face movement direction
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSmooth * Time.deltaTime);
        }

        // Gravity & vertical motion
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        // Feed Animator every frame
        anim.SetFloat(pSpeed, targetSpeed);
        anim.SetFloat(pMoveX, input.x);
        anim.SetFloat(pMoveZ, input.z);

        // Jump (key-driven only)
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            anim.ResetTrigger(pJump);
            anim.SetTrigger(pJump);
        }
    }

    
    // Shooting:
    //  - Single (LMB click)
    //  - Burst  (RMB click)
    //  - Auto   (hold LMB) with cadence limit
    // Maintains IsFiring so standing shots can be interrupted
    // into Walk*Shoot when you start moving.
    void HandleShooting()
    {
        bool firedThisFrame = false;

        // Single shot (LMB click)
        if (Input.GetMouseButtonDown(0))
        {
            anim.ResetTrigger(pSingleShot);
            anim.SetTrigger(pSingleShot);
            firedThisFrame = true;

            // If we’re moving, keep slow speed for the clip duration
            if (anim.GetFloat(pSpeed) > 0.1f)
                shootSlowUntil = Time.time + singleShotExit;
        }

        // Burst (RMB click)
        if (Input.GetMouseButtonDown(1))
        {
            anim.ResetTrigger(pBurstShot);
            anim.SetTrigger(pBurstShot);
            firedThisFrame = true;

            if (anim.GetFloat(pSpeed) > 0.1f)
                shootSlowUntil = Time.time + burstShotExit;
        }

        // Auto fire (hold LMB)
        if (Input.GetMouseButton(0) && Time.time >= nextAutoTime)
        {
            anim.ResetTrigger(pAutoShot);
            anim.SetTrigger(pAutoShot);
            nextAutoTime = Time.time + 1f / Mathf.Max(1f, autoFireRate);
            firedThisFrame = true;
        }

        // Consider "IsFiring" true if:
        // - fired this frame, OR
        // - holding LMB (auto), OR
        // - still within the slowdown window for single/burst
        bool isFiringNow = firedThisFrame || Input.GetMouseButton(0) || (Time.time < shootSlowUntil);
        anim.SetBool(pIsFiring, isFiringNow);
    }

    // Die
    void HandleJumpAndDie()
    {
        // Die on K
        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.ResetTrigger(pDie);
            anim.SetTrigger(pDie);
            anim.SetBool(pIsFiring, false); // optional: stop firing state on death
        }
    }
}
