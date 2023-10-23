using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private SpawnPlace[] _places;
    [SerializeField] private Resource[] _possibleResources;
    [SerializeField] private float _minDelay;
    [SerializeField] private float _maxDelay;

    private void Awake()
    {
        StartCoroutine(Spawning());
    }

    private IEnumerator Spawning()
    {
        while (true)
        {
            SpawnPlace place = GetFreePlace();

            if (place != null)
                place.SpawnResource(_possibleResources[Random.Range(0, _possibleResources.Length)]);

            yield return new WaitForSeconds(Random.Range(_minDelay, _maxDelay));
        }
    }

    private SpawnPlace GetFreePlace()
    {
        List<SpawnPlace> places = _places.ToList();

        while (places.Count > 0)
        {
            SpawnPlace possiblePlace = places[Random.Range(0, places.Count)];

            if (possiblePlace.IsOccupied == false)
                return possiblePlace;

            places.Remove(possiblePlace);
        }

        return null;
    }
}