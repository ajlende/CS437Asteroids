using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CS437
{
    class Asteroid
    {
        public enum Size { LARGE, MEDIUM, SMALL }

        private Texture2D asteroidTexture;
        private Model asteroid;

        public Vector3 position { get; set; }

        public Size size;

        public float mass;

        public float scale;

        public Asteroid(ContentManager Content, Vector3 position, Size size)
        {
            this.size = size;
            this.position = position;

            asteroidTexture = Content.Load<Texture2D>("Textures/seamless_rock");

            switch (size)
            {
                case Size.LARGE:
                    scale = 20f;
                    mass = 1200f;
                    asteroid = Content.Load<Model>("Models/asteroid-large");
                    break;
                case Size.MEDIUM:
                    scale = 15f;
                    mass = 600f;
                    asteroid = Content.Load<Model>("Models/asteroid-medium");
                    break;
                case Size.SMALL:
                    scale = 10f;
                    mass = 300f;
                    asteroid = Content.Load<Model>("Models/asteroid-small");
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Asteroid Update
        }

        public void Draw(Camera camera)
        {
            // TODO: Asteroid Draw
            foreach (ModelMesh mesh in asteroid.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.FogEnabled = true;
                    effect.FogStart = 2000f;
                    effect.FogEnd = 6000f;
                    effect.SpecularColor = new Vector3(0.2f, 0.2f, 0.2f);
                    effect.AmbientLightColor = new Vector3(132f / 255f, 58 / 255f, 59 / 255f);
                    effect.DiffuseColor = new Vector3(99f / 255f, 91f / 255f, 80f / 255f);
                    effect.SpecularPower = 2;
                    effect.DirectionalLight1.Enabled = false;
                    effect.World = Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.TextureEnabled = true;
                    effect.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                    effect.Texture = asteroidTexture;
                }

                mesh.Draw();
            }
        }

    }
}
