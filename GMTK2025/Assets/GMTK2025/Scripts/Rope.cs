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
        var player = FindAnyObjectByType<Player>();
        player.RestartLoop();
        Human.SetActive(true);
        PlayerController.instance.wallMinigame.GetComponent<ClimbingMinigameSlider>().ToggleRopeEffects();
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
