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
using TankGame;

namespace Camera
{

    class GameCameraFPS
    {
        public Vector3 CameraReference { get; set;}
        public Vector3 CameraUpVector { get; set; }
        public Matrix rotationMatrix { get; set; }
        public Vector3 cameraForward;  

        //centre of viewport
        int centreX, centreY;

        //Projection
        float aspectRatio;
        float nearPlane;
        float farPlane;
        float fieldOfView;

        //matrices
        private Matrix view;
        private Matrix projection;

        //Read only
        public Matrix View
        {
            get
            {
                return view;
            }
        }

        public Matrix Projection
        {
            get
            {
                return projection;
            }
        }

        //Mouse
        float cameraYaw;  //rotation about the Y axis
        float cameraPitch; //rotation about the X axis
        MouseState oldMousestate;

        private GraphicsDevice device;

        //camera speed
        public float CamSpeed { get; set; }

        //constructor
        public GameCameraFPS(GraphicsDevice device, Vector3 cameraPos,
                                float fieldOfView, float camSpeed)
        {
            /* Initialize*/
            this.device = device;
            this.CamSpeed = camSpeed;

            // Projection 
            this.nearPlane = 1f;
            this.farPlane = 100000;
            this.fieldOfView = fieldOfView;
            this.aspectRatio = this.device.Viewport.AspectRatio;
            

            //view
            this.CameraUpVector = Vector3.Up;
            this.cameraForward = Vector3.Forward;

            //Mouse 
            this.cameraYaw = 0.0f;
            this.cameraPitch = 0.0f;

            //set mouse to point to middle of screen initially
            centreX = device.Viewport.Width / 2;
            centreY = device.Viewport.Height / 2;

            Mouse.SetPosition(centreX, centreY);
            oldMousestate = Mouse.GetState();

            //create view projection
            this.projection = Matrix.CreatePerspectiveFieldOfView(  MathHelper.ToRadians(this.fieldOfView),
                                                                    this.aspectRatio,
                                                                    this.nearPlane,
                                                                    this.farPlane);
        }


        public void Update(GameTime gametime)
        {

            //set KeyBoard and Mouse States
            KeyboardState keyBoardState = Keyboard.GetState();
            MouseState currentMouseState = Mouse.GetState();

            //time delta as seconds
            float timeDelta = (float)gametime.ElapsedGameTime.TotalSeconds;

            //get mouse position
            float mouseX = currentMouseState.X - oldMousestate.X;
            float mouseY = currentMouseState.Y - oldMousestate.Y;

            //Calulate Yaw and Pitch rotations
            //increase value to increase speed
            cameraYaw -= (mouseX * 0.08f) * timeDelta;
            cameraPitch -= (mouseY * 0.08f) * timeDelta;

            //clamp camera view to a range 
            cameraPitch = MathHelper.Clamp(cameraPitch, MathHelper.ToRadians(-45), MathHelper.ToRadians(45));
            cameraYaw = MathHelper.Clamp(cameraYaw, MathHelper.ToRadians(-70), MathHelper.ToRadians(70));

            //So mouse resets to centre of screen
            Mouse.SetPosition(centreX, centreY);

            

            //calculate camera rotation matrix including tank orientation
            Matrix cameraRotationMatrix = Matrix.CreateRotationX(cameraPitch)
                                        * Matrix.CreateRotationY(cameraYaw)
                                        * TankCabin.TankRotation
                                        * Game1.Instance.orientation;
                                        

            //combines vector and matrix and returns vector, used to calulate new positions
            Vector3 transformedCameraReference =
                    Vector3.Transform(cameraForward, cameraRotationMatrix);

            //new camera target
            CameraReference = transformedCameraReference + TankCabin.Tankposition;
            
            //calculate for new cameraUpvector
            Vector3 cameraRotatedUpVector = Vector3.Transform(CameraUpVector, cameraRotationMatrix);

            //assign view
            view = Matrix.CreateLookAt(TankCabin.Tankposition, CameraReference, cameraRotatedUpVector);


        }
    }
}
