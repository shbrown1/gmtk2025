using UnityEngine;

public class ClimbingMinigameSlider : MonoBehaviour
{
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    [SerializeField] private float speed;
    [SerializeField] private float minSuccessHeight;
    [SerializeField] private float maxSuccessHeight;

    [SerializeField] private Material successZoneMaterial;
    [SerializeField] private Material failZoneMaterial;

    public bool inSuccessZone;
    private Renderer objectRenderer;


    private enum Direction { up, down };
    private Direction direction;

    void Start()
    {
        direction = Random.Range(1, 3) % 2 == 0 ? Direction.down : Direction.up;
        objectRenderer = GetComponent<Renderer>();
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

        inSuccessZone = transform.position.y >= minSuccessHeight && transform.position.y <= maxSuccessHeight;

        if (inSuccessZone)
        {
            objectRenderer.material = successZoneMaterial;
        }
        else
        {
            objectRenderer.material = failZoneMaterial;
        }
    }
}
