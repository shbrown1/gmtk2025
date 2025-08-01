using UnityEngine;

public class RockHandle : MonoBehaviour, IInteractable
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
        IsInUse = true;
        var player = FindAnyObjectByType<Player>();
        player.RestartLoop();
        Human.SetActive(true);
        Human.transform.SetParent(transform.parent.parent);
    }

    public string Prompt()
    {
        return "Press E to Help Lift";
    }

    public bool IsUseable()
    {
        return !IsInUse;
    }

    public void SetLiftPercentage(float percentage)
    {
        _animator.SetFloat("liftPercentage", percentage);
    }
}
