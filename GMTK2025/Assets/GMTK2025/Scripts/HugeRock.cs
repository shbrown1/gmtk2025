using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HugeRock : MonoBehaviour
{
    public AnimationCurve LiftCurve;
    private float LiftDistance = 10f;
    private float LiftSpeed = 1f;
    private List<RockHandle> _rockHandles = new List<RockHandle>();
    private Player _player;
    private Vector3 _startPosition;
    private Vector3 _liftOffset = new Vector3(0, 3, 0); 
    private float _currentLiftPercentage = 0f;

    private void Awake()
    {
        _rockHandles = FindObjectsByType<RockHandle>(FindObjectsSortMode.None).ToList();
        _player = FindAnyObjectByType<Player>();
        _startPosition = transform.position;
    }

    void Update()
    {
        if(_rockHandles.All(handle => handle.IsInUse))
        {
            var distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            if(distanceToPlayer < LiftDistance)
                _currentLiftPercentage += Time.deltaTime * LiftSpeed;
            else
                _currentLiftPercentage -= Time.deltaTime * LiftSpeed;

            _currentLiftPercentage = Mathf.Clamp(_currentLiftPercentage, 0f, 1f);
            var evaluatedLift = LiftCurve.Evaluate(_currentLiftPercentage);

            transform.position = Vector3.Lerp(_startPosition, _startPosition + _liftOffset, evaluatedLift);
            foreach (var handle in _rockHandles)
                handle.SetLiftPercentage(evaluatedLift);
        }
    }
}
