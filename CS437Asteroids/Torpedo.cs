using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CS437
{
    internal class Torpedo
    {
        private Model torpedo;
        private float scale = 10f;

        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }

        public Torpedo(ContentManager Content, Vector3 Position, Vector3 Velocity)
        {
            this.Position = Position;
            this.Velocity = Velocity;
            torpedo = Content.Load<Model>("Models/alt-torpedo");
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Torpedo Update
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * t;
        }

        public void Draw(Camera camera)
        {
            // TODO: Asteroid Draw
            foreach (ModelMesh mesh in torpedo.Meshes)
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
                    effect.World = Matrix.CreateScale(scale) * Matrix.CreateTranslation(Position);
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                }

                mesh.Draw();
            }
        }
    }
}