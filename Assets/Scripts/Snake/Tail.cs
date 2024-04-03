using System.Collections.Generic;
using StaticData;
using UnityEngine;

namespace Snake
{
    public class Tail : MonoBehaviour
    {
        [SerializeField] private Detail detailPrefab;
        [SerializeField] private float detailDistance;
        [SerializeField] private MeshRenderer renderer;

        private Transform _head;
        private List<Transform> _details = new List<Transform>();
        private List<Vector3> _positionHistory = new List<Vector3>();
        private List<Quaternion> _rotationHistory = new List<Quaternion>();
        private SkinData _skinData;
        
        public void Init(Transform head, int detailCount, SkinData skinData)
        {
            _head = head;
            _skinData = skinData;

            _details.Add(transform);
            _positionHistory.Add(head.position);
            _rotationHistory.Add(head.rotation);
            _positionHistory.Add(transform.position);
            _rotationHistory.Add(transform.rotation);

            SetDetailCount(detailCount);
            
            renderer.material.color = skinData.TailColor;
        }

        public void Destroy()
        {
            for (int i = 0; i < _details.Count; i++)
            {
                Destroy(_details[i].gameObject);
            }
        }

        public void SetDetailCount(int detailCount)
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
            Quaternion rotation = _details[_details.Count - 1].rotation;
            Detail detail = Instantiate(detailPrefab, position, rotation, transform.parent);
            
            detail.Renderer.material.color = _skinData.DetailColor;
            
            _details.Insert(0, detail.transform);
            _positionHistory.Add(position);
            _rotationHistory.Add(rotation);
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
            _rotationHistory.RemoveAt(_rotationHistory.Count - 1);
        }

        private void Update()
        {
            float distance = (_head.position - _positionHistory[0]).magnitude;

            while (distance > detailDistance)
            {
                Vector3 direction = (_head.position - _positionHistory[0]).normalized;

                _positionHistory.Insert(0, _positionHistory[0] + direction * detailDistance);
                _positionHistory.RemoveAt(_positionHistory.Count - 1);
                
                _rotationHistory.Insert(0, _head.rotation);
                _rotationHistory.RemoveAt(_rotationHistory.Count - 1);

                distance -= detailDistance;
            }

            for (int i = 0; i < _details.Count; i++)
            {
                float percent = distance / detailDistance;
                
                _details[i].position = Vector3.Lerp(_positionHistory[i + 1], _positionHistory[i], percent);
                _details[i].rotation = Quaternion.Lerp(_rotationHistory[i + 1], _rotationHistory[i], percent);
            }
        }
    }
}