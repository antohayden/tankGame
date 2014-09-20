using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

/*TO DO: Correct error for screen shots, purple screen*/

namespace TankGame
{
    static class Utilities
    {
        private static Texture2D ssTexture;
        private static int counter;
        
        private static KeyboardState currentS, previousS;

        public static void TakeScreenShot(GraphicsDevice device, Keys theKey)
        {

            currentS = Keyboard.GetState();

            if (currentS.IsKeyDown(theKey) && previousS.IsKeyUp(theKey))
            {
                Stream stream;

                //width and height of backbuffer
                int w = device.PresentationParameters.BackBufferWidth;
                int h = device.PresentationParameters.BackBufferHeight;

                //create array to take in and hold data
                int[] backBuffer = new int[w * h];
                device.GetBackBufferData(backBuffer);

                //create new instance and assign data
                ssTexture = new Texture2D(device, w, h, false, device.PresentationParameters.BackBufferFormat);
                ssTexture.SetData<int>(backBuffer);

                string name = "SS" + counter + ".png";

                //if name exists
                if (File.Exists(name))
                {
                    counter++;
                    name = "SS" + counter + ".png";
                }

                //create IO stream for file create
                stream = new FileStream(name, FileMode.Create);

                //save as PNG 
                ssTexture.SaveAsPng(stream, w, h);

                stream.Close();
                ssTexture.Dispose();
                
                
                
            }

            previousS = currentS;

        }
    }
}
