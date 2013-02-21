using System;
using System.Collections.Generic;
using System.Linq;
using TankStealer.Menus;
using TankStealer.PlayerSprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TankStealer
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        private static Game1 instance;
        private LoadMenu loadMenu;
        private MainMenu mainMenu;
        private OptionsMenu optionsMenu;
        private SpriteCreate sprite;

        
        public enum currentState
        {
            gameMenu,
            gameOptions,
            gamePlaying,
            gamePaused,
            gameLoad,
            gameExit
        };

        public currentState state;

        public static Game1 Instance
        {
            get
            {
                return instance;
            }

            set
            {
                instance = value;
            }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            instance = this;
            instance.IsMouseVisible = true;

            mainMenu = new MainMenu();
            loadMenu = new LoadMenu();
            optionsMenu = new OptionsMenu();
            sprite = new SpriteCreate();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();

            

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load main menu sprites
            foreach (GameEntity i in mainMenu.menuSprites)
            {
                i.LoadContent();
            }
            //load loadMenu sorites
            foreach (GameEntity i in loadMenu.menuSprites)
            {
                i.LoadContent();
            }
            //load OptionsMenu Sprites
            foreach (GameEntity i in optionsMenu.menuSprites)
            {
                i.LoadContent();
            }
            //load player Sprite
            sprite.player1.LoadContent();
            sprite.Animations();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //Update current Switch state
            switch ( state )
            {
                #region gameMenu
                case ( currentState.gameMenu):
                    {
                        //update Main Menu
                        foreach (GameEntity i in mainMenu.menuSprites)
                        {
                             i.Update(gameTime);
                        }

                        //change state depending on menu item click
                        if (mainMenu.startGame.IsClicked)
                        {
                            state = currentState.gamePlaying;
                        }
                        else if (mainMenu.options.IsClicked)
                        {
                            state = currentState.gameOptions;
                        }
                        else if (mainMenu.loadGame.IsClicked)
                        {
                            state = currentState.gameLoad;
                        }
                        break;
                    }
                #endregion
                #region Options Menu
                case ( currentState.gameOptions):
                    {
                        foreach (GameEntity i in optionsMenu.menuSprites)
                        {
                            i.Update(gameTime);
                        }

                        if (optionsMenu.exit.IsClicked)
                        {
                            state = currentState.gameMenu;
                        }
                        break;
                    }
                #endregion
                #region Load Menu
                case (currentState.gameLoad):
                    {
                        foreach (GameEntity i in loadMenu.menuSprites)
                        {
                            i.Update(gameTime);
                        }

                        if (loadMenu.exit.IsClicked)
                        {
                            state = currentState.gameMenu;
                        }
                        break;
                    }
                #endregion
                #region Playing
                case (currentState.gamePlaying):
                        {
                            sprite.player1.Update(gameTime);
                            break;
                        }
                #endregion
                #region Paused
                case (currentState.gamePaused):
                        {
                            //TO DO
                            break;
                        }
                #endregion
                #region Exit
                case (currentState.gameExit):
                        {
                            //TODO
                            break;
                        }
                #endregion

            }

            base.Update(gameTime);
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            switch (state)
            {
                //Draw Game Menu
                #region Game Menu
                case (currentState.gameMenu):
                    {
                        foreach (GameEntity i in mainMenu.menuSprites)
                        {
                            i.Draw(gameTime);
                        }
                        break;
                    }
                #endregion
                //Draw options Menu
                #region Options Menu
                case (currentState.gameOptions):
                    {
                        foreach (GameEntity i in optionsMenu.menuSprites)
                        {
                            i.Draw(gameTime);
                        }
                        break;
                    }
                #endregion
                //Draw Load Game Menu
                #region Load Game Menu
                case (currentState.gameLoad):
                    {
                        foreach (GameEntity i in loadMenu.menuSprites)
                        {
                            i.Draw(gameTime);
                        }
                        break;
                    }
                #endregion
                //Draw GamePlay
                #region GamePlaying
                case (currentState.gamePlaying):
                        {
                            sprite.player1.action = "stationary";
                            sprite.player1.Draw(gameTime);
                            break;
                        }
                #endregion
            }

            base.Draw(gameTime);

            spriteBatch.End();
        }
    }
}
