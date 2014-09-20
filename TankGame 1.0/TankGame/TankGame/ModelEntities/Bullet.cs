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
using TankGame.Environment;
using TankGame.ModelEntities;

namespace TankGame.ModelEntities
{
    class Bullet: GameModel
    {
        public CollisionType collision;
        public Vector3 direction;
        Matrix orientation = Matrix.Identity;

          public Bullet(ContentManager content)
            :base(content)
        {
            //size of bullet
            scale = 50;
            speed = 300;
            collision = CollisionType.None;
            isAlive = true;
        }

          public void UpdateBullet(GameTime gameTime)
          {
              
              float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
              
              //increase it's direction by speed
              Vector3 moveVector = direction * speed;

              //add this to it's current position
              position += moveVector;
              //Create bounding sphere based upon position
              Bsphere = getBoundingSphere(position);

              //get height of object
              float height = getHeightOrientation(Game1.Instance.terrain, ref orientation);

              //Checking if a bullet has hit the terrain. If so, we can remove it
              if (position.Y + (Game1.Instance.terrain.terrainScale) <= height &&
                  isAlive)
              {
                  collision = CollisionType.Boundary;
                  isAlive = false;
              }
              else
              {
                  collision = CollisionType.None;
                  isAlive = true;
              }

          }
          

          /*Method to calculate the direction the bullet is going in based upon mouse pointer position*/

          public void calulateDirection(GraphicsDevice device, Matrix projection, Matrix view, MouseState mouseState)
          {
              // create 2 positions in screenspace using the cursor position. 0 is as
              // close as possible to the camera, 1 is as far away as possible.
              Vector3 nearSource = new Vector3(mouseState.X, mouseState.Y, 0f);
              Vector3 farSource = new Vector3(mouseState.X, mouseState.Y, 1f);

              // use Viewport.Unproject to tell what those two screen space positions
              // would be in world space. we'll need the projection matrix and view
              // matrix, which we have saved as member variables. We also need a world
              // matrix, which can just be identity.
              Vector3 nearPoint = device.Viewport.Unproject(nearSource,
                  projection, view, Matrix.Identity);

              Vector3 farPoint = device.Viewport.Unproject(farSource,
                  projection, view, Matrix.Identity);

              // find the direction vector that goes from the nearPoint to the farPoint
              // and normalize it....
              direction = farPoint - nearPoint;
              direction.Normalize();
          }

        


    }
}
