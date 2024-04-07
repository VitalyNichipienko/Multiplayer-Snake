using Multiplayer;
using UI;
using UnityEngine;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private MultiplayerManager multiplayerManager;
        [SerializeField] private LoadingScreen loadingScreen;
        
        private Game _game;

        private void Awake()
        {
            _game = new Game(this, loadingScreen, multiplayerManager);
            _game.StateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }
    }
}