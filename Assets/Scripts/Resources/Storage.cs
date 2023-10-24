using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Storage : MonoBehaviour
{
    private List<Resource> _resources = new List<Resource>();

    public event UnityAction AddedResource;

    public int ResourcesCount => _resources.Count;

    public void SendResource(Resource resource)
    {
        resource.Pick(transform);
        _resources.Add(resource);
        resource.gameObject.SetActive(false);
        AddedResource?.Invoke();
    }
}