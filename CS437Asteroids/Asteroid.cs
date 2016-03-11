using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using BulletSharp;

namespace CS437
{
    class Asteroid : Actor
    {
        public enum AsteroidSize { LARGE, MEDIUM, SMALL }

        public AsteroidSize Size;

        public Asteroid(DynamicsWorld DynamicsWorld,
            Func<string, Model> loadModel,
            Func<string, Texture2D> loadTexture,
            Vector3 position,
            AsteroidSize size)
            : base(DynamicsWorld, null, null, null, 0, 10)
        {
            Size = size;
            Body.Translate(position);

            _texture = loadTexture("Textures/seamless_rock");

            switch (size)
            {
                case AsteroidSize.LARGE:
                    Scale = 4f;
                    Mass = 120f;
                    CollisionShape = new SphereShape(12.125f);
                    _model = loadModel("Models/asteroid-large");
                    break;
                case AsteroidSize.MEDIUM:
                    Scale = 2f;
                    Mass = 60f;
                    CollisionShape = new SphereShape(3.125f);
                    _model = loadModel("Models/asteroid-medium");
                    break;
                case AsteroidSize.SMALL:
                    Scale = 1f;
                    Mass = 30f;
                    CollisionShape = new SphereShape(1.125f);
                    _model = loadModel("Models/asteroid-small");
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Asteroid Update
            base.Update(gameTime);
        }

        public override void Draw(Camera camera)
        {
            // TODO: Asteroid Draw
            foreach (ModelMesh mesh in _model.Meshes)
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
                    effect.Texture = _texture;
                }

                mesh.Draw();
            }
        }

    }
}
