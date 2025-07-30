using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 5f;
    private bool isControllable;

    void Start()
    {
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
        PlayerController.instance.SwitchToNewPlayer();
    }

    void HandleUserInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }
    }
}
