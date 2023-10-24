using UnityEngine;

[RequireComponent(typeof(Mover))]
public class TargetMover : MonoBehaviour
{
    private Mover _mover;
    private Transform _target;
    private Vector3 _lastTargetPosition;
    private Vector3 _currentDirection;
    private float _accuracy;

    private void Awake()
    {
        _mover = GetComponent<Mover>();
    }

    private void FixedUpdate()
    {
        if (_target == null)
            return;

        float distance = Vector3.Distance(transform.position, _target.position);

        if (distance > _accuracy)
        {
            RecalculateDirection();
            _mover.Move(_currentDirection);
        }
        else
        {
            _mover.Move(Vector2.zero);
        }
    }

    public void SetTarget(Transform target, float accuracy)
    {
        _target = target;
        _accuracy = accuracy;
        _lastTargetPosition = target.position;
        RecalculateDirection();
    }

    private void RecalculateDirection()
    {
        _lastTargetPosition = _target.position;
        _currentDirection = (_lastTargetPosition - transform.position).normalized;
    }
}