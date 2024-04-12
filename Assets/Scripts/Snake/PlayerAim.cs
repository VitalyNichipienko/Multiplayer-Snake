using Multiplayer;
using UnityEngine;

namespace Snake
{
    public class PlayerAim : MonoBehaviour
    {
        [SerializeField] private LayerMask collisionLayer;
        [SerializeField] private float overlapRadius;
        [SerializeField] private float rotateSpeed;

        private Transform _snakeHead;
        private Vector3 _targetDirection = Vector3.zero;
        private float _speed;

        public void Init(Transform snakeHead, float speed)
        {
            _snakeHead = snakeHead;
            _speed = speed;
        }

        private void Update()
        {
            Move();
            Rotate();
        }

        private void FixedUpdate()
        {
            CheckCollision();
        }

        private void CheckCollision()
        {
            Collider[] colliders = Physics.OverlapSphere(_snakeHead.position, overlapRadius, collisionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].TryGetComponent(out Apple apple))
                {
                    apple.Collect();
                }
                else
                {
                    Defeat();
                }
            }
        }

        private void Defeat()
        {
            FindObjectOfType<CursorController>()?.Destroy();
            
            Destroy(gameObject);
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