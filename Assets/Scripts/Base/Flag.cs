using UnityEngine;

public class Flag : MonoBehaviour, ITarget
{
    [SerializeField] private Base _basePrefab;

    public void CreateBase(Unit creator)
    {
        Base newBase = Instantiate(_basePrefab);
        newBase.transform.position = new Vector3(transform.position.x, newBase.transform.position.y, transform.position.z);
        newBase.AddUnit(creator);
        Destroy(gameObject);
    }
}