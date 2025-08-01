using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public GameObject currentPlayer;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private GameObject wallMinigame;

    void Awake()
    {
        instance = this;
    }

    public void SwitchToNewPlayer()
    {
        currentPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
    }

    public void StartWallGame()
    {
            Player player = currentPlayer.GetComponent<Player>();
            player.ToggleControllable(false);
            wallMinigame.SetActive(true);
    }
}
