using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Unit[] _units;

    [Header("Scanning")]
    [SerializeField] private SpawnPlace[] _resourcePlaces;
    [SerializeField] private float _scanDelay;

    private List<Unit> _busyUnits = new List<Unit>();
    private List<Unit> _freeUnits = new List<Unit>();
    private Dictionary<Unit, SpawnPlace> _tasks = new Dictionary<Unit, SpawnPlace>();
    private List<SpawnPlace> _freePlaces = new List<SpawnPlace>();

    private void Start()
    {
        _freeUnits = _units.ToList();
        _freePlaces = _resourcePlaces.ToList();

        foreach (var unit in _freeUnits)
        {
            unit.ResourceSent += OnResourceDelivered;
            _tasks[unit] = null;
        }

        StartCoroutine(Scanning());
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
                unit.PickUp(place.Resource);
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

    private void OnResourceDelivered(Unit unit)
    {
        _freePlaces.Add(_tasks[unit]);
        _tasks[unit] = null;
        _busyUnits.Remove(unit);
        _freeUnits.Add(unit);
    }
}