using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TargetMover))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private float _moveToBaseAccuracy = 2;

    private TargetMover _mover;
    private Resource _currentResource;

    public event UnityAction<Unit> ResourceSent;

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

        if (other.TryGetComponent(out resource) && _currentResource == null)
        {
            _currentResource = resource;
            resource.Pick(transform);
            _mover.SetTarget(_base.transform, _moveToBaseAccuracy);
        }

        Storage storage;

        if (other.TryGetComponent(out storage) && _currentResource != null)
        {
            storage.SendResource(_currentResource);
            _currentResource = null;
            ResourceSent?.Invoke(this);
        }
    }
}