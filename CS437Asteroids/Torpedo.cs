using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CS437
{
    internal class Torpedo : Projectile
    {
        private Model model;
        // Effect glow;

        public Torpedo(Game game, Vector3 position) : base(game, position)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            model = Game.Content.Load<Model>("Models/torpedo");
            // glow = Game.Content.Load<Effect>("Effects/glow");
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}