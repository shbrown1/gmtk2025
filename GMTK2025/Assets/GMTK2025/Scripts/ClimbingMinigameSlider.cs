using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClimbingMinigameSlider : MonoBehaviour
{
    private float minHeight = 150f;
    private float maxHeight = 750f;
    [SerializeField] private float speed;
    private float minSuccessHeight = 350f;
    private float maxSuccessHeight = 550f;
    public float climbDistance;
    public float fallDistance;
    public bool pullingActivated = false;

    [SerializeField] private Image RedZone;
    [SerializeField] private Image GreenZone;
    [SerializeField] private Image slider;
    [SerializeField] private Image upArrow;
    [SerializeField] private Image downArrow;
    [SerializeField] private float sizeDownTime;
    [SerializeField] private float arrowMoveDistance;
    [SerializeField] private float arrowFadeDuration;

    private Vector2 baseSliderSize;
    private Vector2 bigSliderSize;
    private float elapsedTime = 0f;
    public bool inSuccessZone;
    private bool isBig = false;


    private enum Direction { up, down };
    private Direction direction;
    void Start()
    {
        direction = UnityEngine.Random.Range(1, 3) % 2 == 0 ? Direction.down : Direction.up;
        SetGreenZoneHeight();
        SetRedZoneHeight();
        baseSliderSize = slider.rectTransform.sizeDelta;
        bigSliderSize = slider.rectTransform.sizeDelta * new Vector2(1.3f, 1.3f);
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
            StartCoroutine(AnimateArrow(upArrow, Vector2.up));
        }
        else
        {
            StartCoroutine(AnimateArrow(downArrow, Vector2.down));
        }
    }

    void CheckSuccessZone()
    {
        RectTransform sliderRT = slider.GetComponent<RectTransform>();
        //TODO: this check should feel better
        float spriteHeight = sliderRT.rect.height;
        float bottomY = transform.position.y - spriteHeight / 2f;
        float topY = transform.position.y + spriteHeight / 2f;
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
        minSuccessHeight = 300f;
        maxSuccessHeight = 600f;
        SetGreenZoneHeight();
    }

    public void TogglePullingEffect()
    {
        pullingActivated = true;
    }

    private IEnumerator AnimateArrow(Image arrow, Vector2 direction)
    {
        arrow.gameObject.SetActive(true);

        RectTransform arrowRT = arrow.rectTransform;
        Vector2 startPos = slider.transform.position;
        Vector2 endPos = startPos + direction * arrowMoveDistance;

        Color startColor = new Color(arrow.color.r, arrow.color.g, arrow.color.b, 1f); //fading by changing color alpha
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);

        float elapsedTime = 0f;
        while (elapsedTime < arrowFadeDuration)
        {
            float t = elapsedTime / arrowFadeDuration;
            arrowRT.position = Vector2.Lerp(startPos, endPos, t);
            arrow.color = Color.Lerp(startColor, endColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        arrowRT.position = endPos;
        arrow.color = endColor;
        arrow.gameObject.SetActive(false);
    }

    public void TurnOffArrows()
    {
        upArrow.gameObject.SetActive(false);
        downArrow.gameObject.SetActive(false);
    }

}
