using StaticData;
using UnityEngine;

namespace Snake
{
    public class SnakeController : MonoBehaviour
    {
        [SerializeField] private Transform head;
        [SerializeField] private Tail tailPrefab;
        [SerializeField] private float moveSpeed;

        private Tail _tail;
        
        public float MoveSpeed => moveSpeed;
        
        public void Init(int detailCount, SkinData skinData)
        {
            _tail = Instantiate(tailPrefab, transform.position, Quaternion.identity, transform);
            _tail.Init(head, detailCount, skinData);
            
            head.GetChild(0).GetComponent<Renderer>().material.color = skinData.HeadColor;
            _tail.transform.GetChild(0).GetComponent<Renderer>().material.color = skinData.TailColor;
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
