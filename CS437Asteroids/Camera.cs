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

        private float _fieldOfView;
        private float _aspectRatio;
        private float _nearPlaneDistance;
        private float _farPlaneDistance;

        public Matrix View
        {
            get { return Matrix.CreateLookAt(Position, Forward + Position, Up); }
        }

        public Matrix Projection
        {
            get { return Matrix.CreatePerspectiveFieldOfView(_fieldOfView, _aspectRatio, _nearPlaneDistance, _farPlaneDistance); }
        }

        public Camera(
            Vector3? position = null,
            Vector3? forward = null,
            Vector3? up = null,
            float? fieldOfView = null,
            float? nearPlaneDistance = null,
            float? farPlaneDistance = null,
            float? aspectRatio = null)
        {
            Position = position ?? Vector3.Zero;
            Forward = forward ?? Vector3.Forward;
            Up = up ?? Vector3.Up;

            _fieldOfView = fieldOfView ?? MathHelper.PiOver4;
            _nearPlaneDistance = nearPlaneDistance ?? 0.1f;
            _farPlaneDistance = farPlaneDistance ?? 10000.0f;
            _aspectRatio = aspectRatio ?? 800f / 600f;
        }

        public void Update(Actor parent, Vector3 CameraOffset, Quaternion CameraRotation)
        {
            Up = parent.Up;
            Forward = Vector3.Negate(parent.Forward);
            Up = Vector3.Transform(Up, CameraRotation);
            Forward = Vector3.Transform(Forward, CameraRotation);
            Position = parent.Position;
            Position += parent.Right * (CameraOffset.X);
            Position += parent.Up * (CameraOffset.Y);
            Position += parent.Forward * (CameraOffset.Z);

            // Up = Vector3.Normalize((Vector3.Transform(Vector3.Up, parent.CameraRotation)) + parent.Up);
            // Forward = Vector3.Normalize(Vector3.Negate(Vector3.Transform(Vector3.Forward, parent.CameraRotation)));
        }
    }
}