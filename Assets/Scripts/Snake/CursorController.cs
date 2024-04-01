using System.Collections.Generic;
using Colyseus.Schema;
using Logic;
using Multiplayer;
using Unity.VisualScripting;
using UnityEngine;

namespace Snake
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField] private float cameraOffsetY;
        [SerializeField] private Transform cursor;

        private SnakeController _snakeController;
        private Camera _mainCamera;
        private Plane _plane;
        private MultiplayerManager _multiplayerManager;
        private Player _player;
        private PlayerAim _playerAim;

        public void Init(Player player, PlayerAim aim, SnakeController snakeController)
        {
            _player = player;
            _playerAim = aim;
            _snakeController = snakeController;
            _multiplayerManager = MultiplayerManager.Instance;

            _mainCamera = Camera.main;
            _plane = new Plane(Vector3.up, Vector3.zero);

            snakeController.AddComponent<CameraManager>().Init(cameraOffsetY);

            _player.OnChange += OnChange;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                MoveCursor();
                _playerAim.SetTargetDirection(cursor.position);
            }

            SendMove();
        }

        private void SendMove()
        {
            _playerAim.GetMoveInfo(out Vector3 position);

            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "x", position.x }, { "z", position.z }
            };

            _multiplayerManager.SendMessage("move", data);
        }

        private void MoveCursor()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            _plane.Raycast(ray, out float distance);
            Vector3 point = ray.GetPoint(distance);

            cursor.position = point;
        }
        
        private void OnChange(List<DataChange> changes)
        {
            Vector3 position = _snakeController.transform.position;
            
            for (int i = 0; i < changes.Count; i++)
            {
                switch (changes[i].Field)
                {
                    case "x" :
                        position.x = (float)changes[i].Value;
                        break;
                    case "z" : 
                        position.z = (float)changes[i].Value;
                        break;
                    case "detailCount" : 
                        _snakeController.SetDetailCount((byte)changes[i].Value);
                        break;
                    default:
                        Debug.LogWarning("Field is not processed" + changes[i].Field);
                        break;
                }
            }
            
            _snakeController.SetRotation(position);
        }
    }
}
