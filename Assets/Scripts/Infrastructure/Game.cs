using Multiplayer;
using Services.Input;
using UI;

namespace Infrastructure
{
    public class Game
    {
        public static IInputService InputService;
        public readonly GameStateMachine StateMachine;
        
        public Game(ICoroutineRunner coroutineRunner, LoadingScreen loadingScreen, MultiplayerManager multiplayerManager)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadingScreen, multiplayerManager);
        }
    }
}