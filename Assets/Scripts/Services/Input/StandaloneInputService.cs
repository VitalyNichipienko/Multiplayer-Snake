using UnityEngine;

namespace Services.Input
{
    public class StandaloneInputService : InputService
    {
        public override Vector2 Axis => UnityEngine.Input.mousePosition;

        public override bool IsCursorButtonDown =>
            SimpleInput.GetKey(KeyCode.Mouse0); // UnityEngine.Input.GetMouseButton(0);
    }
}