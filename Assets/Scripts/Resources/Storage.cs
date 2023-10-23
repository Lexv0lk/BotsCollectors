using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Storage : MonoBehaviour
{
    private List<Resource> _resources = new List<Resource>();

    private void OnTriggerEnter(Collider other)
    {
        Resource resource;

        if (other.TryGetComponent(out resource))
        {
            _resources.Add(resource);
            resource.Pick(transform);
            resource.gameObject.SetActive(false);
        }
    }
}