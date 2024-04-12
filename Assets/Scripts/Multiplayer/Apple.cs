using System.Collections.Generic;
using Colyseus.Schema;
using UnityEngine;

namespace Multiplayer
{
    public class Apple : MonoBehaviour
    {
        private Vector2Float _apple;
        
        public void Init(Vector2Float apple)
        {
            _apple = apple;
            _apple.OnChange += OnChange;
        }

        public void Destroy()
        {
            if (_apple != null)
                _apple.OnChange -= OnChange;
            
            Destroy(gameObject);
        }

        public void Collect()
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                { "id", _apple.id }
            };
            
            MultiplayerManager.Instance.SendMessage(ColyseusKeys.Collect, data);
            
            gameObject.SetActive(false);
        }

        private void OnChange(List<DataChange> changes)
        {
            Vector3 position = transform.position;

            for (int i = 0; i < changes.Count; i++)
            {
                switch (changes[i].Field)
                {
                    case "x":
                        position.x = (float)changes[i].Value;
                        break;
                    case "z":
                        position.z = (float)changes[i].Value;
                        break;
                    default:
                        Debug.LogWarning("Apple does not respond to changes in this field" + changes[i].Field);
                        break;
                }
            }

            transform.position = position;
            gameObject.SetActive(true);
        }
    }
}