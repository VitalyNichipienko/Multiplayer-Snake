using System;
using Services;
using Services.Input;

namespace Infrastructure
{
    public class BootstrapState : IState
    {
        public void Enter()
        {
            RegistryServices();
        }

        public void Exit()
        {
            
        }

        public IInputService SetupInputService()
        {
            return default;
        }

        private void RegistryServices()
        {
            throw new NotImplementedException();
        }
    }
}