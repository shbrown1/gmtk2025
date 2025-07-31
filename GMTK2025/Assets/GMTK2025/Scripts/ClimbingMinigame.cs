using UnityEngine;
using UnityEngine.Rendering;

public class ClimbingMinigame : MonoBehaviour
{
    public float climbStep; //how much you move up on success
    public float slipAmount; //how much you fall on fail
    public float wallHeight;

    public float barSpeed;
    private float barValue = 0f;

    public float successMin;
    public float successMax;
}
