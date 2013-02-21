using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TankStealer.MenuEntities;

namespace TankStealer.Menus
{
    public class LoadMenu
    {
        private Menu backGround;
        public Button loadGame1, loadGame2, loadGame3, exit;

        public List<GameEntity> menuSprites = new List<GameEntity>();

        private static Rectangle button1= new Rectangle( 100, 50, 600, 80 );
        private static Rectangle button2 = new Rectangle(100, 150, 600, 80);
        private static Rectangle button3 = new Rectangle(100, 250, 600, 80);
        private static Rectangle button4 = new Rectangle(350, 350, 150, 80);

        public LoadMenu()
        {

            backGround =    new Menu("LMenuImages/BackGround");

            loadGame1 =     new Button("LMenuImages/LoadGame1",
                                       "LMenuImages/LoadGame1Hover",
                                        button1);

            loadGame2 =     new Button("LMenuImages/LoadGame2",
                                        "LMenuImages/LoadGame2Hover",
                                        button2);

            loadGame3 =     new Button("LMenuImages/LoadGame3",
                                         "LmenuImages/LoadGame3Hover",
                                        button3);

            exit =          new Button("LMenuImages/Return",
                                        "LMenuImages/ReturnHover",
                                        button4);

            menuSprites.Add(backGround);
            menuSprites.Add(loadGame1);
            menuSprites.Add(loadGame2);
            menuSprites.Add(loadGame3);
            menuSprites.Add(exit);
        }
    }
}
