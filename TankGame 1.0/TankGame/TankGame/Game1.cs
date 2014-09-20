using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TankGame.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TankGame.ModelEntities;
using Camera;
using TankGame.Environment;

namespace TankGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public float volume;
        int numOfEnemies;

        public SpriteBatch spriteBatch;
        private static Game1 instance;
        public Terrain terrain;
        private LoadMenu loadMenu;
        private MainMenu mainMenu;
        private OptionsMenu optionsMenu;
        private CrossHair crossHair;
        private ModelManager modelManager;
        private CollisionType collision;
        private Vector3 oldPosition;
        

        /*Used for checking user input*/
        KeyboardState oldKeyBoard, currentKeyBoard;
        MouseState oldMouseState, currentMouseState;

        //used for player's tank
        public Matrix orientation = Matrix.Identity;

        private Bullet bullet;

        /*Stores enemy tanks*/
        Dictionary<int ,Enemy> enemy;

        /*Dictionary to store the rotation matrices of enemy tanks usiing the same indexing*/
        public Dictionary<int, Matrix> E_orientation;

        /*Stores Scenery Items*/
        Dictionary<string, GameModel> sceneItems;

        /*Model surrounding the players camera. It's position is static and used for the position of the camera also*/
        TankCabin tankCabin;

        /*Game camera*/
        GameCameraFPS myCamera;
        public Camera.Score score;
        Camera.Background background;
        Camera.HealthBar healthBar;
        Camera.GameOver gameOver;
        Camera.Minimap miniMap;

        public bool gOver;

        public static Game1 Instance
        {
            get { return instance; }
            set { instance = value;}
        }

        //used to determine the state the screen should be drawn at
        public enum currentState
        {
            gameMenu,
            gameOptions,
            gamePlaying,
            gameOver,
            gameLoad,
            gameExit
        };

        //default to be initialised
        public currentState state;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            instance = this;
  
            //set mouse pointer to be visible, changed when inGame
            instance.IsMouseVisible = true;

            graphics.IsFullScreen = false;

            //Size of playing window
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;

            Content.RootDirectory = "Content";
            graphics.PreferMultiSampling = true;

            
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
            #region instanciate menus
            mainMenu = new MainMenu();
            loadMenu = new LoadMenu();
            optionsMenu = new OptionsMenu();
            #endregion

            #region instanciate models and camera
            //Cross hair sprite appears in centre of screen where bullets are fired from
            crossHair = new CrossHair();

            //speed at which the player tank and camera move*/
            float speed = 1200.0f;
            volume = .99f;
            numOfEnemies = 50;

            tankCabin = new TankCabin(Content, speed);

            //camera taking in graphicsdevice, tank's position, viewing angle and speed
            myCamera = new GameCameraFPS(GraphicsDevice, TankCabin.Tankposition, 45, speed);

            //Terrain takes in Content manager, graphics Device, the grayscale image and an image of which to texture the 
            // terrain with
            terrain = new Terrain(Content, GraphicsDevice, "TerrainImg/heightmap", "TerrainImg/sand");

            enemy = new Dictionary<int, Enemy>();
            E_orientation = new Dictionary<int, Matrix>();
            bullet = new Bullet(Content);
            sceneItems = new Dictionary<string, GameModel>();
            score = new Camera.Score(numOfEnemies);
            background = new Camera.Background();
            healthBar = new Camera.HealthBar();
            gameOver = new Camera.GameOver();
            miniMap = new Camera.Minimap();

            gOver = false;
            #endregion


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            #region load models

            background.LoadContent();
            crossHair.LoadContent();
            score.LoadContent();
            healthBar.LoadContent();
            gameOver.LoadContent();

            

            //calls various methods to create and load vertices to graphics buffer
            terrain.LoadVertices();

            /*Used for managing model interaction, such as collision*/
            modelManager = new ModelManager(terrain.Vertices);
            modelManager.SetUpBoundaries(terrain.terrainScale, terrain.minHeight, terrain.maxHeight);
            
            /*Where the tank initially starts on map, currently set to centre*/
            TankCabin.Tankposition = new Vector3(15000, 500, -15000);


            tankCabin.Load("Models/tankCabin", TankCabin.Tankposition );

            Random number = new Random();

            modelManager.AddSphere("cabin", tankCabin.getBoundingSphere(TankCabin.Tankposition));


            for (int x = 0; x < numOfEnemies; x++)
            {
                Vector3 position = new Vector3(number.Next(5000, 90000), 500, number.Next(-90000, -5000 ));
                enemy.Add(x, new Enemy(Content, 700));
                enemy[x].Load("Models/tank_Enemy", position);
                enemy[x].LoadBones(Content);
                modelManager.AddSphere("enemy"+ x, enemy[x].getBoundingSphere(enemy[x].position));
            }

            miniMap.enemies = enemy;

            float e_height;

            foreach (KeyValuePair<int, Enemy> x in enemy)
            {
                Matrix temp = Matrix.Identity;

                e_height = enemy[x.Key].getHeightOrientation(terrain, ref temp);

                enemy[x.Key].orientation = temp;

                E_orientation.Add(x.Key, temp);

                enemy[x.Key].position.Y = e_height;

                modelManager.boundingSphereList["enemy" + x.Key] = enemy[x.Key].Bsphere;

                enemy[x.Key].collision = modelManager.CheckCollision(enemy[x.Key].Bsphere, "enemy" + x.Key);
            }

            bullet.Load("Models/cannonBall", TankCabin.Tankposition);

            #endregion

            miniMap.LoadContent();

            //load main menu sprites
            #region menu sprites
            foreach (SpriteEntity i in mainMenu.menuSprites)
            {
                i.LoadContent();
            }
            //load loadMenu sorites
            foreach (SpriteEntity i in loadMenu.menuSprites)
            {
                i.LoadContent();
            }
            //load OptionsMenu Sprites
            foreach (SpriteEntity i in optionsMenu.menuSprites)
            {
                i.LoadContent();
            }
            #endregion
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


            currentKeyBoard = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            //Update current Switch state
            switch (state)
            {
                #region gameMenu
                case (currentState.gameMenu):
                    {
                        instance.IsMouseVisible = true;
                        //update Main Menu
                        foreach (SpriteEntity i in mainMenu.menuSprites)
                        {
                            i.Update(gameTime);
                        }

                        //change state depending on menu item click
                        if (mainMenu.startGame.IsClicked)
                        {
                            if (mainMenu.startGame.IsClicked && gOver)
                            {
                                score.points = 0;
                                healthBar.health = 100;
                                gOver = false;
                                gameOver.iMusic.Stop();
                                Game1.instance.Initialize();

                            }
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
                case (currentState.gameOptions):
                    {
                        foreach (SpriteEntity i in optionsMenu.menuSprites)
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
                        foreach (SpriteEntity i in loadMenu.menuSprites)
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
                        instance.IsMouseVisible = false;

                        crossHair.Update(gameTime);
                        score.Update(gameTime);
                        background.Update(gameTime);
                        healthBar.Update(gameTime);

                        //if touching boundary box, tank remains stationary
                        if (collision == CollisionType.Boundary || collision == CollisionType.Bullet)
                        {
                            TankCabin.Tankposition = oldPosition;
                        }
                        else
                        {
                            oldPosition = TankCabin.Tankposition;
                        }

                        float height = tankCabin.getHeightOrientation(terrain, tankCabin, ref orientation);

                        TankCabin.Tankposition.Y = height + (terrain.terrainScale / 10);

                        tankCabin.Update(gameTime);

                        myCamera.Update(gameTime);

                        tankCabin.Bsphere = tankCabin.getBoundingSphere(TankCabin.Tankposition);
                        collision = modelManager.CheckCollision(tankCabin.Bsphere, "cabin");

                        modelManager.boundingSphereList["cabin"] = tankCabin.Bsphere;

                        #region update enemies

                        float e_height;


                        foreach (KeyValuePair<int, Enemy> x in enemy)
                        {
                            if (enemy[x.Key].isAlive)
                            {
                                Matrix temp = E_orientation[x.Key];

                                e_height = enemy[x.Key].getHeightOrientation(terrain, ref temp);

                                E_orientation[x.Key] = temp;

                                enemy[x.Key].position.Y = e_height - (terrain.terrainScale / 10);

                                modelManager.boundingSphereList["enemy" + x.Key] = enemy[x.Key].Bsphere;

                                enemy[x.Key].collision = modelManager.CheckCollision(enemy[x.Key].Bsphere, "enemy" + x.Key);

                            }
                                enemy[x.Key].Update(gameTime);

                                if (enemy[x.Key].collision == CollisionType.Bullet)
                                {
                                    enemy[x.Key].isAlive = false;
                                }

                                if (enemy[x.Key].reduceHealth)
                                {
                                    healthBar.health -= 5;
                                }

                        }

                        miniMap.enemies = enemy;
                        miniMap.Update(gameTime);

                        #endregion

                        if (oldMouseState.LeftButton == ButtonState.Pressed &&
                             currentMouseState.LeftButton == ButtonState.Released &&
                            crossHair.gunReady)
                        {
                            
                            
                            bullet.isAlive = true;
                            bullet.collision = CollisionType.None;
                            bullet.position = TankCabin.Tankposition + new Vector3(0, 50, 0);
                            bullet.calulateDirection(GraphicsDevice, myCamera.Projection, myCamera.View, currentMouseState);

                            tankCabin.playShotFired();
                            crossHair.gunReady = false;
                        }

                        if (bullet.isAlive)
                        {
                            bullet.UpdateBullet(gameTime);
                        }
                        
                        modelManager.boundingSphereList["bullet"] = bullet.Bsphere;

                        bullet.collision = modelManager.CheckCollision(bullet.Bsphere, "bullet");

                        if (bullet.collision == CollisionType.Object && bullet.isAlive)
                        {
                            score.points += 10;
                                
                            score.numEnemies--;
                        }

                        if (bullet.collision == CollisionType.Boundary ||
                            bullet.collision == CollisionType.Object)
                        {
                            bullet.isAlive = false;
                        }


                        if (currentKeyBoard.IsKeyDown(Keys.Escape))
                        {
                            state = currentState.gameMenu;
                        }
                        break;
                    }
                #endregion
                #region GameOver
                case (currentState.gameOver):
                    {
                        if (currentKeyBoard.IsKeyDown(Keys.Escape))
                        {
                            state = currentState.gameMenu;
                        }
                        gOver = true;
                        gameOver.Update(gameTime);
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

            oldKeyBoard = currentKeyBoard;
            oldMouseState = currentMouseState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            switch (state)
            {
                //Draw Game Menu
                #region Game Menu
                case (currentState.gameMenu):
                    {
                        foreach (SpriteEntity i in mainMenu.menuSprites)
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
                        foreach (SpriteEntity i in optionsMenu.menuSprites)
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
                        foreach (SpriteEntity i in loadMenu.menuSprites)
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
                        /*Uncomment for Wireframe*/

                        //RasterizerState rasterizerState = new RasterizerState();
                        //rasterizerState.FillMode = FillMode.WireFrame;
                        //GraphicsDevice.RasterizerState = rasterizerState;

                        background.Draw(gameTime);
                        crossHair.Draw(gameTime);
                        score.Draw(gameTime);
                        healthBar.Draw(gameTime);
                        miniMap.Draw(gameTime);

                        GraphicsDevice.BlendState = BlendState.Opaque;
                        GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                        tankCabin.Render(myCamera.View, myCamera.Projection, TankCabin.Tankposition, orientation);

                        foreach (KeyValuePair<int, Enemy> x in enemy)
                        {
                            enemy[x.Key].Render(myCamera.View, myCamera.Projection, enemy[x.Key].position, E_orientation[x.Key]);
                        }

                        terrain.Draw(gameTime, myCamera.View, myCamera.Projection);

                        if (bullet.isAlive)
                        {
                            bullet.Render(myCamera.View, myCamera.Projection, bullet.position, Matrix.Identity);
                        }

                        break;

                        
                    }
                #endregion
                //Draw gameOver
                #region gameOver
                case (currentState.gameOver):
                        {
                            gameOver.Draw(gameTime);
                            break;
                        }
                #endregion
                //Draw Exit Screen
                #region
                case (currentState.gameExit):
                        {
                            //TO DO
                            break;
                        }
                #endregion
            }
            spriteBatch.End();

            base.Draw(gameTime);


        }


    }
}
