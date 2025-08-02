using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Vector3 _playerOffset = new Vector3(0, 10, -5);
    private Vector3 _playerCameraRotation = new Vector3(60, 0, 0);
    private Vector3 _wallPosition = new Vector3(0, 14, 2.5f);
    private Vector3 _wallRotation = new Vector3(13, -3, 0);
    private Vector3 _stairPosition = new Vector3(-3, 15, 50f);
    private Vector3 _stairRotation = new Vector3(28, 0, 0);
    private Vector3 _treasurePosition = new Vector3(-3, 15, 72);
    private Vector3 _treasureRotation = new Vector3(28, 0, 0);
    private Camera _camera;
    private Player _player;
    public static CameraManager instance;

    public enum CameraMode
    {
        Following,
        LookingAtWall,
        LookingAtStairs,
        LookingAtTreasure,
    }
    CameraMode currentMode = CameraMode.Following;


    private void Awake()
    {
        _camera = FindAnyObjectByType<Camera>();
        if (_camera == null)
            Debug.LogError("Camera component not found on CameraManager.");

        _player = FindAnyObjectByType<Player>();
        if (_player == null)
            Debug.LogError("Player not found on CameraManager");

        instance = this;
    }

    private void Update()
    {
        _player = PlayerController.instance.currentPlayer.GetComponent<Player>();
        var desiredPosition = Vector3.zero;
        var desiredRotation = Quaternion.identity;
        if (currentMode == CameraMode.Following)
        {
            desiredPosition = _player.transform.position + _playerOffset;
            desiredRotation = Quaternion.Euler(_playerCameraRotation);
        }
        else if (currentMode == CameraMode.LookingAtWall)
        {
            desiredPosition = _wallPosition;
            desiredRotation = Quaternion.Euler(_wallRotation);
        }
        else if (currentMode == CameraMode.LookingAtStairs)
        {
            desiredPosition = _stairPosition;
            desiredRotation = Quaternion.Euler(_stairRotation);
        }
        else if (currentMode == CameraMode.LookingAtTreasure)
        {
            desiredPosition = _treasurePosition;
            desiredRotation = Quaternion.Euler(_treasureRotation);
        }

        float cameraMoveSpeed = 2.5f;
        float cameraRotateSpeed = 3;
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, desiredPosition, Time.deltaTime * cameraMoveSpeed);
        _camera.transform.rotation = Quaternion.Slerp(_camera.transform.rotation, desiredRotation, Time.deltaTime * cameraRotateSpeed);
    }

    public void ChangeCameraMode(CameraMode mode)
    {
        currentMode = mode;
    }

    public CameraMode GetCameraMode()
    {
        return currentMode;
    }
}
