using UnityEngine;
using UnityEngine.Events;

public class SpawnPlace : MonoBehaviour
{
    public event UnityAction ResourcePicked;

    public Resource Resource { get; private set; }
    public bool IsOccupied { get; private set; }

    public void SpawnResource(Resource prefab)
    {
        IsOccupied = true;
        Resource = Instantiate(prefab, transform);
        Resource.Picked += OnResourcePicked;
    }

    private void OnResourcePicked()
    {
        IsOccupied = false;
        Resource.Picked -= OnResourcePicked;
        Resource = null;
        ResourcePicked?.Invoke();
    }
}