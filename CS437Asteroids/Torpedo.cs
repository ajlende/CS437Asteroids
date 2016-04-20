using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BulletSharp;

namespace CS437
{
    internal class Torpedo : Actor
    {
        public Torpedo(DynamicsWorld DynamicsWorld, Func<string, Model> loadModel, Vector3 position, Vector3 velocity) : base(DynamicsWorld, null, null, null, GamePhysics.CollisionTypes.Torpedo, GamePhysics.CollisionTypes.Asteroid, 0, 1)
        {
            _model = loadModel("Models/alt-torpedo");
            Body.Translate(position);
            Scale = 3f;
            var radius = _model.Meshes[0].BoundingSphere.Radius;
            Mass = 1f;
            CollisionShape = new SphereShape(radius * Scale);
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Torpedo Update
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
                    effect.SpecularColor = new Vector3(0.8f, 0.8f, 1f);
                    effect.AmbientLightColor = new Vector3(0f, 0f, 0.5f);
                    effect.DiffuseColor = new Vector3(0.1f, 0f, 0.1f);
                    effect.EmissiveColor = new Vector3(0.7f, 0f, 0.7f);
                    effect.SpecularPower = 12;
                    // effect.DirectionalLight0.Enabled = false;
                    // effect.DirectionalLight1.Enabled = false;
                    // effect.DirectionalLight2.Enabled = false;
                    effect.World = Matrix.CreateScale(Scale)
                        * Matrix.CreateFromQuaternion(Orientation)
                        * Matrix.CreateTranslation(Position);
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                }
                mesh.Draw();
            }
        }
    }
}