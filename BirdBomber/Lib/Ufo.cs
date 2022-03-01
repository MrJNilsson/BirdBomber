using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace BirdBomber.Lib
{
    public class Ufo: Sprite
    {
        public Ufo(Game _game) : base(_game)
        {
            Speed = 5;
            Texture = game.Content.Load<Texture2D>("ufo");
            Sound = game.Content.Load<SoundEffect>("laserSound");

            //PlaySound();
            Position = new Vector2(-100, 50);
        }
        public override void Update(GameTime gameTime)
        {
            Position.X += Speed;
            if (Position.X > game.GraphicsDevice.Viewport.Width + 50)
            {
                this.IsActive = false;
            }
            base.Update(gameTime);
        }
    }
}
