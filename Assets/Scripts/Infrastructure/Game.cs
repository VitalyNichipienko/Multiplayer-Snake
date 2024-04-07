using Services.Input;
using UnityEngine;

namespace Infrastructure
{
    public class Game
    {
        public static IInputService InputService;

        public Game()
        {
            RegisterInputService();
        }

        private static void RegisterInputService()
        {
#if UNITY_EDITOR || PLATFORM_STANDALONE
            InputService = new StandaloneInputService();
#elif UNITY_ANDROID
            InputService = new MobileInputService();
#endif
        }
    }
}