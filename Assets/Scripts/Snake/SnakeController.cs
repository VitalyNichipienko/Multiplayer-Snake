using StaticData;
using UnityEngine;

namespace Snake
{
    public class SnakeController : MonoBehaviour
    {
        [SerializeField] private ColorChangingDetail head;
        [SerializeField] private Tail tailPrefab;
        [SerializeField] private float moveSpeed;

        public Transform Head => head.transform;
        
        private Tail _tail;
        
        public float MoveSpeed => moveSpeed;
        
        public void Init(int detailCount, SkinData skinData, bool isPlayer = false)
        {
            int playerLayer = LayerMask.NameToLayer(Constants.PlayerLayer);
            
            if (isPlayer)
            {
                gameObject.layer = playerLayer;
                var childrenObjects = GetComponentsInChildren<Transform>();
                
                for (int i = 0; i < childrenObjects.Length; i++)
                {
                    childrenObjects[i].gameObject.layer = playerLayer;
                }
            }
            
            _tail = Instantiate(tailPrefab, transform.position, Quaternion.identity, transform);
            _tail.Init(head.transform, detailCount, skinData, playerLayer, isPlayer);
            
            head.Init(skinData.HeadColor);
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
            transform.position += head.transform.forward * moveSpeed * Time.deltaTime;
        }

        public void SetRotation(Vector3 pointToLook)
        {
            head.transform.LookAt(pointToLook);
        }
    }
}
