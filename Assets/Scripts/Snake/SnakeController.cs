using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    private Vector3 _targetDirection = Vector3.zero;

    private void Update()
    {
        Rotate();
        Move();
    }

    private void Rotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_targetDirection);
        float maxAngle = rotateSpeed * Time.deltaTime;
        head.rotation = Quaternion.RotateTowards(head.rotation, targetRotation, maxAngle);
    }

    private void Move()
    {
        transform.position += head.forward * moveSpeed * Time.deltaTime;
    }

    public void LookAt(Vector3 cursorPosition)
    {
        _targetDirection = cursorPosition - head.position;
    }
}
