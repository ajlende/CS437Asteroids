using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CS437
{
    class GameInput
    {
        private KeyboardState _originalKeyboard;
        private GamePadState _originalGamepad;
        private MouseState _originalMouse;

        private KeyboardState _currentKeyboard;
        private GamePadState _currentGamepad;
        private MouseState _currentMouse;

        private GraphicsDevice _device;

        public KeyboardState KeyboardState;
        public Vector2 MouseDelta;

        public GameInput(GraphicsDevice graphicsDevice)
        {
            _device = graphicsDevice;
            MouseDelta = Vector2.Zero;
        }

        public void Initialize()
        {
            Mouse.SetPosition(_device.Viewport.Width / 2, _device.Viewport.Height / 2);

            _originalKeyboard = Keyboard.GetState();
            _originalMouse = Mouse.GetState();
            _originalGamepad = GamePad.GetState(PlayerIndex.One);
        }

        public void PollInput()
        {
            _currentKeyboard = Keyboard.GetState();
            _currentMouse = Mouse.GetState();
            _currentGamepad = GamePad.GetState(PlayerIndex.One);

            KeyboardState = _currentKeyboard;

            if (_currentMouse != _originalMouse)
            {
                MouseDelta.X = _currentMouse.Position.X - _originalMouse.Position.X;
                MouseDelta.Y = _currentMouse.Position.Y - _originalMouse.Position.Y;
            }
            else
            {
                MouseDelta = Vector2.Zero;
            }

            Mouse.SetPosition(_originalMouse.Position.X, _originalMouse.Position.Y);
        }
    }
}
