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
        
        private const string GameRoomName = "state_handler";
        private ColyseusRoom<State> _room;
        private Dictionary<string, EnemyController> _enemies = new Dictionary<string, EnemyController>();


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
            Quaternion quaternion = Quaternion.identity;
            
            SnakeController snake = Instantiate(snakePrefab, position, quaternion);
            snake.Init(player.detailCount, skinsConfig.SkinData[player.skinIndex]);

            PlayerAim playerAim = Instantiate(playerAimPrefab, position, quaternion);
            playerAim.Init(snake.MoveSpeed);
            
            CursorController cursorController = Instantiate(cursorPrefab);
            cursorController.Init(player, playerAim, snake);
        }

        private void CreateEnemy(string key, Player player)
        {            
            Vector3 position = new Vector3(player.x, 0, player.z);
            
            SnakeController snake = Instantiate(snakePrefab, position, Quaternion.identity);
            snake.Init(player.detailCount, skinsConfig.SkinData[player.skinIndex]);

            EnemyController enemyController = snake.AddComponent<EnemyController>();
            enemyController.Init(player, snake);
            
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
    }
}
