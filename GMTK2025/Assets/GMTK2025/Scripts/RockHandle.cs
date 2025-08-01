using UnityEngine;

public class RockHandle : MonoBehaviour, IInteractable
{
    public bool IsInUse;
    public GameObject Human;

    public void Interact()
    {
        IsInUse = true;
        var player = FindAnyObjectByType<Player>();
        player.RestartLoop();
        Human.SetActive(true);
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
