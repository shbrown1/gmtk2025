using UnityEngine;
using UnityEngine.UI;

public class ClimbingMinigameSlider : MonoBehaviour
{
    private float minHeight = 0.15f * Screen.height;
    private float maxHeight = Screen.height - (0.15f * Screen.height);
    [SerializeField] private float speed;

    private float baseSuccessSizeModifier = 0.40f;
    private float ropeSuccessSizeModifier = 0.35f;

    private float minSuccessHeight = 0.35f * Screen.height;
    private float maxSuccessHeight = Screen.height - (0.35f * Screen.height);
    public float climbDistance;
    public float fallDistance;
    public bool pullingActivated = false;

    [SerializeField] private Image RedZone;
    [SerializeField] private Image GreenZone;
    [SerializeField] private Image slider;
    [SerializeField] private float sizeDownTime;

    private Vector2 baseSliderSize;
    private Vector2 bigSliderSize;
    private float elapsedTime = 0f;
    public bool inSuccessZone;
    private bool isBig = false;


    private enum Direction { up, down };
    private Direction direction;
    void Start()
    {
        minSuccessHeight = baseSuccessSizeModifier * Screen.height;
        maxSuccessHeight = Screen.height - (baseSuccessSizeModifier * Screen.height);
        direction = UnityEngine.Random.Range(1, 3) % 2 == 0 ? Direction.down : Direction.up;
        SetGreenZoneHeight();
        SetRedZoneHeight();
        baseSliderSize = slider.rectTransform.sizeDelta;
        bigSliderSize = slider.rectTransform.sizeDelta * new Vector2(1.2f, 1.2f);
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

        if (isBig)
        {
            if (elapsedTime < sizeDownTime)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / sizeDownTime);
                slider.rectTransform.sizeDelta = Vector2.Lerp(bigSliderSize, baseSliderSize, t);
            }

            if (slider.rectTransform.sizeDelta == baseSliderSize) isBig = false;
        }
        else
        {
            elapsedTime = 0;
        }
        
    }

    public void ProcessButtonPress()
    {
        if (inSuccessZone)
        {
            isBig = true;
            slider.rectTransform.sizeDelta = bigSliderSize;
        }
        else
        {

        }
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

    public void ToggleRopeEffects()
    {
        minSuccessHeight = ropeSuccessSizeModifier * Screen.height;
        maxSuccessHeight = Screen.height - (ropeSuccessSizeModifier * Screen.height);
    }

    public void TogglePullingEffect()
    {
        pullingActivated = true;
    }
}
