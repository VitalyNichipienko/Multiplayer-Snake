using UnityEngine;

namespace Services.Input
{
    public class MobileInputService : InputService
    {
        public override Vector2 Axis => GetSimpleInputAxis();

        public override bool IsCursorButtonDown => Axis != Vector2.zero;
        
        private static Vector2 GetSimpleInputAxis() =>
            new(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
    }
}