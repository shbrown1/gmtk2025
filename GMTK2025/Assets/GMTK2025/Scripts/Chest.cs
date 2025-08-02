using UnityEngine;
using UnityEngine.SceneManagement;

public class Chest : MonoBehaviour, IInteractable
{
    public bool IsInUse;
    public GameObject ChestTop;
    public AnimationCurve RotationCurve;
    public GameObject Treasure;
    private float RotationMax = 15;
    private float RotationMin = -90;
    private float Timer;
    private float StartDelay = 0.2f;
    private bool IsRunning = false;
    private bool successSoundPlayed;

    void Awake()
    {
        successSoundPlayed = false;
        if (Treasure != null)
            Treasure.SetActive(false);
    }

    public void Interact()
    {
        IsInUse = true;
        var player = FindAnyObjectByType<Player>();
        player.transform.position = new Vector3(-3.8f, 10, 82.5f);
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        player.isControllable = false;
        var animator = player.Model.GetComponentInChildren<Animator>();
        animator.SetTrigger("useLever");
        animator.SetFloat("Speed", 0);
        IsRunning = true;

        AudioManager.instance.PlaySound("chest");
    }

    void Update()
    {
        if (Treasure != null && Treasure.activeSelf)
        {
            Treasure.transform.Rotate(Vector3.up * 70 * Time.deltaTime, Space.World);
            PlaySuccessSoundOnce();
        }

        if (IsRunning)
        {

            Timer += Time.deltaTime;
            ChestTop.transform.localRotation = Quaternion.Euler(Mathf.Lerp(RotationMin, RotationMax, RotationCurve.Evaluate((Timer - StartDelay) * 2)), 0, 0);
            if (Timer > 1.5f)
            {
                IsRunning = false;
                var player = FindAnyObjectByType<Player>();
                player.transform.rotation = Quaternion.Euler(0, 180, 0);
                var animator = player.Model.GetComponentInChildren<Animator>();
                animator.SetTrigger("openChest");
                Treasure.SetActive(true);
                Invoke("BackToHomeScreen", 5f);
            }
        }
    }

    void BackToHomeScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public string Prompt()
    {
        return "Press E to Open Chest";
    }

    public bool IsUseable()
    {
        return !IsInUse;
    }

    private void PlaySuccessSoundOnce()
    {
        if (!successSoundPlayed)
        {
            AudioManager.instance.PlaySound("treasure");
            successSoundPlayed = true;
        }
    }
}
