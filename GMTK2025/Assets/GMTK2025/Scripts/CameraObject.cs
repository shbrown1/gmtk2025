using UnityEngine;

public class CameraObject : MonoBehaviour //naming this file "camera" broke a ton of shit
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    public static CameraObject instance;

    void Awake()
    {
        instance = this;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }

    public void UpdatePlayerToFollow(Transform newPlayer)
    {
        player = newPlayer;
    }
}
