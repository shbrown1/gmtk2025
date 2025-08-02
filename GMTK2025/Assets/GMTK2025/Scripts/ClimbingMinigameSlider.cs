using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClimbingMinigameSlider : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Color SuccessColor;
    [SerializeField] private Color FailureColor;
    public float successZoneHeight = 100;
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
        baseSliderSize = slider.rectTransform.sizeDelta;
        bigSliderSize = slider.rectTransform.sizeDelta * new Vector2(1.3f, 1.3f);
    }

    void Update()
    {
        var currentSpeed = speed * (direction == Direction.down ? -1 : 1);
        var yPosition = transform.localPosition.y + Time.deltaTime * currentSpeed;
        yPosition = Mathf.Clamp(yPosition, -RedZone.rectTransform.sizeDelta.y / 2f, RedZone.rectTransform.sizeDelta.y / 2f);
        transform.localPosition = new Vector3(transform.localPosition.x, yPosition, transform.localPosition.z);

        if(Mathf.Abs(transform.localPosition.y) >= RedZone.rectTransform.sizeDelta.y / 2f)
        {
            if(direction == Direction.up)
                direction = Direction.down;
            else 
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
        isBig = true;
        slider.rectTransform.sizeDelta = bigSliderSize;

        if (inSuccessZone)
        {
            StartCoroutine(AnimateArrow(upArrow, Vector2.up));
        }
        else
        {
            StartCoroutine(AnimateArrow(downArrow, Vector2.down));
        }
    }

    void CheckSuccessZone()
    {
        var image = GetComponent<Image>();
        if (Mathf.Abs(transform.localPosition.y) - slider.rectTransform.sizeDelta.y / 2f <= successZoneHeight / 2f)
        {
            inSuccessZone = true;
            image.color = SuccessColor;
        }
        else
        {
            inSuccessZone = false;
            image.color = FailureColor;
        }
    }

    private void SetGreenZoneHeight()
    {
        RectTransform greenZoneRT = GreenZone.GetComponent<RectTransform>();
        greenZoneRT.sizeDelta = new Vector2(greenZoneRT.sizeDelta.x, successZoneHeight);
    }

    public void ToggleRopeEffects()
    {
        successZoneHeight = 300;
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
