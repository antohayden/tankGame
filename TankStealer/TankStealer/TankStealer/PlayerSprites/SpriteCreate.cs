using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TankStealer.PlayerSprites
{
    class SpriteCreate
    {
        public Sprite player1;

        public SpriteCreate()
        {
            player1 = new Sprite("SpriteImages/Player", 6, 10);
            player1.isAlive = true;
            player1.position = new Vector2(100, 300);
        }
        
        public void Animations()
        {
            player1.addAnimations("stationary", 0, 10);
            player1.addAnimations("run", 1, 10);
            player1.addAnimations("jump", 2, 10);
        }
    }
}
