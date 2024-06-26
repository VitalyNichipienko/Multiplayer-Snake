using System.Collections.Generic;
using Colyseus;
using Snake;
using StaticData;
using Unity.VisualScripting;
using UnityEngine;

namespace Multiplayer
{
    public class MultiplayerManager : ColyseusManager<MultiplayerManager>
    {
        [SerializeField] private PlayerAim playerAimPrefab;
        [SerializeField] private CursorController cursorPrefab;
        [SerializeField] private SnakeController snakePrefab;
        [SerializeField] private SkinsConfig skinsConfig;
        [SerializeField] private Apple applePrefab;

        private Dictionary<Vector2Float, Apple> _apples = new Dictionary<Vector2Float, Apple>();
        private const string GameRoomName = "state_handler";
        private ColyseusRoom<State> _room;
        private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();


        public void Init()
        {
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

        public void SendMessage(string key, string data)
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

            _room.State.apples.ForEach(CreateApple);
            _room.State.apples.OnAdd += (_, apple) => CreateApple(apple);
            _room.State.apples.OnRemove += RemoveApple;
        }

        private void CreatePlayer(Player player)
        {
            Vector3 position = new Vector3(player.x, 0, player.z);
            Quaternion quaternion = Quaternion.identity;
            
            SnakeController snake = Instantiate(snakePrefab, position, quaternion);
            snake.Init(player.detailCount, skinsConfig.SkinData[player.skinIndex], true);

            PlayerAim playerAim = Instantiate(playerAimPrefab, position, quaternion);
            playerAim.Init(snake.Head, snake.MoveSpeed);
            
            CursorController cursorController = Instantiate(cursorPrefab);
            cursorController.Init(player, playerAim, snake, _room.SessionId);
        }

        private void CreateEnemy(string key, Player player)
        {            
            Vector3 position = new Vector3(player.x, 0, player.z);
            
            SnakeController snake = Instantiate(snakePrefab, position, Quaternion.identity);
            snake.Init(player.detailCount, skinsConfig.SkinData[player.skinIndex]);

            EnemyController enemyController = snake.AddComponent<EnemyController>();
            enemyController.Init(player, snake, key);
            _enemies.Add(key, enemyController);
        }

        private void RemoveEnemy(string key, Player value)
        {
            if (_enemies.ContainsKey(key) == false)
            {
                Debug.LogWarning("The specified enemy does not exist");
                return;
            }
            
            EnemyController enemyController = _enemies[key];
            _enemies.Remove(key);
            enemyController.Destroy();
        }


        private void CreateApple(Vector2Float vector2Float)
        {
            Vector3 position = new Vector3(vector2Float.x, 0, vector2Float.z);

            Apple apple = Instantiate(applePrefab, position, Quaternion.identity);
            apple.Init( vector2Float);
            
            _apples.Add(vector2Float, apple);
        }

        private void RemoveApple(int key, Vector2Float vector2Float)
        {
            if (_apples.ContainsKey(vector2Float) == false)
            {
                return;
            }

            Apple apple = _apples[vector2Float];
            _apples.Remove(vector2Float);
            apple.Destroy();
        }
    }
}
