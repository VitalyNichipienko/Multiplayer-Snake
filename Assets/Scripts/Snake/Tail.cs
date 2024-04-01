using System.Collections.Generic;
using UnityEngine;

namespace Snake
{
    public class Tail : MonoBehaviour
    {
        [SerializeField] private GameObject detailPrefab;
        [SerializeField] private float detailDistance;

        private float _moveSpeed;
        private Transform _head;
        private List<Transform> _details = new List<Transform>();
        private List<Vector3> _positionHistory = new List<Vector3>();

        public void Init(Transform head, float speed, int detailCount)
        {
            _head = head;
            _moveSpeed = speed;

            _details.Add(transform);
            _positionHistory.Add(head.position);
            _positionHistory.Add(transform.position);

            SetDetailCount(detailCount);
        }

        public void Destroy()
        {
            for (int i = 0; i < _details.Count; i++)
            {
                Destroy(_details[i].gameObject);
            }
        }

        private void SetDetailCount(int detailCount)
        {
            if (detailCount == _details.Count - 1)
                return;

            int diff = (_details.Count - 1) - detailCount;

            if (diff < 1)
            {
                for (int i = 0; i < -diff; i++)
                {
                    AddDetail();
                }
            }
            else
            {
                for (int i = 0; i < diff; i++)
                {
                    RemoveDetail();
                }
            }
        }

        private void AddDetail()
        {
            Vector3 position = _details[_details.Count - 1].position;
            Transform detail = Instantiate(detailPrefab, position, Quaternion.identity).transform;
            _details.Insert(0, detail);
            _positionHistory.Add(position);
        }

        private void RemoveDetail()
        {
            if (_details.Count <= 1)
            {
                Debug.LogWarning("Detail count of snake <= 1");
                return;
            }

            Transform detail = _details[0];
            _details.Remove(detail);
            Destroy(detail.gameObject);
            _positionHistory.RemoveAt(_positionHistory.Count - 1);
        }

        private void Update()
        {
            float distance = (_head.position - _positionHistory[0]).magnitude;

            while (distance > detailDistance)
            {
                Vector3 direction = (_head.position - _positionHistory[0]).normalized;

                _positionHistory.Insert(0, _positionHistory[0] + direction * detailDistance);
                _positionHistory.RemoveAt(_positionHistory.Count - 1);

                distance -= detailDistance;
            }

            for (int i = 0; i < _details.Count; i++)
            {
                _details[i].position =
                    Vector3.Lerp(_positionHistory[i + 1], _positionHistory[i], distance / detailDistance);

                // Vector3 direction = (_positionHistory[i] - _positionHistory[i + 1]).normalized;
                // _details[i].position += direction * Time.deltaTime * _moveSpeed;
            }
        }
    }
}