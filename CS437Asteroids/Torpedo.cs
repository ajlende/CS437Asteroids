using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CS437
{
    internal class Torpedo
    {
        private Game game;

        private Model model;
        // Effect glow;

        public Vector3 position { get; set; }

        public Torpedo(Game game, Vector3 position)
        {
            this.game = game;
            this.position = position;
        }

        public void Initialize()
        {
            model = game.Content.Load<Model>("Models/torpedo");
            // glow = Game.Content.Load<Effect>("Effects/glow");
        }

        public void Update(GameTime gameTime)
        {
            // TODO: Torpedo Update
        }

        public void Draw(Camera camera)
        {
            // TODO: Torpedo Draw
        }
    }
}