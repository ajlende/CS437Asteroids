using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CS437
{
    class Asteroid
    {
        public enum AsteroidSize { LARGE, MEDIUM, SMALL }

        private Texture2D _asteroidTexture;
        private Model _asteroid;

        public Vector3 Position { get; set; }

        public AsteroidSize Size;

        public float Mass;

        public float Scale;

        public Asteroid(ContentManager Content, Vector3 position, AsteroidSize size)
        {
            Size = size;
            Position = position;

            _asteroidTexture = Content.Load<Texture2D>("Textures/seamless_rock");

            switch (size)
            {
                case AsteroidSize.LARGE:
                    Scale = 20f;
                    Mass = 1200f;
                    _asteroid = Content.Load<Model>("Models/asteroid-large");
                    break;
                case AsteroidSize.MEDIUM:
                    Scale = 15f;
                    Mass = 600f;
                    _asteroid = Content.Load<Model>("Models/asteroid-medium");
                    break;
                case AsteroidSize.SMALL:
                    Scale = 10f;
                    Mass = 300f;
                    _asteroid = Content.Load<Model>("Models/asteroid-small");
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
            foreach (ModelMesh mesh in _asteroid.Meshes)
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
                    effect.World = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.TextureEnabled = true;
                    effect.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
                    effect.Texture = _asteroidTexture;
                }

                mesh.Draw();
            }
        }

    }
}
