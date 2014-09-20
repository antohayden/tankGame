using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TankGame.ModelEntities;
using TankGame.Environment;

 namespace TankGame.ModelEntities
 {

    class TankCabin: GameModel
    {

        public static Vector3 tankForward;
        public static Vector3 Tankposition;
        public static Matrix TankRotation;
        bool moving { get;  set; }
        private float TankSpeed { get; set;}

        bool noKeyPressed;
        SoundEffect tankMove, tankIdle, fireShot;
        SoundEffectInstance iTankMove, iTankIdle, iFireShot;
        float volumeControlLower, volumeControlUpper;

        public TankCabin(ContentManager content, float speed)
            :base(content)
        {
            this.moving = false;
            this.TankSpeed = speed;
            UpVector = Vector3.Up;
            tankForward = Vector3.Forward;
            target = tankForward + position;
            
            volumeControlLower = 0.01f;
            volumeControlUpper = Game1.Instance.volume;

            tankMove = content.Load<SoundEffect>("Sounds/TankMoving");
            tankIdle = content.Load<SoundEffect>("Sounds/TankIdle");
            fireShot = content.Load<SoundEffect>("Sounds/CannonBlast");

            iFireShot = fireShot.CreateInstance();
            iTankMove = tankMove.CreateInstance();
            iTankIdle = tankIdle.CreateInstance();

            iFireShot.Volume = volumeControlUpper;
            iFireShot.IsLooped = false;

            iTankIdle.Volume = volumeControlUpper;
            iTankMove.Volume = volumeControlLower;


            noKeyPressed = true;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyBoardState = Keyboard.GetState();

            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector3 moveVector = Vector3.Zero;

            float moveFactor = TankSpeed * timeDelta;

            if (keyBoardState.IsKeyDown(Keys.W))
            {
                noKeyPressed = false;

                moveVector.Z -= moveFactor;

                playMoving();
            }

            if (keyBoardState.IsKeyDown(Keys.S))
            {
                noKeyPressed = false;

                playMoving();

                moveVector.Z += moveFactor;
            }
            if (keyBoardState.IsKeyDown(Keys.A))
            {
                noKeyPressed = false;

                playMoving();

                rotationY += ( moveFactor / 10 );
            }
            if (keyBoardState.IsKeyDown(Keys.D))
            {
                noKeyPressed = false;

                playMoving();

                rotationY -= ( moveFactor / 10 );
            }

            if (noKeyPressed)
            {
                playIdle();
                if ( iTankMove.Volume < 0.02f)
                    iTankMove.Pause();
            }

            else
            {
                iTankIdle.Pause();
            }

            TankRotation = Matrix.CreateRotationY(MathHelper.ToRadians(rotationY));

            Tankposition += Vector3.Transform(moveVector, TankRotation);

            noKeyPressed = true;

        }



        void playMoving()
        {
            iTankMove.Play();

            if (iTankMove.Volume < volumeControlUpper)
            {
                if (iTankMove.Volume < volumeControlUpper)
                    iTankMove.Volume += 0.01f;

                if (iTankIdle.Volume > volumeControlLower)
                    iTankIdle.Volume -= 0.01f;
            }
        }

        void playIdle()
        {
            iTankIdle.Play();

            if (iTankMove.Volume > volumeControlLower)
                iTankMove.Volume -= 0.01f;

            if (iTankIdle.Volume < volumeControlUpper)
                iTankIdle.Volume += 0.01f;
          
        }

        public void playShotFired()
        {
            if (iFireShot.State != SoundState.Playing)
                iFireShot.Play();
            else
            {
                iFireShot.Stop();
                iFireShot.Play();
            }
        }
    }


}
