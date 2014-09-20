using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TankGame.MenuEntities;

namespace TankGame
{
    public class CrossHair: SpriteEntity
    {
        Texture2D crossHair;
        Rectangle rec;

        public bool gunReady = true;
        float readyCount = 0;

        private MouseState mouse;

        public override void Draw(GameTime gameTime)
        {
            Game1.Instance.spriteBatch.Draw(crossHair, rec, Color.White);
        }

        public override void LoadContent()
        {
            crossHair = Game1.Instance.Content.Load<Texture2D>("Sprites/crossHair");
            this.rec = crossHair.Bounds;
        }

        public override void Update(GameTime gameTime)
        {
            if (!gunReady)
            {
                readyCount += gameTime.ElapsedGameTime.Milliseconds;
            }
            
            if ( readyCount >= 2000 )
            {
                gunReady = true;
                readyCount = 0;
            }

            mouse = Mouse.GetState();
            rec.X = Game1.Instance.GraphicsDevice.Viewport.Width / 2 - (crossHair.Width / 2);
            rec.Y = Game1.Instance.GraphicsDevice.Viewport.Height / 2 - (crossHair.Height / 2);
        }
    }
}
