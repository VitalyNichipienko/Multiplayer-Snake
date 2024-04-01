using UnityEngine;

namespace Snake
{
    public class SnakeController : MonoBehaviour
    {
        [SerializeField] private Transform head;
        [SerializeField] private Tail tailPrefab;
        [SerializeField] private float moveSpeed;

        private Vector3 _targetDirection = Vector3.zero;
        private Tail _tail;

        public float MoveSpeed => moveSpeed;
        
        public void Init(int detailCount)
        {
            _tail = Instantiate(tailPrefab, transform.position, Quaternion.identity, transform);
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
            Move();
        }

        private void Move()
        {
            transform.position += head.forward * moveSpeed * Time.deltaTime;
        }

        public void SetRotation(Vector3 pointToLook)
        {
            head.LookAt(pointToLook);
        }
    }
}
