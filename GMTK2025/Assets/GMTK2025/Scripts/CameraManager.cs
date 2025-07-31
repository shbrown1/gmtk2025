using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Vector3 _playerOffset = new Vector3(0, 10, -5);
    private Camera _camera;
    private Player _player;

    public enum CameraMode
    {
        Following,
        LookingAtWall,
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
    }

    private void Update()
    {
        if (currentMode == CameraMode.Following)
        {
            FollowPlayer();
        }
    }

    public void ChangeCameraMode(CameraMode mode)
    {
        currentMode = mode;
    }

    public void FollowPlayer()
    {
        _camera.transform.position = _player.transform.position + _playerOffset;
    }
}
