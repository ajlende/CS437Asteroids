using System;
using Microsoft.Xna.Framework;

namespace CS437
{
    internal abstract class Projectile : DrawableGameEntity
    {
        public Projectile(Game Game, Vector3 position) : base(Game, position)
        {
        }
    }
}