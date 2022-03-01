using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BirdBomber.Lib
{
    public class Bomb:Sprite
    {
        public Bomb(Game game) : base(game)
        {
            Speed = 5;
            Texture = game.Content.Load<Texture2D>("bomb");
        }
        public override void Update(GameTime gameTime)
        {
            Position.Y += Speed;
            //Om bomberna faller förbi
            if (Position.Y > game.GraphicsDevice.Viewport.Height+20) this.IsActive = false;
            base.Update(gameTime);
        }

    }
}
