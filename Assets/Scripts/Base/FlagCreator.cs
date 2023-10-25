using UnityEngine;
using UnityEngine.Events;

public class FlagCreator : MonoBehaviour
{
    public static FlagCreator Instance { get; private set; }

    [SerializeField] private Flag _flagPrefab;

    public event UnityAction FlagCreated;

    public Flag CurrentFlag { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (CurrentFlag != null)
                {
                    CurrentFlag.transform.position = hit.point;
                }
                else
                {
                    CurrentFlag = Instantiate(_flagPrefab, hit.point, Quaternion.identity);
                    CurrentFlag.transform.position = new Vector3(CurrentFlag.transform.position.x, CurrentFlag.transform.position.y + CurrentFlag.transform.localScale.y, CurrentFlag.gameObject.transform.position.z);
                    FlagCreated?.Invoke();
                }
            }
        }
    }
}
