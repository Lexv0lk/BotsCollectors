using TMPro;
using UnityEngine;

public class StorageDisplayer : MonoBehaviour
{
    [SerializeField] private Storage _storage;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _storage.AddedResource += OnResourceAdded;
    }

    private void OnDisable()
    {
        _storage.AddedResource -= OnResourceAdded;
    }

    private void OnResourceAdded()
    {
        _text.text = _storage.ResourcesCount.ToString();
    }
}