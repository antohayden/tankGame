using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TankGame.MenuEntities;

namespace TankGame.Camera
{
    class Background: SpriteEntity
    {
        Texture2D background = new Texture2D(Game1.Instance.GraphicsDevice,
                                             Game1.Instance.GraphicsDevice.Viewport.Width,
                                             70);

        public Background()
        {
            position = new Vector2(0, Game1.Instance.GraphicsDevice.Viewport.Height - 70);
        }

        public override void LoadContent()
        {
            background = Game1.Instance.Content.Load<Texture2D>("Sprites/barBackground");
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Game1.Instance.spriteBatch.Draw(background, position, Color.Black);
        }
    }
}
