using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Model;
    public GameObject Ragdoll;

    private float speed = 5f;
    private bool isControllable;
    private new Rigidbody rigidbody;

    private enum State
    {
        Idle,
        Jumping,
    }
    private State currentState = State.Idle;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        isControllable = true;
    }
    void Update()
    {
        if (isControllable)
        {
            HandleUserInput();
        }
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
        }
    }

    public void Die()
    {
        if (isControllable)
        {
            isControllable = false;
            rigidbody.constraints = RigidbodyConstraints.None;
            Model.SetActive(false);
            Ragdoll.SetActive(true);
            PlayerController.instance.SwitchToNewPlayer();
        }
    }

    void HandleUserInput()
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

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            currentState = State.Jumping;
            isControllable = false;
            transform.transform.position += Vector3.up * 0.3f; // Small boost to stop grounded state issues
            rigidbody.AddForce(Vector3.up * 5f + transform.forward * 5f, ForceMode.Impulse);
        }

        /*
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
        */
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
