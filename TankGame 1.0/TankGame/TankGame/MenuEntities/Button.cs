using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankGame.MenuEntities
{
    public class Button : SpriteEntity
    {
        string buttonDefault, buttonPressed;
        Rectangle buttonSize;

        Texture2D ButtonDefault, ButtonPressed;

        private bool isClicked, isIntersect;

        public bool IsClicked
        {
            get
            {
                return isClicked;
            }
        }

        private Rectangle pixel;
        private MouseState mouse;
        private MouseState oldState;

        /// <summary>
        /// Constructor to creates button on screen
        /// </summary>
        /// <param name="buttonDefault">Sprite when button is not pressed</param>
        /// <param name="buttonPressed">Sprite when buttton is pressed</param>
        /// <param name="buttonSize">position and size of button</param>
        /// <param name="buttonText">text to appear on button</param>
        /// <param name="fontSize"> Size of text </param>
        public Button(string buttonDefault, string buttonPressed, Rectangle buttonSize)
        {
            this.buttonDefault = buttonDefault;
            this.buttonPressed = buttonPressed;
            this.buttonSize = buttonSize;
        }

        public override void LoadContent()
        {
            ButtonDefault = Game1.Instance.Content.Load<Texture2D>(buttonDefault);
            ButtonPressed = Game1.Instance.Content.Load<Texture2D>(buttonPressed);
        }

        public override void Draw(GameTime gameTime)
        {
            if (isIntersect)
            {
                Game1.Instance.spriteBatch.Draw(ButtonPressed, buttonSize, Color.White);
            }
            else
            {
                Game1.Instance.spriteBatch.Draw(ButtonDefault, buttonSize, Color.White);
            }
        }

        public override void Update(GameTime gameTime)
        {

            pixel = new Rectangle(mouse.X, mouse.Y, 1, 1);
            isClicked = checkClick(pixel, buttonSize);
            isIntersect = checkIntersect(pixel, buttonSize);
            oldState = mouse;
            mouse = Mouse.GetState();
        }

        bool checkClick(Rectangle pixel, Rectangle buttonSize)
        {

            if (pixel.Intersects(buttonSize) && ((mouse.LeftButton == ButtonState.Released) &&
                                                 (oldState.LeftButton == ButtonState.Pressed)))
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        bool checkIntersect(Rectangle pixel, Rectangle buttonRec)
        {
            if (pixel.Intersects(buttonSize))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
