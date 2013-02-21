using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankStealer.PlayerSprites
{
   public class Sprite: GameEntity
    {
       int maxRows;
       int maxFrames;
       int FrameIndex;
       int time = 0;

       private string _spriteSheet;
       private Texture2D spriteSheet;
       public string action;

       public Dictionary<string, Rectangle[]> spriteFrames;
       

       /// <summary>
       /// 
       /// </summary>
       /// <param name="sprite">Sprite Sheet</param>
       /// <param name="cols">Number of columns in Sprite Sheet</param>
       /// <param name="maxFrames">Maximum number of frames in Sprite Sheet</param>
       public Sprite(string _spriteSheet, int maxRows, int maxFrames)
       {
           this._spriteSheet = _spriteSheet;
           this.maxRows = maxRows;
           this.maxFrames = maxFrames;
           this.FrameIndex = 0;
           spriteFrames = new Dictionary<string, Rectangle[]>();
           
       }

       public void addAnimations(string action, int row, int frames)
       {
           Rectangle[] recs = new Rectangle[frames];

           for (int i = 0; i < frames; i++)
           {
               recs[i] = new Rectangle((spriteSheet.Width / maxFrames) * i, (spriteSheet.Height / maxRows) * row,
                                            spriteSheet.Width/ maxFrames, spriteSheet.Height / maxRows);
           }

           spriteFrames.Add(action, recs);
           
       }

       public override void LoadContent()
       {
           spriteSheet = Game1.Instance.Content.Load<Texture2D>(_spriteSheet);
       }

       public override void Update(GameTime gameTime)
       {
           
           time += gameTime.ElapsedGameTime.Milliseconds;

           //updating at 10 fps
           if (time > 100)
           {
               FrameIndex++;
               //Loops animations
               if (FrameIndex >= maxFrames)
               {
                   FrameIndex = FrameIndex % maxFrames;
               }
               time = 0;
           }
       }

       public override void Draw(GameTime gameTime)
       {

           Game1.Instance.spriteBatch.Draw(spriteSheet, position, spriteFrames[action][FrameIndex],
               Color.White);
       }
    }
}
