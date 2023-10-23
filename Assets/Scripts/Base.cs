using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [Header("Units")]
    [SerializeField] private Unit[] _units;

    [Header("Scanning")]
    [SerializeField] private SpawnPlace[] _resourcePlaces;
    [SerializeField] private float _scanDelay;

    private List<Unit> _busyUnits = new List<Unit>();
    private List<Unit> _freeUnits = new List<Unit>();

    private void Start()
    {
        _freeUnits = _units.ToList();
        StartCoroutine(Scanning());
    }

    private IEnumerator Scanning()
    {
        WaitForSeconds delay = new WaitForSeconds(_scanDelay);

        while (true)
        {
            if (_freeUnits.Count > 0)
            {
                SpawnPlace place = GetOccupiedPlace();

                if (place != null)
                {
                    Unit unit = _freeUnits[0];
                    _freeUnits.Remove(unit);
                    unit.PickUp(place.Resource);
                    _busyUnits.Add(unit);
                }
            }

            yield return delay;
        }
    }

    private SpawnPlace GetOccupiedPlace()
    {
        return _resourcePlaces.FirstOrDefault(x => x.IsOccupied);
    }
}