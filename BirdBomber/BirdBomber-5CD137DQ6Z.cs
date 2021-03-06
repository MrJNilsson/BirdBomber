using BirdBomber.Lib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace BirdBomber
{
    public class BirdBomber : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        Random r = new Random();
        SpriteFont Font;
        int Life = 3;
        int Points = 0;

        Fighter fighter { get; set; }
        List<Shot> shots { get; set; } = new List<Shot>();
        List<Bomb> bombs { get; set; } = new List<Bomb>();
        List<Explosion> explosions { get; set; } = new();

        Texture2D Background;

        //Hur ofta man får skjuta - tiden mellan skotten
        const int Shot_delay = 300;
        int Shot_time;

        //Hur ofta bomberna ska falla
        const int Bomb_delay = 350;
        int Bomb_time;

        //Ljud
        SoundEffect laserSound;
        SoundEffect explosionSound;

        public BirdBomber()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            fighter = new Fighter(this)
            {
                Position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 20, graphics.GraphicsDevice.Viewport.Height - 60),
                Speed=5, 
            };
            base.Initialize();
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            Background = Content.Load<Texture2D>("bgspace");
            laserSound = Content.Load<SoundEffect>("laserSound");
            explosionSound = Content.Load<SoundEffect>("explosionSound");
            Font = Content.Load<SpriteFont>("Text");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState ks = Keyboard.GetState();
            
            if (ks.IsKeyDown(Keys.Left))
            {
                fighter.Position.X -= 5f;
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                fighter.Position.X += 5f;
            }
            if (ks.IsKeyDown(Keys.Enter) && Life <= 0)
            {
                Life = 3;Points = 0;
            }
            //Skotten
            Shot_time -= gameTime.ElapsedGameTime.Milliseconds; // Tiden för en loop
            if (Shot_time < 0)
            {
                Shot_time = 0;
            }

            if (ks.IsKeyDown(Keys.Space) && Shot_time == 0 && Life>0)
            {
                laserSound.Play(0.7f, 0f, 0f);
                Shot_time = Shot_delay;
                shots.Add(new Shot(this)
                {
                    Position = new Vector2(fighter.Position.X+20,fighter.Position.Y)
                });
            }
            fighter.Update(gameTime);
            shots.ForEach(e => e.Update(gameTime));
            shots.RemoveAll(e => !e.IsActive );

            //Bomber
            Bomb_time -= gameTime.ElapsedGameTime.Milliseconds;
            if (Bomb_time < 0)
            {
                Bomb_time = 0;
            }
            if (Bomb_time == 0 && Life>0)
            {

                Bomb_time = Bomb_delay;
                //Slumpa x position.
                int x = r.Next(0, graphics.GraphicsDevice.Viewport.Width - 40);
                bombs.Add(new Bomb(this)
                {
                    Position = new Vector2(x, -50)
                });
              
            }
            bombs.ForEach(e => e.Update(gameTime));
            bombs.RemoveAll(e => e.IsActive == false);

            explosions.ForEach(e => e.Update(gameTime));
            explosions.RemoveAll(e => e.IsActive == false);

            foreach(Bomb b in bombs)
            {
                if (b.Rectangle.Intersects(fighter.Rectangle))
                {
                    //Om en bomb krockar med rymdskeppet
                    explosions.Add(new Explosion(this)
                    {
                        Position = fighter.Position
                    });
                    explosionSound.Play(0.05f, 0f, 0f);
                    if(Life>0)Life -= 1;
                    b.IsActive = false;
                }
                foreach(Shot s in shots)
                {
                    if (b.Rectangle.Intersects(s.Rectangle))
                    {
                        explosions.Add(new Explosion(this)
                        {
                            Position = b.Position
                        });
                        Points += 1;
                        explosionSound.Play(0.05f, 0f, 0f);
                        b.IsActive = false;
                        s.IsActive = false;
                    }
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(Background, new Vector2(0, 0), Color.White);
            if (Life > 0)
            {
                fighter.Draw(spriteBatch);
            }
            
            shots.ForEach(e => e.Draw(spriteBatch));
            bombs.ForEach(e => e.Draw(spriteBatch));
            explosions.ForEach(e => e.Draw(spriteBatch));

            spriteBatch.DrawString(Font, "Skott: " + shots.Count, new Vector2(30, 20), Color.White);
            spriteBatch.DrawString(Font, "Bomber: " + bombs.Count, new Vector2(100, 20), Color.White);
            spriteBatch.DrawString(Font, "Liv: " + Life, new Vector2(650, 20), Color.White);
            spriteBatch.DrawString(Font, "Points: " + Points, new Vector2(700, 20), Color.White);
            if (Life == 0)
            {
                spriteBatch.DrawString(Font, "GAME OVER", new Vector2(350, 250), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
