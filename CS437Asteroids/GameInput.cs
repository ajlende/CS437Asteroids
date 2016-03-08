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
        public Vector2 mouseDelta;

        public GameInput(GraphicsDevice graphicsDevice)
        {
            device = graphicsDevice;
            mouseDelta = Vector2.Zero;
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
                mouseDelta.X = newMouse.Position.X - oldMouse.Position.X;
                mouseDelta.Y = newMouse.Position.Y - oldMouse.Position.Y;
            }
            else
            {
                mouseDelta = Vector2.Zero;
            }

            Mouse.SetPosition(device.Viewport.Width / 2, device.Viewport.Height / 2);
        }
    }
}
