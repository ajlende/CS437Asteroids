using System;
using Microsoft.Xna.Framework;

namespace CS437
{
    class Asteroid
    {
        public enum Size { LARGE, MEDIUM, SMALL }

        public Size size;

        public Vector3 position { get; set; }

        public float mass
        {
            get
            {
                switch (size)
                {
                    case Size.LARGE: return 1024f;
                    case Size.MEDIUM: return 512f;
                    case Size.SMALL: return 256f;
                }
                return 0f;
            }
        }

        public Asteroid(Vector3 position, Size size)
        {

        }

        public void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
