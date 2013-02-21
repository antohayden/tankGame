using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankStealer.MenuEntities
{
    public class Menu: GameEntity
    {
        string MenuBackground;
        Rectangle ScreenSize = new Rectangle( 0, 0, 800, 
                                                    600);

        public Menu(string MenuBackground)
        {
            this.MenuBackground = MenuBackground;
        }
        
        public override void LoadContent()
        {
            Sprite = Game1.Instance.Content.Load<Texture2D>(MenuBackground);
        }

        public override void Draw(GameTime gameTime)
        {
            Game1.Instance.spriteBatch.Draw(Sprite, ScreenSize, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            //do nothing
        }

    }
}
