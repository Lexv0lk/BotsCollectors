using UnityEngine;

public class CameraMover : MonoBehaviour
{
    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 delta = new Vector2(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            Vector3 movement = new Vector3(delta.x, 0, delta.y);

            transform.position += movement;
        }
    }
}
