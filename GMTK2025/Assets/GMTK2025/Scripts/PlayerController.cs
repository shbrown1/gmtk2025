using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public GameObject currentPlayer;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private GameObject wallMinigame;
    private bool inWallGame;

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
        inWallGame = true;
        Player player = currentPlayer.GetComponent<Player>();
        player.ToggleControllable(false);
        wallMinigame.SetActive(true);
    }

    void ExitWallGame()
    {
        inWallGame = false;
        Player player = currentPlayer.GetComponent<Player>();
        player.ToggleControllable(true);
        wallMinigame.SetActive(false);
        FindAnyObjectByType<CameraManager>().ChangeCameraMode(CameraManager.CameraMode.Following);
    }

    void Update()
    {
        if (inWallGame && currentPlayer.GetComponent<Player>().transform.position.y >= 18) //wall height
        {
            ExitWallGame();
        }
    }
}
