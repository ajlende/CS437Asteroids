using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

/*
 * - If the ship collides with the edge of the universe (the edge of the sphere) the ship bounces
 * off. You may choose how the bounce works.
 * - If an asteroid and ship collide, the ship is destroyed.
 */

namespace CS437
{
    internal class Spaceship
    {
        /// <summary>
        /// The spaceship model
        /// </summary>
        private Model spaceship;

        /// <summary>
        /// The spaceship texture
        /// </summary>
        private Texture2D spaceshipTexture;

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
        /// Function to be invoked when firing a projectile
        /// </summary>
        Func<Torpedo> fireTorpedo;

        /// <summary>
        /// Spaceship's pitch or rotation about the x-axis
        /// </summary>
        public float pitch;

        /// <summary>
        /// Spaceship's yaw or rotation about the y-axis
        /// </summary>
        public float yaw;

        /// <summary>
        /// Spaceship's roll or rotation about the z-axis
        /// </summary>
        public float roll;

        /// <summary>
        /// Position in the world of the spaceship
        /// </summary>
        public Vector3 position { get; set; }

        /// <summary>
        /// How far the camera should be away from the 
        /// </summary>
        public Vector3 cameraOffset { get; set; }

        /// <summary>
        /// The quaternion used for calculations
        /// </summary>
        public Quaternion quaternion
        {
            get
            {
                return Quaternion.CreateFromYawPitchRoll(yaw, pitch, roll);
            }
        }

        /// <summary>
        /// The forward vector for the spaceship
        /// </summary>
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
        public Spaceship(ContentManager Content,
            Vector3 position, Vector3 cameraOffset,
            Func<Torpedo> fireTorpedo,
            float scale = 1,
            int reloadSpeed = 5000)
        {
            this.position = position;
            this.scale = scale;
            this.reloadSpeed = reloadSpeed;
            this.cameraOffset = cameraOffset;
            this.fireTorpedo = fireTorpedo;

            reloadTimer = 0;
            spaceship = Content.Load<Model>("Models/spaceship");
            spaceshipTexture = Content.Load<Texture2D>("Textures/spaceship");
        }

        /// <summary>
        /// Draw the spaceship using the given camera
        /// </summary>
        /// <param name="camera"></param>
        public void Draw(Camera camera)
        {
            // Console.WriteLine("View:       " + view);
            foreach(ModelMesh mesh in spaceship.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) * Matrix.CreateTranslation(position) * Matrix.CreateScale(scale);
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    effect.Texture = spaceshipTexture;
                }

                mesh.Draw();
            }
        }

        public void Update(GameTime gameTime, GameInput input)
        {
            float t = (float) gameTime.ElapsedGameTime.TotalSeconds;

            //////////////////// Move and Look ////////////////////
            yaw -= input.xDiff * 0.5f * t;
            pitch += input.yDiff * 0.5f * t;

            if (input.keyboard.IsKeyDown(Keys.A))
                roll -= 3.0f * t;
            if (input.keyboard.IsKeyDown(Keys.D))
                roll += 3.0f * t;
            if (input.keyboard.IsKeyDown(Keys.W))
                position += forward * 900f * t;
            if (input.keyboard.IsKeyDown(Keys.S))
                position -= forward * 900f * t;

            //////////////////// Projectiles ////////////////////
            if (reloadTimer > 0) reloadTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if (input.keyboard.IsKeyDown(Keys.Space))
            {
                if (reloadTimer <= 0)
                {
                    fireTorpedo();
                    reloadTimer = reloadSpeed;
                }
            }
        }
    }
}
