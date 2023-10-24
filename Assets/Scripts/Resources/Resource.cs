using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Resource : MonoBehaviour
{
    [SerializeField] private float _yOffset = 1f;

    private Collider _collider;

    public event UnityAction<Resource> Picked;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Pick(Transform newHost)
    {
        Picked?.Invoke(this);
        _collider.enabled = false;
        transform.SetParent(newHost, false);
        transform.localPosition = new Vector3(0, _yOffset, 0);
    }
}