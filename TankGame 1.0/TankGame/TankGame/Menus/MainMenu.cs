using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TankGame.MenuEntities;

namespace TankGame.Menus
{
    public class MainMenu
    {

        private Menu menuBackground;
        public Button loadGame, startGame, options;
        public List<SpriteEntity> menuSprites = new List<SpriteEntity>();

        private static Rectangle button1 = new Rectangle(300, 50, 300, 80);
        private static Rectangle button2 = new Rectangle(300, 200, 300, 80);
        private static Rectangle button3 = new Rectangle(300, 350, 300, 80);

        public MainMenu()
        {

            menuBackground = new Menu("MainMenuImg/BackGround");

            startGame = new Button("MainMenuImg/StartButtonDefault",
                                       "MainMenuImg/StartButtonPressed",
                                        button1);

            loadGame = new Button("MainMenuImg/LoadGameButton",
                                       "MainMenuImg/LoadGameButtonPressed",
                                        button2);

            options = new Button("MainMenuImg/Options",
                                       "MainMenuImg/OptionsPressed",
                                        button3);

            menuSprites.Add(menuBackground);
            menuSprites.Add(startGame);
            menuSprites.Add(loadGame);
            menuSprites.Add(options);
        }
    }
}
