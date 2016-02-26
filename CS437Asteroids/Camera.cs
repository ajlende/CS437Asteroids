using System;
using Microsoft.Xna.Framework;

namespace CS437
{
    internal class Camera
    {
        public Vector3 position;
        public float xRotation, yRotation, zRotation;
        public float fieldOfView, nearPlaneDistance, farPlaneDistance, aspectRatio;

        public Matrix projection { get { return Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance); } }
        public Matrix view
        {
            get
            {
                Quaternion qx = Quaternion.CreateFromAxisAngle(new Vector3(1, 0, 0), xRotation);
                Quaternion qy = Quaternion.CreateFromAxisAngle(new Vector3(0, 1, 0), yRotation);
                Quaternion qz = Quaternion.CreateFromAxisAngle(new Vector3(0, 0, 1), zRotation);
                return Matrix.CreateTranslation(-position) * Matrix.CreateFromQuaternion(qx * qy * qz);
            }
        }

        public Camera(
            Vector3? position = null,
            float? xRotation = null,
            float? yRotation = null,
            float? zRotation = null,
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
            this.xRotation = xRotation ?? 0.0f;
            this.yRotation = yRotation ?? 0.0f;
            this.zRotation = zRotation ?? 0.0f;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}