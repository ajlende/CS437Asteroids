using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CS437
{
    class GameInput
    {
        private KeyboardState oldKeyboard;
        private GamePadState oldGamepad;
        private MouseState oldMouse;

        private KeyboardState newKeyboard;
        private GamePadState newGamepad;
        private MouseState newMouse;

        private GraphicsDevice device;

        public KeyboardState keyboard;
        public float xDiff { get; private set; }
        public float yDiff { get; private set; }

        public GameInput(GraphicsDevice graphicsDevice)
        {
            device = graphicsDevice;
            xDiff = 0f;
            yDiff = 0f;
        }

        public void Initialize()
        {
            Mouse.SetPosition(device.Viewport.Width / 2, device.Viewport.Height / 2);

            oldKeyboard = Keyboard.GetState();
            oldMouse = Mouse.GetState();
            oldGamepad = GamePad.GetState(PlayerIndex.One);
        }

        public void pollInput()
        {
            newKeyboard = Keyboard.GetState();
            newMouse = Mouse.GetState();
            newGamepad = GamePad.GetState(PlayerIndex.One);

            keyboard = newKeyboard;

            if (newMouse != oldMouse)
            {
                xDiff = newMouse.Position.X - oldMouse.Position.X;
                yDiff = newMouse.Position.Y - oldMouse.Position.Y;
            }
            else
            {
                xDiff = 0f;
                yDiff = 0f; 
            }

            Mouse.SetPosition(device.Viewport.Width / 2, device.Viewport.Height / 2);
        }
    }
}
