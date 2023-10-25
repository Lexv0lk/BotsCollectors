using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [Header("Units")]
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private Transform[] _possibleSpawnPlaces;

    [Header("Scanning")]
    [SerializeField] private float _scanDelay;

    [Header("Creating")]
    [SerializeField] private int _newUnitCost = 3;
    [SerializeField] private int _newBaseCost = 5;

    [Space]
    [SerializeField] private Storage _storage;

    private List<Unit> _busyUnits = new List<Unit>();
    private List<Unit> _freeUnits = new List<Unit>();
    private Dictionary<Unit, SpawnPlace> _tasks = new Dictionary<Unit, SpawnPlace>();
    private List<SpawnPlace> _freePlaces = new List<SpawnPlace>();
    private Flag _currentFlag = null;
    private FlagCreator _flagCreator;
    private SpawnPlace[] _resourcePlaces;

    public Storage Storage => _storage;

    private void Start()
    {
        _resourcePlaces = FindObjectsOfType<SpawnPlace>();
        _flagCreator = FlagCreator.Instance;
        _flagCreator.FlagCreated += OnFlagCreated;

        if (_freeUnits.Count == 0)
            CreateNewUnit();

        _freePlaces = _resourcePlaces.ToList();

        StartCoroutine(Scanning());
    }

    private void OnEnable()
    {
        _storage.AddedResource += OnResourceAddedToStorage;
    }

    private void OnDisable()
    {
        _storage.AddedResource -= OnResourceAddedToStorage;
    }

    public void AddUnit(Unit unit)
    {
        InitializeUnit(unit);
    }

    private IEnumerator Scanning()
    {
        WaitForSeconds delay = new WaitForSeconds(_scanDelay);

        while (true)
        {
            List<SpawnPlace> possiblePlaces = GetOccupiedPlaces();

            while (_freeUnits.Count > 0 && possiblePlaces.Count > 0)
            {
                SpawnPlace place = possiblePlaces[Random.Range(0, possiblePlaces.Count)];
                possiblePlaces.Remove(place);
                _freePlaces.Remove(place);

                Unit unit = _freeUnits[0];
                _freeUnits.Remove(unit);
                unit.PickResource(place);
                _busyUnits.Add(unit);
                _tasks[unit] = place;
            }

            yield return delay;
        }
    }

    private List<SpawnPlace> GetOccupiedPlaces()
    {
        return _freePlaces.Where(x => x.IsOccupied).ToList();
    }

    private void OnUnitFinished(Unit unit)
    {
        _freePlaces.Add(_tasks[unit]);
        _tasks[unit] = null;
        _busyUnits.Remove(unit);
        _freeUnits.Add(unit);
    }

    private void OnFlagCreated()
    {
        _currentFlag = _flagCreator.CurrentFlag;
        _flagCreator.FlagCreated -= OnFlagCreated;
    }

    private void OnResourceAddedToStorage()
    {
        if (_currentFlag == null && _storage.ResourcesCount >= _newUnitCost)
        {
            _storage.SpendResources(_newUnitCost);
            CreateNewUnit();
        }
        else if (_currentFlag != null && _storage.ResourcesCount >= _newBaseCost)
        {
            _storage.SpendResources(_newBaseCost);
            StartCoroutine(SendUnitToCreateBase());
        }
    }

    private IEnumerator SendUnitToCreateBase()
    {
        while (_freeUnits.Count == 0)
            yield return null;

        Unit unit = _freeUnits[0];
        _freeUnits.Remove(unit);
        unit.TaskFinished -= OnUnitFinished;
        unit.CreateBase(_currentFlag);
        _currentFlag = null;
    }

    private void CreateNewUnit()
    {
        Unit unit = Instantiate(_unitPrefab, transform, true);
        unit.transform.position = _possibleSpawnPlaces[Random.Range(0, _possibleSpawnPlaces.Length)].position;
        InitializeUnit(unit);
    }

    private void InitializeUnit(Unit unit)
    {
        unit.Initialize(this);
        _freeUnits.Add(unit);
        unit.TaskFinished += OnUnitFinished;
        _tasks[unit] = null;
    }
}