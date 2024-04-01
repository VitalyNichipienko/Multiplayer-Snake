using System.Collections.Generic;
using Colyseus;
using Snake;
using UnityEngine;

namespace Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        [SerializeField] private CursorController cursorPrefab;
        [SerializeField] private SnakeController snakePrefab;
        
        private const string GameRoomName = "state_handler";
        private ColyseusRoom<State> _room;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
            InitializeClient();
            Connection();
        }

        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            LeaveRoom();
        }

        public void LeaveRoom()
        {
            _room?.Leave();
        }

        public void SendMessage(string key, Dictionary<string, object> data)
        {
            _room.Send(key, data);
        }
        
        private async void Connection()
        {
            _room = await client.JoinOrCreate<State>(GameRoomName);
            _room.OnStateChange += OnChange;
        }

        private void OnChange(State state, bool isFirstState)
        {
            if (isFirstState == false)
                return;
            
            _room.OnStateChange -= OnChange;
            
            state.players.ForEach((string key, Player player) =>
            {
                if (key == _room.SessionId)
                    CreatePlayer(player);
                else
                    CreateEnemy(key, player);
            });

            _room.State.players.OnAdd += CreateEnemy;
            _room.State.players.OnRemove += RemoveEnemy;
        }

        private void CreatePlayer(Player player)
        {
            Vector3 position = new Vector3(player.x, 0, player.z);
            SnakeController snake = Instantiate(snakePrefab, position, Quaternion.identity);
            snake.Init(player.detailCount);
            CursorController cursorController = Instantiate(cursorPrefab);
            cursorController.Init(snake);
        }

        private void CreateEnemy(string key, Player player)
        {
            throw new System.NotImplementedException();
        }

        private void RemoveEnemy(string key, Player value)
        {
            throw new System.NotImplementedException();
        }
    }
}
