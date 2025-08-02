using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public GameObject currentPlayer;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 spawnPosition;
    public GameObject wallMinigame;
    [SerializeField] private GameObject minigameUI;
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
        minigameUI.SetActive(true);
    }

    void ExitWallGame()
    {
        inWallGame = false;
        currentPlayer.transform.position += new Vector3(0, 1, 4);
        Player player = currentPlayer.GetComponent<Player>();
        wallMinigame.SetActive(false);
        minigameUI.SetActive(false);
        player.EndWallClimb();
        FindAnyObjectByType<CameraManager>().ChangeCameraMode(CameraManager.CameraMode.Following);
        wallMinigame.GetComponent<ClimbingMinigameSlider>().TurnOffArrows();
    }

    void Update()
    {
        if (inWallGame && currentPlayer.GetComponent<Player>().transform.position.y >= 17) //wall height
        {
            ExitWallGame();
        }
    }
}
