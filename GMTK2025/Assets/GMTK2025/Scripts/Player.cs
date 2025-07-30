using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 5f;
    private float rotationSpeed = 180f;
    private bool isControllable;
    private new Rigidbody rigidbody;
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

    public void Die()
    {
        isControllable = false;
        rigidbody.constraints = RigidbodyConstraints.None;
        PlayerController.instance.SwitchToNewPlayer();
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
            transform.Translate(Vector3.forward* speed * Time.deltaTime);
        }

        /*
        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
        */
    }
}
