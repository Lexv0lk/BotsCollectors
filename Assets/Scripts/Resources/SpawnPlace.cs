using UnityEngine;

public class SpawnPlace : MonoBehaviour
{
    public Resource Resource { get; private set; }
    public bool IsOccupied { get; private set; }

    public void SpawnResource(Resource prefab)
    {
        IsOccupied = true;
        Resource = Instantiate(prefab, transform);
        Resource.Picked += OnResourcePicked;
    }

    private void OnResourcePicked(Resource resource)
    {
        IsOccupied = false;
        Resource.Picked -= OnResourcePicked;
        Resource = null;
    }
}