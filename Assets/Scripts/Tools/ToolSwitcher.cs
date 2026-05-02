using UnityEngine;
using UnityEngine.InputSystem;

namespace BoatGame
{
    public class ToolSwitcher : MonoBehaviour
    {
        [SerializeField] private Tool[] tools;

        private int _currentToolIndex = 0;
        private BoatInput _input;

        private void Awake()
        {
            _input = new BoatInput();
            _input.Enable();
            _input.BoatControls.ScrollTool.performed += OnScroll;
            
            tools[_currentToolIndex].OnActivate();
        }

        private void OnDestroy()
        {
            _input.Disable();
            _input.Dispose();
        }

        private void OnScroll(InputAction.CallbackContext ctx)
        {
            Scroll(ctx.ReadValue<float>() > 0);
        }

        private void Scroll(bool isRight)
        {
            var nextIndex = isRight ? _currentToolIndex + 1 : _currentToolIndex - 1;

            if (nextIndex < 0)
                nextIndex = tools.Length - 1;
            else if (nextIndex >= tools.Length)
                nextIndex = 0;
            
            tools[_currentToolIndex].OnDeactivate();
            
            if (nextIndex != -1)
                tools[nextIndex].OnActivate();

            _currentToolIndex = nextIndex;
        }
    }
}
