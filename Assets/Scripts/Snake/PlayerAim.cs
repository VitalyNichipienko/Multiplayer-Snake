using UnityEngine;

namespace Snake
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed;
        
        private Vector3 _targetDirection = Vector3.zero;
        private float _speed;

        public void Init(float speed)
        {
            _speed = speed;
        }

        private void Update()
        {
            Move();
            Rotate();
        }

        public void SetTargetDirection(Vector3 pointToLook)
        {
            _targetDirection = pointToLook - transform.position;
        }
        
        public void GetMoveInfo(out Vector3 position)
        {
            position = transform.position;
        }
        
        private void Move()
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
        }

        private void Rotate()
        {
            Quaternion targetRotation = Quaternion.LookRotation(_targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
}