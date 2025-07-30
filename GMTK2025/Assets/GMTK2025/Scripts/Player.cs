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
        float moveInput = 0f;
        float rotationInput = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            moveInput += 1f;
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            moveInput -= 1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            rotationInput += 1f;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            rotationInput -= 1f;
        }
        transform.Translate(Vector3.forward * moveInput * Time.deltaTime * speed);
        transform.Rotate(Vector3.up * rotationInput * Time.deltaTime * rotationSpeed);

        

        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }
}
