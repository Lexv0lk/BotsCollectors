using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TargetMover))]
public class Unit : MonoBehaviour
{
    [SerializeField] private float _moveToBaseAccuracy = 2;

    private Base _base;
    private TargetMover _mover;
    private Resource _currentResource;
    private ITarget _currentTarget;
    private SpawnPlace _targetedResourcePlace;

    public event UnityAction<Unit> TaskFinished;

    private void Awake()
    {
        _mover = GetComponent<TargetMover>();
    }

    public void Initialize(Base parentBase)
    {
        _base = parentBase;
    }

    public void PickResource(SpawnPlace resourcePlace)
    {
        _targetedResourcePlace = resourcePlace;
        _currentTarget = resourcePlace.Resource;
        _mover.SetTarget(resourcePlace.Resource.transform, 0);
        _targetedResourcePlace.ResourcePicked += OnResourcePicked;
    }

    public void CreateBase(Flag flag)
    {
        _currentTarget = flag;
        _mover.SetTarget(flag.transform, 0);
    }

    private void OnResourcePicked()
    {
        _mover.SetTarget(_base.transform, _moveToBaseAccuracy);

        if (_currentResource == null)
            TaskFinished?.Invoke(this);

        _targetedResourcePlace.ResourcePicked -= OnResourcePicked;
    }

    private void OnTriggerEnter(Collider other)
    {
        Resource resource;

        if (other.TryGetComponent(out resource) && _currentTarget == resource && _currentResource == null)
        {
            _currentResource = resource;
            resource.Pick(transform);
        }

        Flag flag;

        if (other.TryGetComponent(out flag) && _currentTarget == flag)
        {
            flag.CreateBase(this);
        }

        Storage storage;

        if (other.TryGetComponent(out storage) && _currentResource != null && storage == _base.Storage)
        {
            storage.SendResource(_currentResource);
            _currentResource = null;
            TaskFinished?.Invoke(this);
        }
    }
}