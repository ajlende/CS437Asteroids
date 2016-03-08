using System;
using Microsoft.Xna.Framework;

namespace CS437
{
    internal class Camera
    {
        public Vector3 Position { get; set; }

        private Vector3 _forward;
        public Vector3 Forward
        {
            get { return _forward; }
            set { var f = value; f.Normalize(); _forward = f; }
        }

        private Vector3 _up;
        public Vector3 Up
        {
            get { return _up; }
            set { var u = value; u.Normalize(); _up = u; }
        }

        private float fieldOfView;
        private float aspectRatio;
        private float nearPlaneDistance;
        private float farPlaneDistance;

        public Matrix View
        {
            get { return Matrix.CreateLookAt(Position, Forward + Position, Up); }
        }

        public Matrix Projection
        {
            get { return Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance); }
        }

        public Camera(
            Vector3? Position = null,
            Vector3? Forward = null,
            Vector3? Up = null,
            float? fieldOfView = null,
            float? nearPlaneDistance = null,
            float? farPlaneDistance = null,
            float? aspectRatio = null)
        {
            this.Position = Position ?? Vector3.Zero;
            this.Forward = Forward ?? Vector3.Forward;
            this.Up = Up ?? Vector3.Up;
            this.fieldOfView = fieldOfView ?? MathHelper.PiOver4;
            this.nearPlaneDistance = nearPlaneDistance ?? 0.1f;
            this.farPlaneDistance = farPlaneDistance ?? 10000.0f;
            this.aspectRatio = aspectRatio ?? 800f / 600f;
        }

        public void Update(Spaceship playerShip)
        {
            Position = playerShip.Position;
            Position += playerShip.Right * (playerShip.CameraOffset.X);
            Position += playerShip.Up * (playerShip.CameraOffset.Y);
            Position += playerShip.Forward * (playerShip.CameraOffset.Z);
            Up = playerShip.Up;
            Forward = Vector3.Negate(playerShip.Forward);
            Matrix cameraRotation = Matrix.CreateFromAxisAngle(playerShip.Right, MathHelper.Pi / 8f);
            Up = Vector3.Transform(Up, cameraRotation);
            Forward = Vector3.Transform(Forward, cameraRotation);
        }
    }
}