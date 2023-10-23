using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 8;
    [SerializeField] private float _acceleration = 200;
    [SerializeField] private float _maxAccelerationForce = 150;

    private Rigidbody _rigidbody;
    private Vector3 _moveGoal;
    private Vector3 _goalVelocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 neededAccel = (_goalVelocity - _rigidbody.velocity) / Time.fixedDeltaTime;
        neededAccel = Vector3.ClampMagnitude(neededAccel, _maxAccelerationForce);
        _rigidbody.AddForce(new Vector3(neededAccel.x * _rigidbody.mass, 0));
    }

    public void Move(Vector3 direction)
    {
        _moveGoal = direction;
        Vector3 goalVelocity = _moveGoal * _maxSpeed;
        _goalVelocity = Vector3.MoveTowards(_goalVelocity, goalVelocity, _acceleration);
    }
}