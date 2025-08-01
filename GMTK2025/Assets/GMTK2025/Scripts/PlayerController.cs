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
        FindAnyObjectByType<Interactor>().interactorSource = currentPlayer.transform;
    }

    public void StartWallGame()
    {
        inWallGame = true;
        Player player = currentPlayer.GetComponent<Player>();
        player.StartWallClimb();
        wallMinigame.SetActive(true);
    }

    void ExitWallGame()
    {
        inWallGame = false;
        Player player = currentPlayer.GetComponent<Player>();
        wallMinigame.SetActive(false);
        player.EndWallClimb();
        currentPlayer.transform.position += new Vector3(0, 1, 2);
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
