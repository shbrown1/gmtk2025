using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Model;
    public GameObject Ragdoll;

    private float speed = 5f;
    private bool isControllable;
    private new Rigidbody rigidbody;
    private Animator animator;

    private enum State
    {
        Idle,
        Jumping,
    }
    private State currentState = State.Idle;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = Model.GetComponentInChildren<Animator>();
        isControllable = true;
    }
    void Update()
    {
        if (isControllable)
        {
            rigidbody.useGravity = true;
            HandleUserMovement();
        }
        if (CameraManager.instance.GetCameraMode() == CameraManager.CameraMode.LookingAtWall)
        {
            rigidbody.useGravity = false;
            HandleWallGameMovement();
        }
    }

    void HandleWallGameMovement()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ClimbingMinigameSlider climbingMinigame = FindAnyObjectByType<ClimbingMinigameSlider>();

            if (climbingMinigame.inSuccessZone)
            {
                ClimbWall(climbingMinigame.climbDistance);
            }
            else
            {
                FallFromWall(climbingMinigame.fallDistance);
            }
        }
    }

    public void StartWallClimb()
    {
        animator.SetBool("isClimbing", true);
        isControllable = false;
    }

    public void EndWallClimb()
    {
        animator.SetBool("isClimbing", false);
    }

    void ClimbWall(float distance)
    {
        transform.position += new Vector3(0, distance, 0);
        animator.SetTrigger("climb");
    }
    //these could be the same function but seperating them since they'll probably use different animations
    void FallFromWall(float distance)
    {
        transform.position -= new Vector3(0, distance, 0);
    }

    public bool IsCurrentPlayer()
    {
        return isControllable;
    }

    private void FixedUpdate()
    {
        if (currentState == State.Jumping && IsGrounded())
        {
            isControllable = true;
            currentState = State.Idle;
            animator.SetBool("IsJumping", false);
        }
    }

    public void Die()
    {
        if (isControllable)
        {
            isControllable = false;
            Destroy(GetComponent<Collider>()); // Remove capsule collider for ragdoll effect
            Model.SetActive(false);
            Ragdoll.SetActive(true);
            PlayerController.instance.SwitchToNewPlayer();
        }
    }

    void HandleUserMovement()
    {
        var direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.back;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }

        if (direction != Vector3.zero)
        {
            transform.LookAt(transform.position + direction);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        animator.SetFloat("Speed", direction.magnitude);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            currentState = State.Jumping;
            animator.SetBool("IsJumping", true);
            isControllable = false;
            transform.transform.position += Vector3.up * 0.3f; // Small boost to stop grounded state issues
            rigidbody.AddForce(Vector3.up * 5f + transform.forward * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            transform.position = new Vector3(0, 19, 28);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f) && CameraManager.instance.GetCameraMode() != CameraManager.CameraMode.LookingAtWall;
    }
}
