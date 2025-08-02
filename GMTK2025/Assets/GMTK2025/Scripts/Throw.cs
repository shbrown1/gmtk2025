using UnityEngine;

public class Throw : MonoBehaviour, IInteractable
{
    public bool IsInUse;
    public GameObject Human;
    private Animator _animator;
    private Player _player;
    private float _throwTimer = -1;
    private float _throwTime = 3;

    void Awake()
    {
        _animator = Human.GetComponent<Animator>();
        _player = FindAnyObjectByType<Player>();
    }

    void Update()
    {
        if (_throwTimer != -1)
        {
            _throwTimer += Time.deltaTime;

            if (_throwTimer > _throwTime)
            {
                _throwTimer = -1f;
                _player.Stand();
            }
        }
    }
    
    void CallRestartLoop()
    {
        var player = FindAnyObjectByType<Player>();
        player.RestartLoop();
    }


    public void Interact()
    {
        if (!IsInUse)
        {
            IsInUse = true;
            Human.SetActive(true);
            Invoke("CallRestartLoop", 0.5f);
        }
        else
        {
            _animator.SetTrigger("throw");
            _throwTimer = 0f;
            _player.transform.position = transform.position + (Vector3.back * 1);
            _player.Throw();
        }

        string[] sounds = { "interact1", "interact2" };
        string sound = sounds[Random.Range(0, 2)];
        AudioManager.instance.PlaySound(sound);
    }

    public string Prompt()
    {
        if (IsInUse)
            return "Press E to Get Thrown";

        return "Press E to Help Throw";
    }

    public bool IsUseable()
    {
        return _throwTimer == -1f;
    }
}
