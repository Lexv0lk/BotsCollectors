using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Storage : MonoBehaviour
{
    public event UnityAction AddedResource;
    public event UnityAction ResourcesSpent;

    public uint ResourcesCount { get; private set; } = 0;

    public void SendResource(Resource resource)
    {
        ResourcesCount++;
        Destroy(resource.gameObject);
        AddedResource?.Invoke();
    }

    public void SpendResources(int count)
    {
        ResourcesCount -= (uint)count;
        ResourcesSpent?.Invoke();
    }
}