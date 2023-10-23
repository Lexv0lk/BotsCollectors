using UnityEngine;

[RequireComponent(typeof(TargetMover))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private float _moveToBaseAccuracy = 2;

    private TargetMover _mover;

    private void Awake()
    {
        _mover = GetComponent<TargetMover>();
    }

    public void PickUp(Resource resource)
    {
        _mover.SetTarget(resource.transform, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Resource resource;

        if (other.TryGetComponent(out resource))
        {
            resource.Pick(transform);
            _mover.SetTarget(_base.transform, _moveToBaseAccuracy);
        }
    }
}