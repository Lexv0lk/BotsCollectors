using TMPro;
using UnityEngine;

public class StorageDisplayer : MonoBehaviour
{
    [SerializeField] private Storage _storage;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _storage.AddedResource += OnResourcesCountChanged;
        _storage.ResourcesSpent += OnResourcesCountChanged;
    }

    private void OnDisable()
    {
        _storage.AddedResource -= OnResourcesCountChanged;
        _storage.ResourcesSpent -= OnResourcesCountChanged;
    }

    private void OnResourcesCountChanged()
    {
        _text.text = _storage.ResourcesCount.ToString();
    }
}