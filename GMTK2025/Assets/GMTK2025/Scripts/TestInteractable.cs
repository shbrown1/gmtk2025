using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interacted!");
    }

    public string Prompt()
    {
        return "Press E to Interact";
    }
}
