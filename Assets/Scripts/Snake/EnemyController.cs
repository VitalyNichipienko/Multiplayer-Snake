using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

namespace Snake
{
    public class EnemyController : MonoBehaviour
    {
        private Player _player;
        private SnakeController _snakeController;
        
        public void Init(Player player, SnakeController snake)
        {
            _player = player;
            _snakeController = snake;
            
            player.OnChange += OnChange;
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

        public void Destroy()
        {
            _player.OnChange -= OnChange;
        }
    }
}
