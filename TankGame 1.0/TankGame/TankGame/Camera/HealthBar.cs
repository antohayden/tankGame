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
    class HealthBar : SpriteEntity
    {
        public int health { get; set; }

        SpriteFont font;
        Vector2 healthText;
        Vector2 barText;

        public HealthBar()
        {
            health = 100;

            healthText = new Vector2(210, Game1.Instance.GraphicsDevice.Viewport.Height - 40);
            barText = new Vector2(300, Game1.Instance.GraphicsDevice.Viewport.Height - 40);
        }

        public override void LoadContent()
        {
            font = Game1.Instance.Content.Load<SpriteFont>("Sprites/ScoreFont");
        }

        public override void Draw(GameTime gameTime)
        {
            string bar = "";

            for (int x = 0; x < health; x++)
            {
                if (x % 5 == 0)
                    bar += "I";

                
            }
            
            Game1.Instance.spriteBatch.DrawString(font, "Health: ", healthText, Color.White);

            if (health >= 70)
            {
                Game1.Instance.spriteBatch.DrawString(font, bar, barText, Color.Green);
            }
            else if (health >= 40)
            {
                Game1.Instance.spriteBatch.DrawString(font, bar, barText, Color.Orange);
            }
            else if (health < 40)
            {
                Game1.Instance.spriteBatch.DrawString(font, bar, barText, Color.Red);
            }
            
        }

        public override void Update(GameTime gameTime)
        {
            if (health <= 0)
            {
                Game1.Instance.state = Game1.currentState.gameOver;
            }
        }
    }
}
