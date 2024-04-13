using System.Collections.Generic;
using Multiplayer;
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

        private void Update()
        {
            Move();
        }

        public void Destroy(string clientId)
        {
            DetailPositions detailPositions = _tail.GetDetailPositions();
            detailPositions.id = clientId;
            
            string json = JsonUtility.ToJson(detailPositions);
            MultiplayerManager.Instance.SendMessage("gameOver", json);
            
            _tail.Destroy();
            Destroy(gameObject);
        }

        public void SetDetailCount(int detailCount)
        {
            _tail.SetDetailCount(detailCount);
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
