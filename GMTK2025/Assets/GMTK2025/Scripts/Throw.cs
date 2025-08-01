using UnityEngine;

public class Throw : MonoBehaviour, IInteractable
{
    public bool IsInUse;
    public GameObject Human;
    private Animator _animator;

    void Awake()
    {
        _animator = Human.GetComponent<Animator>();
    }

    public void Interact()
    {
        Human.SetActive(true);
        IsInUse = true;
        var player = FindAnyObjectByType<Player>();
        player.RestartLoop();
    }

    public string Prompt()
    {
        return "Press E to Help Throw";
    }

    public bool IsInteractable()
    {
        return IsInUse;
    }
}
