using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Player _player;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private Vector3 _navLinkStart;
    private Vector3 _navLinkEnd;
    private bool _isOnNavLink;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _player = FindAnyObjectByType<Player>();
        _animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _navMeshAgent.enabled = false;
        _navMeshAgent.autoTraverseOffMeshLink = false;
    }

    private void Update()
    {
        var distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
        if (IsGrounded() && !_isOnNavLink)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(_player.transform.position);
        }

        if(!_isOnNavLink && _navMeshAgent.isOnOffMeshLink)
        {
            _navLinkStart = _navMeshAgent.currentOffMeshLinkData.startPos;
            _navLinkEnd = _navMeshAgent.currentOffMeshLinkData.endPos;
            Debug.Log("Starting off-mesh link: " + _navLinkStart + " to " + _navLinkEnd);
            _isOnNavLink = true;
            _animator.SetBool("IsClimbing", true);
        }

        if (_isOnNavLink)
        {
            _rigidbody.isKinematic = true;
            transform.Translate(Vector3.up * Time.deltaTime * 2f);
            if(transform.position.y >= _navLinkEnd.y)
            {
                _isOnNavLink = false;
                _rigidbody.isKinematic = false;
                _animator.SetBool("IsClimbing", false);
                transform.position = _navLinkEnd;
                _navMeshAgent.CompleteOffMeshLink();
            }
        }

        if (distanceToPlayer < 3f)
            _navMeshAgent.isStopped = true;
        _animator.SetFloat("Speed", _navMeshAgent.velocity.magnitude);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.2f);
    }

    private void Climb()
    {

    }
}
