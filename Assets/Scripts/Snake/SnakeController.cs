using UnityEngine;

namespace Snake
{
    public class SnakeController : MonoBehaviour
    {
        [SerializeField] private Transform head;
        [SerializeField] private Tail tailPrefab;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotateSpeed;

        private Vector3 _targetDirection = Vector3.zero;
        private Tail _tail;

        public void Init(int detailCount)
        {
            _tail = Instantiate(tailPrefab, transform.position, Quaternion.identity);
            _tail.Init(head, moveSpeed, detailCount);
        }

        public void SetDetailCount(int detailCount)
        {
            _tail.SetDetailCount(detailCount);
        }

        public void Destroy()
        {
            _tail.Destroy();
            Destroy(gameObject);
        }
        
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

        public void SetRotation(Vector3 pointToLook)
        {
            head.LookAt(pointToLook);
        }
        
        public void LerpRotation(Vector3 cursorPosition)
        {
            _targetDirection = cursorPosition - head.position;
        }

        public void GetMoveInfo(out Vector3 position)
        {
            position = transform.position;
        }
    }
}
