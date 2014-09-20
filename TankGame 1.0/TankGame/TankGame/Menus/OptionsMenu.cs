using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TankGame.MenuEntities;

namespace TankGame.Menus
{
    public class OptionsMenu
    {
        private Menu background;
        public Button sound, graphics, difficulty, exit;

        public List<SpriteEntity> menuSprites = new List<SpriteEntity>();

        private static Rectangle button1 = new Rectangle(50, 50, 300, 80);
        private static Rectangle button2 = new Rectangle(50, 200, 300, 80);
        private static Rectangle button3 = new Rectangle(425, 50, 300, 80);
        private static Rectangle button4 = new Rectangle(425, 200, 300, 80);

        public OptionsMenu()
        {
            background = new Menu("OptionsMenuImg/BackGround");
            sound = new Button("OptionsMenuImg/SoundButton", "OptionsMenuImg/SoundButtonPressed", button1);
            graphics = new Button("OptionsMenuImg/GraphicsButton", "OptionsMenuImg/GraphicsPressed", button2);
            difficulty = new Button("OptionsMenuImg/DifficultyButton", "OptionsMenuImg/DifficultyPressed", button3);
            exit = new Button("OptionsMenuImg/ExitButton", "OptionsMenuImg/ExitPressed", button4);

            menuSprites.Add(background);
            menuSprites.Add(sound);
            menuSprites.Add(graphics);
            menuSprites.Add(difficulty);
            menuSprites.Add(exit);
        }


    }
}
