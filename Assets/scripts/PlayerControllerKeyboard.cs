using UnityEngine;

public class PlayerControllerKeyboard : MonoBehaviour
{
    public string pSingleShot = "SingleShot";
    public string pBurstShot = "BurstShot";
    public string pAutoShot = "AutoShot";
    public string pJump = "Jump";
    public string pDie = "Die";
    public string pIsFiring = "IsFiring";

    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    public float autoFireRate = 8f;

    private Animator anim;
    private CharacterController cc;

    private float nextAutoTime;
    private float velocityY = 0f;
    private bool isJumping = false;

    void Start()
    {
        // Get the CharacterController component from the character itself
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();

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
        HandleShooting();
        ApplyGravity();
        HandleJumpAndDie();
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
        bool isFiringNow = firedThisFrame || Input.GetMouseButton(0);
        anim.SetBool(pIsFiring, isFiringNow);
    }

    // Jump Dead
    // Speace k
    void HandleJumpAndDie()
    {
        if (cc.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;

            anim.ResetTrigger(pJump);
            anim.SetTrigger(pJump);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.ResetTrigger(pDie);
            anim.SetTrigger(pDie);
            anim.SetBool(pIsFiring, false);
        }
    }
    void ApplyGravity()
    {
        if (isJumping)
        {
            Vector3 jumpMotion = Vector3.up * velocityY * Time.deltaTime;
            cc.Move(jumpMotion);
            velocityY += gravity * Time.deltaTime;

            if (cc.isGrounded && velocityY < 0f)
            {
                isJumping = false;
                velocityY = 0f;
            }
        }
    }
}
