using UnityEngine;

public class Rope : MonoBehaviour, IInteractable
{
    public bool IsInUse;
    public GameObject Human;
    public GameObject RopeModel;
    private Animator _animator;

    void Awake()
    {
        _animator = Human.GetComponent<Animator>();
    }

    public void Interact()
    {
        IsInUse = true;
        RopeModel.SetActive(false);
        Human.SetActive(true);
        var player = FindAnyObjectByType<Player>();
        player.Model.SetActive(false);
        Invoke("CallRestartLoop", 1f);
        PlayerController.instance.wallMinigame.GetComponent<ClimbingMinigameSlider>().ToggleRopeEffects();

        string[] sounds = { "interact1", "interact2" };
        string sound = sounds[Random.Range(0, 2)];
        AudioManager.instance.PlaySound(sound);
    }

    void CallRestartLoop()
    {
        var player = FindAnyObjectByType<Player>();
        player.RestartLoop();
        player.Model.SetActive(true);
    }


    public string Prompt()
    {
        return "Press E to Use Rope";
    }

    public bool IsUseable()
    {
        return !IsInUse;
    }
}
