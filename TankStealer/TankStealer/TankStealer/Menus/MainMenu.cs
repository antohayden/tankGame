using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TankStealer.MenuEntities;

namespace TankStealer.Menus
{
    public class MainMenu
    {

        private Menu menuBackground;
        public Button loadGame, startGame, options;
        public List<GameEntity> menuSprites = new List<GameEntity>();

        private static Rectangle button1= new Rectangle( 300, 50, 300, 80 );
        private static Rectangle button2 = new Rectangle(300, 200, 300, 80);
        private static Rectangle button3 = new Rectangle(300, 350, 300, 80);

        public MainMenu()
        {

            menuBackground =new Menu("MenuImages/BackGround");

            startGame =     new Button("MenuImages/StartButtonDefault",
                                       "MenuImages/StartButtonPressed",
                                        button1);

            loadGame =      new Button("MenuImages/LoadGameButton",
                                       "MenuImages/LoadGameButtonPressed",
                                        button2);

            options =       new Button("MenuImages/Options",
                                       "menuImages/OptionsPressed",
                                        button3);

            menuSprites.Add(menuBackground);
            menuSprites.Add(startGame);
            menuSprites.Add(loadGame);
            menuSprites.Add(options);
        }
    }
}
