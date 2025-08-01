using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ClimbingMinigameSlider : MonoBehaviour
{
    private float minHeight = 0.15f * Screen.height;
    private float maxHeight = Screen.height - (0.15f * Screen.height);
    [SerializeField] private float speed;
    private float minSuccessHeight = 0.35f * Screen.height;
    private float maxSuccessHeight = Screen.height - (0.35f * Screen.height);
    public float climbDistance;
    public float fallDistance;

    [SerializeField] private Image RedZone;
    [SerializeField] private Image GreenZone;
    [SerializeField] private Image slider;

    public bool inSuccessZone;


    private enum Direction { up, down };
    private Direction direction;
    void Start()
    {
        direction = Random.Range(1, 3) % 2 == 0 ? Direction.down : Direction.up;
        SetGreenZoneHeight();
        SetRedZoneHeight();
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

        CheckSuccessZone();
    }

    void CheckSuccessZone()
    {
        RectTransform sliderRT = slider.GetComponent<RectTransform>();
        //TODO: this check should feel better
        float spriteHeight = sliderRT.rect.height;
        float bottomY = transform.position.y - spriteHeight / 2f;
        float topY = transform.position.y - spriteHeight / 2f;
        inSuccessZone = topY >= minSuccessHeight && bottomY <= maxSuccessHeight;
    }

    private void SetGreenZoneHeight()
    {
        RectTransform greenZoneRT = GreenZone.GetComponent<RectTransform>();
        RectTransform canvasRT = GreenZone.canvas.GetComponent<RectTransform>();

        float zoneHeight = maxSuccessHeight - minSuccessHeight;
        float zoneMidY = (maxSuccessHeight + minSuccessHeight) / 2f;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        canvasRT,
            new Vector2(Screen.width / 2f, zoneMidY),
            null,
            out localPoint
        );
        greenZoneRT.anchoredPosition = new Vector2(greenZoneRT.anchoredPosition.x, localPoint.y);
        greenZoneRT.sizeDelta = new Vector2(greenZoneRT.sizeDelta.x, zoneHeight);
    }

    private void SetRedZoneHeight()
    {
        RectTransform redZoneRT = RedZone.GetComponent<RectTransform>();
        RectTransform canvasRT = RedZone.canvas.GetComponent<RectTransform>();

        float zoneHeight = maxHeight - minHeight;
        float zoneMidY = (maxHeight + minHeight) / 2f;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
        canvasRT,
            new Vector2(Screen.width / 2f, zoneMidY),
            null,
            out localPoint
        );
        redZoneRT.anchoredPosition = new Vector2(redZoneRT.anchoredPosition.x, localPoint.y);
        redZoneRT.sizeDelta = new Vector2(redZoneRT.sizeDelta.x, zoneHeight);
    }
}
