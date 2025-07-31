using UnityEngine;

public class WallInteractable : MonoBehaviour , IInteractable
{
    public void Interact()
    {
        Debug.Log("Start Climbing Wall");
    }

    public string Prompt()
    {
        return "Press E to Climb";
    }
}
