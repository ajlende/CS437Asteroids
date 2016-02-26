using System;
using Microsoft.Xna.Framework;

namespace CS437
{
    internal abstract class DrawableGameEntity : GameEntity
    {
        public float scale { get; set; }

        public DrawableGameEntity(Game Game, Vector3? position = null, Quaternion? rotation = null, float scale = 1) : base(Game, position, rotation)
        {
            this.scale = scale;
        }
         
        public abstract void Draw(GameTime gameTime);
    }
}