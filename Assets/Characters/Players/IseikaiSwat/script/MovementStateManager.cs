using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    public float currentSpeed;
    public float WalkSpeed = 3, WalkBackSpeed = 2;
    public float RunSpeed = 7, RunBackSpeed = 5;
    public float CrouchSpeed = 2, CrouchBackSpeed = 1;

    [HideInInspector] public Vector3 dir;
    [HideInInspector] public float hzInput, vtInput;
    [HideInInspector] public Animator anime;
    CharacterController controller;

    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundMask;
    Vector3 spherePos;
    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;

    MovementBaseState currentState;
    public idleState idle = new idleState();
    public walkstate walk = new walkstate();
    public runstate run = new runstate();
    public crouchstate crouch = new crouchstate();

    void Start()
    {
        anime = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        SwitchState(idle);
    }

    public void SwitchState(MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void Update()
    {
        GetDirAndMove();
        Gravity();

        anime.SetFloat("hzInput", hzInput);
        anime.SetFloat("vtInput", vtInput);

        currentState.UpdateState(this);
    }

    void GetDirAndMove()
    {
        hzInput = Input.GetAxisRaw("Horizontal");
        vtInput = Input.GetAxisRaw("Vertical");
        dir = transform.forward * vtInput + transform.right * hzInput;
        controller.Move(dir.normalized * currentSpeed * Time.deltaTime);
    }

    bool IsGrounded()
    {
        spherePos = new Vector3(transform.position.x, transform.position.y - groundYOffset, transform.position.z);
        if (Physics.CheckSphere(spherePos, controller.radius - 0.05f, groundMask)) return true;
        return false;
    }
    void Gravity()
    {
        if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2f;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spherePos, controller.radius - 0.05f);
    }  

}

public abstract class MovementBaseState
{
    public abstract void EnterState(MovementStateManager swat);
  
    public abstract void UpdateState(MovementStateManager swat);
}
