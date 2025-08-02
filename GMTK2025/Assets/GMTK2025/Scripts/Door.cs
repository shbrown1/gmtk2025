using UnityEngine;

public class Door : MonoBehaviour
{
    private bool IsRunning = false;
    private float Timer;
    private Vector3 StartPosition;
    private Vector3 Offset = new Vector3(0, 5, 0);
    private Vector3 EndPosition => StartPosition + Offset;

    private void Awake()
    {
        StartPosition = transform.position;
    }

    private void Update()
    {
        if (IsRunning)
        {
            Timer += Time.deltaTime;
            transform.position = Vector3.Lerp(StartPosition, EndPosition, Timer);
        }
    }

    public void LiftDoor()
    {
        IsRunning = true;
    }
}
