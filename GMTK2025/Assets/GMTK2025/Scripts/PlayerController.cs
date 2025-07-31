using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public GameObject currentPlayer;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 spawnPosition;

    void Awake()
    {
        instance = this;
    }

    public void SwitchToNewPlayer()
    {
        currentPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
    }
}
