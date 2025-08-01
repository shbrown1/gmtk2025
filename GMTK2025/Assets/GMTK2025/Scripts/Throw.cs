using UnityEngine;

public class Throw : MonoBehaviour, IInteractable
{
    public bool IsInUse;
    public GameObject Human;
    private Animator _animator;
    private float _throwTimer = -1;
    private float _throwTime;

    void Awake()
    {
        _animator = Human.GetComponent<Animator>();
    }

    void Update()
    {
        if (_throwTimer != -1)
        {
            _throwTimer += Time.deltaTime;

            if (_throwTimer > _throwTime)
            {
                _throwTimer = -1f;
            }
        }
    }

    public void Interact()
    {
        if (!IsInUse)
        {
            IsInUse = true;
            Human.SetActive(true);
            var player = FindAnyObjectByType<Player>();
            player.RestartLoop();
        }
        else
        {
            _animator.SetTrigger("throw");
        }
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
