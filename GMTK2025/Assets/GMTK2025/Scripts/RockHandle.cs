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
        Invoke("CallRestartLoop", 0.5f);
        Human.SetActive(true);
        Human.transform.SetParent(transform.parent.parent);

        string[] sounds = { "interact1", "interact2" };
        string sound = sounds[Random.Range(0, 2)];
        AudioManager.instance.PlaySound(sound);
    }

    void CallRestartLoop()
    {
        var player = FindAnyObjectByType<Player>();
        player.RestartLoop();
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
