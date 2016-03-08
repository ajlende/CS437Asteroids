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

        public Asteroid(ContentManager Content, Vector3 position, Size size)
        {
            this.size = size;
            this.position = position;

            asteroidTexture = Content.Load<Texture2D>("Textures/seamless_rock");

            switch (size)
            {
                case Size.LARGE:
                    mass = 1200f;
                    asteroid = Content.Load<Model>("Models/asteroid-large");
                    break;
                case Size.MEDIUM:
                    mass = 600f;
                    asteroid = Content.Load<Model>("Models/asteroid-medium");
                    break;
                case Size.SMALL:
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
            foreach(ModelMesh mesh in asteroid.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.SpecularPower = 100f;
                    effect.World = Matrix.CreateScale(10f) * Matrix.CreateTranslation(position);
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
