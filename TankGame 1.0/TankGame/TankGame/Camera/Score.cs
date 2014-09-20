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
    public class Score: SpriteEntity
    {
        public int points { get; set; }
        public int numEnemies { get; set; }

        SpriteFont font;
        Vector2 pointsText;
        Vector2 numEnemiesText;

        public Score(int numEnemies)
        {
            points = 0;
            this.numEnemies = numEnemies;

            pointsText = new Vector2(40, Game1.Instance.GraphicsDevice.Viewport.Height - 40);
            numEnemiesText = new Vector2(Game1.Instance.GraphicsDevice.Viewport.Width - 300, Game1.Instance.GraphicsDevice.Viewport.Height - 40);

        }

        public override void LoadContent()
        {
            font = Game1.Instance.Content.Load<SpriteFont>("Sprites/ScoreFont");
        }

        public override void Draw(GameTime gameTime)
        {
            Game1.Instance.spriteBatch.DrawString(font, "Score: " + points, pointsText, Color.White);
            Game1.Instance.spriteBatch.DrawString(font, "Enemies Remaining: " + numEnemies, numEnemiesText, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            
        }


    }
}
