using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    public class Tail : MonoBehaviour
    {
        [SerializeField] private SnakeController snakeController;
        [SerializeField] private Transform head;
        [SerializeField] private List<Transform> details;
        [SerializeField] private float detailDistance;
        
        private List<Vector3> _positionHistory = new List<Vector3>();

        private void Awake()
        {
            _positionHistory.Add(head.position);

            for (int i = 0; i < details.Count; i++)
            {
                _positionHistory.Add(details[i].position);
            }
        }

        private void Update()
        {
            float distance = (head.position - _positionHistory[0]).magnitude;

            while (distance > detailDistance)
            {
                Vector3 direction = (head.position - _positionHistory[0]).normalized;
                
                _positionHistory.Insert(0, _positionHistory[0] + direction * detailDistance);
                _positionHistory.RemoveAt(_positionHistory.Count - 1);

                distance -= detailDistance;
            }

            for (int i = 0; i < details.Count; i++)
            {
                details[i].position = Vector3.Lerp(_positionHistory[i + 1], _positionHistory[i], distance / detailDistance);

                Vector3 direction = (_positionHistory[i] - _positionHistory[i + 1]).normalized;

                details[i].position += direction * Time.deltaTime * snakeController.MoveSpeed;
            }
        }
    }
}
