﻿using Microsoft.Xna.Framework;
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
        /// How far the camera should be away from the 
        /// </summary>
        public Vector3 CameraOffset { get; set; }

        /// <summary>
        /// Position in the world of the spaceship
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// The velocity of the spaceship
        /// </summary>
        public Vector3 Velocity { get; set; }

        /// <summary>
        /// Quaternion representing the rotation of the spaceship
        /// </summary>
        public Quaternion Orientation { get; set; }

        /// <summary>
        /// The forward vector for the spaceship
        /// </summary>
        public Vector3 Forward
        {
            get { return Vector3.Transform(Vector3.Forward, Orientation); }
        }

        /// <summary>
        /// Up vector for the spaceship
        /// </summary>
        public Vector3 Up
        {
            get { return Vector3.Transform(Vector3.Up, Orientation); }
        }

        /// <summary>
        /// Right vector for the spaceship
        /// </summary>
        public Vector3 Right
        {
            get { return Vector3.Transform(Vector3.Right, Orientation); }
        }

        /// <summary>
        /// Constructor for a spaceship
        /// </summary>
        /// <param name="position"></param>
        /// <param name="cameraOffset"></param>
        /// <param name="scale"></param>
        /// <param name="reloadSpeed"></param>
        public Spaceship(ContentManager Content,
            Vector3 Position,
            Vector3 CameraOffset,
            Func<Torpedo> fireTorpedo,
            float scale = 1,
            int reloadSpeed = 5000)
        {
            this.Position = Position;
            this.CameraOffset = CameraOffset;
            this.scale = scale;
            this.reloadSpeed = reloadSpeed;
            this.fireTorpedo = fireTorpedo;

            Orientation = Quaternion.Identity;

            reloadTimer = 0;

            spaceship = Content.Load<Model>("Models/spaceship");
            spaceshipTexture = Content.Load<Texture2D>("Textures/spaceship");
        }

        public void addRoll(float amount, float time)
        {
            amount *= time;
            Quaternion rotator = Quaternion.CreateFromAxisAngle(Forward, amount);
            Orientation = rotator * Orientation;
        }

        public void addPitch(float amount, float time)
        {
            amount *= time;
            Quaternion rotator = Quaternion.CreateFromAxisAngle(Right, amount);
            Orientation = rotator * Orientation;
        }

        public void addYaw(float amount, float time)
        {
            amount *= time;
            Quaternion rotator = Quaternion.CreateFromAxisAngle(Up, amount);
            Orientation = rotator * Orientation;
        }

        public void addThrust(float amount, float time)
        {
            Velocity += Forward * amount * time;
        }

        public void slowDown(float amount, float time)
        {
            Velocity += Vector3.Negate(Velocity) * amount * time;
        }

        /// <summary>
        /// Draw the spaceship using the given camera
        /// </summary>
        /// <param name="camera"></param>
        public void Draw(Camera camera)
        {
            // Console.WriteLine("View:       " + view);
            foreach (ModelMesh mesh in spaceship.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.CreateScale(scale)
                        * Matrix.CreateFromQuaternion(Orientation)
                        * Matrix.CreateTranslation(Position);
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.TextureEnabled = true;
                    effect.Texture = spaceshipTexture;
                }
                mesh.Draw();
            }
        }

        public void Update(GameTime gameTime, GameInput input)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //////////////////// Look ////////////////////
            addYaw(-input.xDiff * 0.1f, t);
            addPitch(-input.yDiff * 0.1f, t);
            if (input.keyboard.IsKeyDown(Keys.D))
                addRoll(0.5f, t);
            if (input.keyboard.IsKeyDown(Keys.A))
                addRoll(-0.5f, t);

            //////////////////// Move ////////////////////
            if (input.keyboard.IsKeyDown(Keys.W))
                addThrust(0.5f, t);
            if (input.keyboard.IsKeyDown(Keys.S))
                addThrust(-0.5f, t);
            if (input.keyboard.IsKeyDown(Keys.LeftShift))
                slowDown(3f, t);

            Position += Velocity * t;

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
