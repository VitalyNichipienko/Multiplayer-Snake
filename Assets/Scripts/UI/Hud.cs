using UnityEngine;

namespace UI
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private GameObject joystickArea;

        private void Awake()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            joystickArea.SetActive(false);
#endif
        }
    }
}
