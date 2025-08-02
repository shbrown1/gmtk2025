using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    public bool IsInUse;
    public GameObject LeverObject;
    public AnimationCurve RotationCurve;
    private float RotationMax = -153;
    private float RotationMin = -24;
    private float Timer;
    private float StartDelay = 0.2f;
    private bool IsRunning = false;

    public void Interact()
    {
        IsInUse = true;
        var player = FindAnyObjectByType<Player>();
        player.transform.position = new Vector3(3, 10, 79);
        player.transform.rotation = Quaternion.Euler(0, 270, 0);
        player.isControllable = false;
        var animator = player.Model.GetComponentInChildren<Animator>();
        animator.SetTrigger("useLever");
        IsRunning = true;
    }

    void Update()
    {
        if (IsRunning)
        {
            Timer += Time.deltaTime;
            LeverObject.transform.localRotation = Quaternion.Euler(Mathf.Lerp(RotationMin, RotationMax, RotationCurve.Evaluate((Timer - StartDelay) * 2)), 90, 0);
            if (Timer > 1)
            {
                IsRunning = false;
                var player = FindAnyObjectByType<Player>();
                player.isControllable = true;
                var door = FindAnyObjectByType<Door>();
                door.LiftDoor();
            }
        }
    }

    public string Prompt()
    {
        return "Press E to Help Lift";
    }

    public bool IsUseable()
    {
        return !IsInUse;
    }
}
