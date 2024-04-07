using System;
using Services;
using Services.Input;

namespace Infrastructure
{
    public class BootstrapState : IState
    {
        private const string InitialSceneName = "Initial";
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            RegistryServices();
            _sceneLoader.Load(InitialSceneName, EnterLoadLevel);
        }

        private void EnterLoadLevel() => 
            _gameStateMachine.Enter<LoadLevelState, string>("Main");

        public void Exit()
        {
            
        }

        private void RegistryServices()
        {
            Game.InputService = RegisterInputService();
        }

        private static IInputService RegisterInputService()
        {
#if UNITY_EDITOR || PLATFORM_STANDALONE
            return new StandaloneInputService();
#elif UNITY_ANDROID
            return new MobileInputService();
#endif
        }
    }
}