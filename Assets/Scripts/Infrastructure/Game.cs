using Services.Input;
using UI;
using UnityEngine;

namespace Infrastructure
{
    public class Game
    {
        public static IInputService InputService;
        public readonly GameStateMachine StateMachine;
        
        public Game(ICoroutineRunner coroutineRunner, LoadingScreen loadingScreen)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadingScreen);
        }
    }
}