using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TankStealer.MenuEntities;

namespace TankStealer.Menus
{
    public class OptionsMenu
    {
        private Menu background;
        public Button sound, graphics, difficulty, exit;

        public List<GameEntity> menuSprites = new List<GameEntity>();

        private static Rectangle button1 = new Rectangle(50, 50, 300, 80);
        private static Rectangle button2 = new Rectangle(50, 200, 300, 80);
        private static Rectangle button3 = new Rectangle(425, 50, 300, 80);
        private static Rectangle button4 = new Rectangle(425, 200, 300, 80);

        public OptionsMenu()
        {
            background = new Menu("OMenuImgs/BackGround");
            sound = new Button("OMenuImgs/SoundButton", "OMenuImgs/SoundButtonPressed", button1);
            graphics = new Button("OMenuImgs/GraphicsButton", "OMenuImgs/GraphicsPressed", button2);
            difficulty = new Button("OMenuImgs/DifficultyButton", "OmenuImgs/DifficultyPressed", button3);
            exit = new Button("OMenuImgs/ExitButton", "OMenuImgs/ExitPressed", button4);

            menuSprites.Add(background);
            menuSprites.Add(sound);
            menuSprites.Add(graphics);
            menuSprites.Add(difficulty);
            menuSprites.Add(exit);
        }
        

    }
}
