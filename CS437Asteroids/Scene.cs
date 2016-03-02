using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CS437
{
    class Scene
    {
        private Camera camera { get; set; }
        private Skybox skybox { get; }

        private Spaceship playerShip { get; set; }
        private List<Projectile> torpedos { get; set; }

        private List<Asteroid> asteroids { get; set; }

        private Game game { get; set; }

        public Scene(Game Game)
        {
            camera = new Camera(fieldOfView: MathHelper.PiOver4,
                aspectRatio: Game.GraphicsDevice.Viewport.AspectRatio);

            skybox = new Skybox(Game.Content, "Textures/Skybox");

            playerShip = new Spaceship(Game.Content,
                position: new Vector3(0.0f, 0.0f, 0.0f),
                cameraOffset: new Vector3(0.0f, 108f, 35f),
                scale: 0.5f);

            torpedos = new List<Projectile>();

            asteroids = new List<Asteroid>();
        }

        /// <summary>
        /// TODO: Delete this method because it's just a template that should be in each draw call for various objects
        /// </summary>
        /// <param name="model"></param>
        /// <param name="position"></param>
        /// <param name="forward"></param>
        /// <param name="up"></param>
        private void DrawModel(Model model, Vector3 position, Vector3 forward, Vector3 up)
        {
            Matrix world = Matrix.CreateWorld(position, forward, up);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = world;
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                }

                mesh.Draw();
            }
        }

        public void update(GameTime gameTime, GameInput input)
        {
            float td = (float)gameTime.ElapsedGameTime.TotalSeconds;

            playerShip.yaw += input.xDiff * 3.0f * td;
            playerShip.pitch += input.yDiff * 3.0f * td;

            if (input.keyboard.IsKeyDown(Keys.A))
                playerShip.roll += 3.0f * td;
            if (input.keyboard.IsKeyDown(Keys.D))
                playerShip.roll -= 3.0f * td;
            if (input.keyboard.IsKeyDown(Keys.W))
                playerShip.position += playerShip.forward * 900f * td;
            if (input.keyboard.IsKeyDown(Keys.S))
                playerShip.position -= playerShip.forward * 900f * td;

            camera.position = playerShip.position;
        }

        public void Draw(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            ////////////////////////// SkyBox //////////////////////////
            RasterizerState originalRasterizerState = graphics.GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphics.GraphicsDevice.RasterizerState = rasterizerState;

            skybox.Draw(camera.view, camera.projection, camera.position);

            graphics.GraphicsDevice.RasterizerState = originalRasterizerState;

            ////////////////////////// Ship //////////////////////////
            playerShip.Draw(camera.view, camera.projection);


            ////////////////////////// Asteroids //////////////////////////


            ////////////////////////// Torpedos //////////////////////////


        }
    }
}
