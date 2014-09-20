using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using TankGame.MenuEntities;
using Microsoft.Xna.Framework.Audio;
namespace TankGame.Camera
{
    public class GameOver : SpriteEntity
    {

        SpriteFont font, font2;
        Texture2D backDrop;

        SoundEffect music;
        public SoundEffectInstance iMusic;
        Rectangle rec;
        Vector2 textPosition;
        string text { get; set; }

        string path = @"\scores.txt";
        bool written;

        float colorAlpha;

        public GameOver()
        {
            rec = new Rectangle(0, 0, Game1.Instance.GraphicsDevice.DisplayMode.Width,
                                    Game1.Instance.GraphicsDevice.DisplayMode.Height);

            textPosition = new Vector2(250, 100);
            colorAlpha = 0;
        }

        public override void LoadContent()
        {
            backDrop = Game1.Instance.Content.Load<Texture2D>("Sprites/Sunset");
            font = Game1.Instance.Content.Load<SpriteFont>("Sprites/GameOver");
            font2 = Game1.Instance.Content.Load<SpriteFont>("Sprites/ScoreFont");
            music = Game1.Instance.Content.Load<SoundEffect>("Sounds/GameOver");
            iMusic = music.CreateInstance();
            iMusic.Volume = Game1.Instance.volume;

            if (!System.IO.File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("");
                }
            }

            written = false;

        }

        public override void Draw(GameTime gameTime)
        {
            Game1.Instance.spriteBatch.Draw(backDrop, rec, Color.White);
            Game1.Instance.spriteBatch.DrawString(font, "Game Over", textPosition, new Color(colorAlpha, 0, 0, colorAlpha));
            Game1.Instance.spriteBatch.DrawString(font, "Score: " + Game1.Instance.score.points, textPosition + new Vector2(30, 150), new Color(colorAlpha, 0, 0, colorAlpha));
            Game1.Instance.spriteBatch.DrawString(font2, "press Esc...", new Vector2(200, 560), Color.White);
            Game1.Instance.spriteBatch.DrawString(font2, "Top Scores", new Vector2(600, 300), Color.Red);

            using (StreamReader sr = File.OpenText(path))
            {
                int positionY = 300;

                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    string readScore = s;
                    Game1.Instance.spriteBatch.DrawString(font2, s, new Vector2(500, positionY), Color.White);
                    positionY += 50;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!written)
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    if (Game1.Instance.score.points != 0)
                    {
                        string score = ("" + Game1.Instance.score.points + "    " + DateTime.Now);
                        sw.WriteLine(score);
                    }
                }
            }
            written = true;

            iMusic.Play();

            if (colorAlpha < 255)
            {
                colorAlpha += 0.001f;
            }

        }
    }
}
