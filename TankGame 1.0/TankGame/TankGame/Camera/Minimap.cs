using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TankGame.MenuEntities;
using Microsoft.Xna.Framework.Audio;
using TankGame.ModelEntities;


namespace TankGame.Camera
{
    class Minimap : SpriteEntity
    {

        Texture2D pixel;
        Texture2D backDrop;

        Rectangle rec;
        Vector2 pixelPosition;

        string text { get; set; }


        Color color;
        float mapWidth;

        public Dictionary<int, Enemy> enemies { get; set; }

        public Minimap()
        {
            mapWidth = 227  ;

            rec = new Rectangle(0, 0, (int)mapWidth, (int)mapWidth);

            color = new Color(0, 0, 0, 150);
        }

        public override void LoadContent()
        {
            backDrop = Game1.Instance.Content.Load<Texture2D>("Sprites/mapBackground");
            pixel = Game1.Instance.Content.Load<Texture2D>("Sprites/pixel");

            position = new Vector2((TankCabin.Tankposition.X / mapWidth ) ,
                                      Math.Abs(TankCabin.Tankposition.Z / mapWidth  ));
        }

        public override void Draw(GameTime gameTime)
        {

            Game1.Instance.spriteBatch.Draw(backDrop, rec, color);
            //draw player pixel in centre
            Game1.Instance.spriteBatch.Draw(pixel, position, Color.White);

            foreach (KeyValuePair<int, Enemy> x in enemies)
            {
                if (enemies[x.Key].isAlive)
                {
                    pixelPosition = new Vector2((enemies[x.Key].position.X / (mapWidth * 2 )),
                                                    Math.Abs(enemies[x.Key].position.Z / ( mapWidth * 2) ));

                    Game1.Instance.spriteBatch.Draw(pixel, pixelPosition, Color.Red);
                }
            }

        }

        public override void Update(GameTime gameTime)
        {
            position = new Vector2(TankCabin.Tankposition.X /  ( mapWidth * 2 )  ,
                                        Math.Abs(TankCabin.Tankposition.Z / ( mapWidth * 2 ) ));
        }
    }
}
