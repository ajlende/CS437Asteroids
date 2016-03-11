using BulletSharp;
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
    internal class Spaceship : Actor
    {
        private int _reloadTimer;

        public int ReloadSpeed;

        public Func<Torpedo> FireTorpedo;

        public Vector3 CameraOffset { get; set; }
        public Quaternion CameraRotation { get; set; }

        public Spaceship(DynamicsWorld DynamicsWorld,
            Func<string, Model> loadModel,
            Func<string, Texture2D> loadTexture,
            Vector3 cameraOffset,
            Quaternion cameraRotation,
            Func<Torpedo> fireTorpedo,
            float scale = 1f,
            int reloadSpeed = 1000) : 
            base(DynamicsWorld, null, null, null, scale, 1f)
        {
            CameraOffset = cameraOffset;
            CameraRotation = cameraRotation;
            CollisionShape = new BoxShape(24.5f/2, 5f/2, 15f/2);

            FireTorpedo = fireTorpedo;
            ReloadSpeed = reloadSpeed;
            _reloadTimer = 0;

            _model = loadModel("Models/spaceship");
            _texture = loadTexture("Textures/spaceship");
        }

        public void addRoll(float amount, float time)
        {
            Body.AngularVelocity += Forward * amount * time;
        }

        public void addPitch(float amount, float time)
        {
            Body.AngularVelocity += Right * amount * time;
        }

        public void addYaw(float amount, float time)
        {
            Body.AngularVelocity += Up * amount * time;
        }

        public void addThrust(float amount, float time)
        {
            Body.ApplyCentralForce(Forward * amount * time);
        }

        public void slowDown(float amount, float time)
        {
            Body.ApplyCentralForce(Vector3.Negate(Body.LinearVelocity) * amount * time);
        }

        public void Update(GameTime gameTime, GameInput input)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Body.Activate();

            //////////////////// Look ////////////////////
            Body.AngularVelocity = Vector3.Zero;
            addYaw(-input.MouseDelta.X * 1.5f, t);
            addPitch(input.MouseDelta.Y * 1.5f, t);
            if (input.KeyboardState.IsKeyDown(Keys.D))
                addRoll(-30f, t);
            if (input.KeyboardState.IsKeyDown(Keys.A))
                addRoll(30f, t);

            // Vector2 scaledMouse = input.MouseDelta * (sensitivity * smoothing * t);
            // _smoothMouse = Vector2.Lerp(_smoothMouse, scaledMouse, 1f / smoothing);
            // _mouseAbsolute += _smoothMouse;
            // _mouseAbsolute = Vector2.Clamp(_mouseAbsolute, clampInDegrees * -0.5f, clampInDegrees * 0.5f);
            // Quaternion xrotator = Quaternion.CreateFromAxisAngle(Up, -_mouseAbsolute.X);
            // Orientation = xrotator * Orientation;
            // Quaternion yrotator = Quaternion.CreateFromAxisAngle(Right, _mouseAbsolute.Y);
            // Orientation = yrotator * Orientation;

            //////////////////// Move ////////////////////
            if (input.KeyboardState.IsKeyDown(Keys.W))
                addThrust(-5000f, t);
            if (input.KeyboardState.IsKeyDown(Keys.S))
                addThrust(5000f, t);
            if (input.KeyboardState.IsKeyDown(Keys.LeftShift))
                slowDown(200f, t);

            //////////////////// Projectiles ////////////////////
            if (_reloadTimer > 0) _reloadTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if (input.KeyboardState.IsKeyDown(Keys.Space))
            {
                if (_reloadTimer <= 0)
                {
                    FireTorpedo();
                    _reloadTimer = ReloadSpeed;
                }
            }
        }

        public override void Draw(Camera camera)
        {
            base.Draw(camera);
        }
    }
}
