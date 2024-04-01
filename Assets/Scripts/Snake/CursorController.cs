using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

namespace Snake
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField] private Transform cursor;

        private SnakeController _snakeController;
        private Camera _mainCamera;
        private Plane _plane;
        private MultiplayerManager _multiplayerManager;

        public void Init(SnakeController snakeController)
        {
            _multiplayerManager = MultiplayerManager.Instance;
            _snakeController = snakeController;
            
            _mainCamera = Camera.main;
            _plane = new Plane(Vector3.up, Vector3.zero);
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                MoveCursor();
                _snakeController.LookAt(cursor.position);
            }

            SendMove();
        }

        private void SendMove()
        {
            _snakeController.GetMoveInfo(out Vector3 position);

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
    }
}
