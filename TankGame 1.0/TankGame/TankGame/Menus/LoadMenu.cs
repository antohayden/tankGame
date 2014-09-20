using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TankGame.MenuEntities;

namespace TankGame.Menus
{
    public class LoadMenu
    {
        private Menu backGround;
        public Button loadGame1, loadGame2, loadGame3, exit;

        public List<SpriteEntity> menuSprites = new List<SpriteEntity>();

        private static Rectangle button1 = new Rectangle(100, 50, 600, 80);
        private static Rectangle button2 = new Rectangle(100, 150, 600, 80);
        private static Rectangle button3 = new Rectangle(100, 250, 600, 80);
        private static Rectangle button4 = new Rectangle(350, 350, 150, 80);

        public LoadMenu()
        {

            backGround = new Menu("LoadMenuImg/BackGround");

            loadGame1 = new Button("LoadMenuImg/LoadGame1",
                                       "LoadMenuImg/LoadGame1Hover",
                                        button1);

            loadGame2 = new Button("LoadMenuImg/LoadGame2",
                                        "LoadMenuImg/LoadGame2Hover",
                                        button2);

            loadGame3 = new Button("LoadMenuImg/LoadGame3",
                                         "LoadMenuImg/LoadGame3Hover",
                                        button3);

            exit = new Button("LoadMenuImg/Return",
                                        "LoadMenuImg/ReturnHover",
                                        button4);

            menuSprites.Add(backGround);
            menuSprites.Add(loadGame1);
            menuSprites.Add(loadGame2);
            menuSprites.Add(loadGame3);
            menuSprites.Add(exit);
        }
    }
}
