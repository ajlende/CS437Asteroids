using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

/*
- If the ship collides with the edge of the universe (the edge of the sphere) the ship bounces
off. You may choose how the bounce works.
- If an asteroid and ship collide, the ship is destroyed.

*/


namespace CS437
{
    internal class Spaceship
    {
        /// <summary>
        /// The spaceship model
        /// </summary>
        public Model spaceship;

        /// <summary>
        /// The spaceship texture
        /// </summary>
        private Texture2D spaceshipTexture;

        /// <summary>
        /// The effect used to render the spaceship
        /// </summary>
        private Effect spaceshipEffect;

        /// <summary>
        /// The scale of the spaceship
        /// </summary>
        private float scale;

        /// <summary>
        /// Milliseconds remaining on timer
        /// </summary>
        private int reloadTimer;

        /// <summary>
        /// Time it takes to reload in milliseconds
        /// </summary>
        public int reloadSpeed;

        /// <summary>
        /// Position in the world of the spaceship
        /// </summary>
        public Vector3 position;

        /// <summary>
        /// How far the camera should be away from the 
        /// </summary>
        public Vector3 cameraOffset { get; set; }

        /// <summary>
        /// Spaceship's pitch or rotation about the x-axis
        /// </summary>
        public float pitch { get; set; }

        /// <summary>
        /// Spaceship's yaw or rotation about the y-axis
        /// </summary>
        public float yaw { get; set; }

        /// <summary>
        /// Spaceship's roll or rotation about the z-axis
        /// </summary>
        public float roll { get; set; }

        public Quaternion quaternion
        {
            get
            {
                return Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
            }
        }

        public Vector3 forward
        {
            get
            {
                return Vector3.Transform(Vector3.Forward, quaternion);
            }
        }

        /// <summary>
        /// Constructor for a spaceship
        /// </summary>
        /// <param name="position"></param>
        /// <param name="cameraOffset"></param>
        /// <param name="scale"></param>
        /// <param name="reloadSpeed"></param>
        public Spaceship(ContentManager Content, Vector3 position, Vector3 cameraOffset, float scale = 1, int reloadSpeed = 100)
        {
            this.position = position;
            this.scale = scale;
            this.reloadSpeed = reloadSpeed;
            this.cameraOffset = cameraOffset;

            reloadTimer = reloadSpeed;
            spaceship = Content.Load<Model>("Models/spaceship");
            spaceshipTexture = Content.Load<Texture2D>("Textures/spaceship");
        }

        public void Draw(Matrix view, Matrix projection)
        {
            foreach(ModelMesh mesh in spaceship.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) * Matrix.CreateTranslation(position) * Matrix.CreateScale(scale);
                    effect.View = view;
                    effect.Projection = projection;
                    effect.TextureEnabled = true;
                    effect.Texture = spaceshipTexture;
                }

                mesh.Draw();
            }
        }

        public void Initialize()
        {

        }

        public void Update(GameTime gameTime)
        {
            if (reloadTimer > 0) reloadTimer -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public void shootTorpedo(Func<Projectile> fireTorpedo)
        {
            if (reloadTimer <= 0)
            {
                var torpedo = fireTorpedo();
                reloadTimer = reloadSpeed;
            }

        }
    }
}
