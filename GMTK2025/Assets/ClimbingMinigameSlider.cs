using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ClimbingMinigameSlider : MonoBehaviour
{
    private float minHeight = 0.25f * Screen.height;
    private float maxHeight = Screen.height - (0.25f * Screen.height);
    [SerializeField] private float speed;
    private float minSuccessHeight = 0.35f * Screen.height;
    private float maxSuccessHeight = Screen.height - (0.35f * Screen.height);

    public bool inSuccessZone;
    private Image sprite;


    private enum Direction { up, down };
    private Direction direction;

    //TODO: scalable size (use lerping)
    //TODO: check for input presses during move up or down
    void Start()
    {
        direction = Random.Range(1, 3) % 2 == 0 ? Direction.down : Direction.up;
        sprite = GetComponent<Image>();
    }
    void Update()
    {
        if (direction == Direction.up)
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        else
        {
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }

        if (transform.position.y >= maxHeight)
        {
            direction = Direction.down;
        }
        else if (transform.position.y <= minHeight)
        {
            direction = Direction.up;
        }

        if (inSuccessZone)
        {
            sprite.color = Color.green;
        }
        else
        {
            sprite.color = Color.red;
        }

        CheckSuccessZone();
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inSuccessZone)
            {
                Debug.Log("Climb");
            }
            else
            {
                Debug.Log("Fall");
            }
        }
    }

    void CheckSuccessZone()
    {
        RectTransform rt = GetComponent<RectTransform>();
        float spriteHeight = rt.rect.height;

        //this assumes pivot in center
        float bottomY = transform.position.y - spriteHeight / 2f;
        float topY = transform.position.y - spriteHeight / 2f;

        inSuccessZone = topY >= minSuccessHeight && bottomY <= maxSuccessHeight;
        sprite.color = inSuccessZone ? Color.green : Color.red;
    }
}
