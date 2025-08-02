using Unity.VisualScripting;
using UnityEngine;

public class WallInteractable : MonoBehaviour , IInteractable
{
    public void Interact()
    {
        Debug.Log("Start Climbing Wall");
        FindAnyObjectByType<CameraManager>().ChangeCameraMode(CameraManager.CameraMode.LookingAtWall);
        PlayerController.instance.StartWallGame();

        string[] sounds = { "interact1", "interact2" };
        string sound = sounds[Random.Range(0, 2)];
        AudioManager.instance.PlaySound(sound);
    }

    public string Prompt()
    {
        return "Press E to Climb";
    }
}
