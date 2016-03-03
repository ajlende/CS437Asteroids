using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CS437
{
    class Scene
    {
        private Camera camera;
        private Skybox skybox;

        private Spaceship playerShip;
        private List<Torpedo> torpedos;

        private List<Asteroid> asteroids;

        public Scene(Game Game)
        {
            camera = new Camera(fieldOfView: MathHelper.PiOver4,
                aspectRatio: Game.GraphicsDevice.Viewport.AspectRatio);

            skybox = new Skybox(Game.Content, "Textures/Skybox");

            torpedos = new List<Torpedo>();

            asteroids = new List<Asteroid>();

            playerShip = new Spaceship(Game.Content,
                Position: new Vector3(0.0f, 0.0f, 0.0f),
                CameraOffset: new Vector3(0.0f, 108f, 35f),
                scale: 0.5f,
                fireTorpedo: () =>
                {
                    var torpedo = new Torpedo(Game, Vector3.Zero);
                    torpedos.Add(torpedo);
                    Console.WriteLine("BOOM! <" + torpedos.Count + ">");
                    return torpedo;
                });
        }

        public void update(GameTime gameTime, GameInput input)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            playerShip.Update(gameTime, input);

            foreach (var asteroid in asteroids)
            {
                asteroid.Update(gameTime);
            }

            foreach (var torpedo in torpedos)
            {
                torpedo.Update(gameTime);
            }

            camera.Position = playerShip.Position;
            camera.Up = playerShip.Up;
            camera.Forward = playerShip.Forward;

        }

        public void Draw(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            // Console.WriteLine("Ship:   " + playerShip.position);
            // Console.WriteLine("Camera: " + camera.position);

            var view = camera.View;
            var projection = camera.Projection;

            // Console.WriteLine("View:       " + camera.view);
            // Console.WriteLine("Projection: " + camera.projection);

            ////////////////////////// SkyBox //////////////////////////
            RasterizerState originalRasterizerState = graphics.GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphics.GraphicsDevice.RasterizerState = rasterizerState;

            skybox.Draw(camera.View, camera.Projection, camera.Position);

            graphics.GraphicsDevice.RasterizerState = originalRasterizerState;

            ////////////////////////// Ship //////////////////////////
            playerShip.Draw(camera);

            ////////////////////////// Asteroids //////////////////////////
            foreach (var asteroid in asteroids)
            {
                asteroid.Draw(camera);
            }
            
            ////////////////////////// Torpedos //////////////////////////
            foreach (var torpedo in torpedos)
            {
                torpedo.Draw(camera);
            }
        }
    }
}
