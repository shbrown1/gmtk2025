using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Model;
    public GameObject Ragdoll;

    private float speed = 5f;
    public bool isControllable;
    private new Rigidbody rigidbody;
    private Animator animator;
    private Vector3 restartPosition = new Vector3(0, 2, 0);
    private GameObject _currentRagdoll;
    private float wallClimbPressCooldown = 0.2f;
    private float nextPressTime = 0f;

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

        var lever = FindAnyObjectByType<Lever>();
        if (CameraManager.instance.GetCameraMode() == CameraManager.CameraMode.LookingAtStairs && transform.position.z > 77.3f && lever.IsInUse)
        {
            CameraManager.instance.ChangeCameraMode(CameraManager.CameraMode.LookingAtTreasure);
        }
        else if(CameraManager.instance.GetCameraMode() == CameraManager.CameraMode.LookingAtTreasure && transform.position.z < 77.3f && lever.IsInUse)
        {
            CameraManager.instance.ChangeCameraMode(CameraManager.CameraMode.LookingAtStairs);
        }
        else if (CameraManager.instance.GetCameraMode() == CameraManager.CameraMode.Following && transform.position.z > 63)
        {
            CameraManager.instance.ChangeCameraMode(CameraManager.CameraMode.LookingAtStairs);
        }
        else if (CameraManager.instance.GetCameraMode() == CameraManager.CameraMode.LookingAtStairs && transform.position.z < 63)
        {
            CameraManager.instance.ChangeCameraMode(CameraManager.CameraMode.Following);
        }

        if (CameraManager.instance.GetCameraMode() == CameraManager.CameraMode.LookingAtWall)
        {
            rigidbody.useGravity = false;
            HandleWallGameMovement();
        }
    }

    void HandleWallGameMovement()
    {
        ClimbingMinigameSlider climbingMinigame = FindAnyObjectByType<ClimbingMinigameSlider>();
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextPressTime)
        {
            if (climbingMinigame.inSuccessZone)
            {
                ClimbWall(climbingMinigame.climbDistance);
            }
            else
            {
                FallFromWall(climbingMinigame.fallDistance);
            }

            climbingMinigame.ProcessButtonPress();
            nextPressTime = Time.time + wallClimbPressCooldown;
        }

        if (climbingMinigame.pullingActivated)
        {
            float pullSpeed = 1f;
            transform.position += new Vector3(0, pullSpeed * Time.deltaTime, 0);
        }

    }

    public void StartWallClimb()
    {
        transform.position = new Vector3(-1, 1, 20);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        animator.SetFloat("Speed", 0);
        animator.SetBool("isClimbing", true);
        var rope = FindAnyObjectByType<Rope>();
        if (rope.IsInUse)
        {
            animator.SetBool("hasRope", true);
        }
        isControllable = false;
    }

    public void EndWallClimb()
    {
        animator.SetBool("isClimbing", false);
        isControllable = true;
    }

    void ClimbWall(float distance)
    {
        transform.position += new Vector3(0, distance, 0);
        animator.SetTrigger("climb");
    }
    //these could be the same function but seperating them since they'll probably use different animations
    void FallFromWall(float distance)
    {
        if (transform.position.y >= 1) transform.position -= new Vector3(0, distance, 0);
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

        if(Input.GetKeyDown(KeyCode.L))
        {
            transform.position = new Vector3(-1.75f, -7.5f, 47);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            transform.position = new Vector3(-5, 10, 79);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f) && CameraManager.instance.GetCameraMode() != CameraManager.CameraMode.LookingAtWall;
    }

    public void RestartLoop()
    {
        transform.position = restartPosition;
    }

    public void Throw()
    {
        isControllable = false;
        gameObject.SetActive(false);
        _currentRagdoll = Instantiate(Ragdoll, transform.position, Quaternion.identity);
        _currentRagdoll.SetActive(true);
        var firstChild = _currentRagdoll.GetComponentInChildren<Rigidbody>();
        firstChild.AddForce(Vector3.up * 240f + Vector3.forward * 50f, ForceMode.Impulse);
    }

    public void Stand()
    {
        isControllable = true;
        gameObject.SetActive(true);
        var firstChild = _currentRagdoll.GetComponentInChildren<Rigidbody>();
        transform.position = firstChild.transform.position + Vector3.up;
        Destroy(_currentRagdoll);
        _currentRagdoll = null;
    }
}
