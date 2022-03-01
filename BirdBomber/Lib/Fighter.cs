using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace BirdBomber.Lib
{
    public class Fighter:Sprite
    {
        public Fighter(Game game):base(game) {
            Texture = game.Content.Load<Texture2D>("ship");
        }

        public override void Update(GameTime gameTime)
        {
            if (Position.X < -40)
            {
                Position.X = game.GraphicsDevice.Viewport.Width + 40;
            }
            if (Position.X > game.GraphicsDevice.Viewport.Width + 40)
            {
                Position.X = -40;
            }
            base.Update(gameTime);
        }

    }
}
