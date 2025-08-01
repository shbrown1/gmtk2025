using Unity.VisualScripting;
using UnityEngine;

public class WallInteractable : MonoBehaviour , IInteractable
{
    public void Interact()
    {
        Debug.Log("Start Climbing Wall");
        FindAnyObjectByType<CameraManager>().ChangeCameraMode(CameraManager.CameraMode.LookingAtWall);
        PlayerController.instance.StartWallGame();
    }

    public string Prompt()
    {
        return "Press E to Climb";
    }
}
