using System;
using Microsoft.Xna.Framework;

namespace CS437
{
    internal class Camera
    {
        public Vector3 position;

        public float pitch, yaw, roll;
        public float fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance;

        public Matrix rotation
        {
            get { return Matrix.CreateFromYawPitchRoll(yaw, pitch, roll); }
        }

        public Matrix view
        {
            get { return Matrix.CreateTranslation(-position) * rotation; }
        }

        public Matrix projection
        {
            get { return Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance); }
        }

        /// <summary>
        /// Camera Constructor has all optional parameters
        /// </summary>
        /// <param name="position"></param>
        /// <param name="pitch"></param>
        /// <param name="yaw"></param>
        /// <param name="roll"></param>
        /// <param name="fieldOfView"></param>
        /// <param name="nearPlaneDistance"></param>
        /// <param name="farPlaneDistance"></param>
        /// <param name="aspectRatio"></param>
        public Camera(
            Vector3? position = null,
            float? pitch = null,
            float? yaw = null,
            float? roll = null,
            float? fieldOfView = null,
            float? nearPlaneDistance = null,
            float? farPlaneDistance = null,
            float? aspectRatio = null)
        {
            this.position = position ?? Vector3.Zero;
            this.fieldOfView = fieldOfView ?? MathHelper.PiOver4;
            this.nearPlaneDistance = nearPlaneDistance ?? 0.1f;
            this.farPlaneDistance = farPlaneDistance ?? 10000.0f;
            this.aspectRatio = aspectRatio ?? 800f / 600f;
            this.pitch = pitch ?? 0.0f;
            this.yaw = yaw ?? 0.0f;
            this.roll = roll ?? 0.0f;
        }
    }
}