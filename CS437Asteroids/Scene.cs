using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            camera = new Camera(
                position: new Vector3(0, 100, 100),
                fieldOfView: MathHelper.PiOver4,
                aspectRatio: Game.GraphicsDevice.Viewport.AspectRatio
            );
            skybox = new Skybox("Textures/Skybox", Game.Content);
            playerShip = new Spaceship(Game.Content, Vector3.Zero, new Vector3(0f, 0f, 0f), 0.05f);
            torpedos = new List<Projectile>();
            asteroids = new List<Asteroid>();
        }

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

        public void Draw(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            // RasterizerState originalRasterizerState = graphics.GraphicsDevice.RasterizerState;
            // RasterizerState rasterizerState = new RasterizerState();
            // rasterizerState.CullMode = CullMode.None;
            // graphics.GraphicsDevice.RasterizerState = rasterizerState;


            // graphics.GraphicsDevice.RasterizerState = originalRasterizerState;

            // camera.xRotation += 0.0f / gameTime.ElapsedGameTime.Milliseconds;
            camera.xRotation = MathHelper.PiOver4;
            camera.yRotation += 0.0f / gameTime.ElapsedGameTime.Milliseconds;
            camera.zRotation += 0.0f / gameTime.ElapsedGameTime.Milliseconds;
            camera.position.Z += 0.0f / gameTime.ElapsedGameTime.Milliseconds;
            // playerShip.Draw(camera.view, camera.projection);
            skybox.Draw(camera.view, camera.projection, camera.position);
        }

        public void update(GameTime gameTime)
        {
            camera.Update(gameTime);
        }
    }
}
