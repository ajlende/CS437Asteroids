using Microsoft.Xna.Framework;
using System;

namespace CS437
{
    internal abstract class GameEntity : GameComponent, IGameComponent, IUpdateable
    {
        public Vector3 position { get; set; }
        public Quaternion rotation  { get; set; }

        public GameEntity(Game Game, Vector3? position = null, Quaternion? rotation  = null) : base(Game)
        {
            var _position = position ?? Vector3.Zero;
            var _rotation = rotation ?? Quaternion.Identity;

            this.position = _position;
            this.rotation = _rotation ;
        }

        public override abstract void Update(GameTime gameTime);
        public override abstract void Initialize();
    }
    
}