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

//Some code taken from Microsoft Example "Tank on HeightMap" along with model tank

namespace TankGame.ModelEntities
{
    class Enemy:GameModel
    {
         enum Turning
         {
             clockwise,
             anticlockwise,
             straightAhead
         };


         enum Range
         {
             followRange,
             fireRange,
             outOf
         };

         Range range;

         Matrix rotation;
         public Matrix orientation { get; set;}
         int counter;
         int counter2;

         Turning turning;
         public CollisionType collision;
         Random rand;
         int num;
         float rotateAmount;

         List<Vector3> explosionDirections;
         List<Vector3> explodedPositions;
         int index;

         float explodeTimer;
         float explodeDuration;
         float explodePivot;
         float explosionForce;
         int explodeRotation;
         bool hasExploded;

         SoundEffect tankExplode, fireShot;
         SoundEffectInstance iTankExplode, iFireShot;
         AudioEmitter emitter = new AudioEmitter();
         AudioListener listener = new AudioListener();
         
         //if tank is no longer alive and has exploded it should be removed from list of tanks
         public bool removable;

         // we'll use this value when making the wheels roll. It's calculated based on 
         // the distance moved.
         Matrix wheelRollMatrixForward = Matrix.Identity;
         Matrix wheelRollMatrixBack = Matrix.Identity;

         const float TankWheelRadius = 100;

         ModelBone leftBackWheelBone;
         ModelBone rightBackWheelBone;
         ModelBone leftFrontWheelBone;
         ModelBone rightFrontWheelBone;

         Matrix leftBackWheelTransform;
         Matrix rightBackWheelTransform;
         Matrix leftFrontWheelTransform;
         Matrix rightFrontWheelTransform;

         Vector3 direction;

         public Vector3 playerBulletDirection { get; set; }

         public bool hasFired;
         public int firingTimer;
         public bool reduceHealth;


        //constructor with intial values
        public Enemy(ContentManager content, float speed)
            :base(content)
        {
            UpVector = Vector3.Up;
            target = Vector3.Forward;
            this.scale = .5f;
            this.speed = 700;
            this.counter = 0;
            collision = CollisionType.None;
            rand = new Random(10);
            num = rand.Next();

            //list to contain random directions the meshes will travel
            explosionDirections = new List<Vector3>();
            //list to contain the position of each mesh as it travels
            explodedPositions = new List<Vector3>();

            //used to set the tank to be removed from list after a certain time
            explodeTimer = 0;
            //how long the exploding takes
            explodeDuration = 3;
            //how fast explosion takes
            explosionForce = 25;
            //Used to change the direction to downward
            explodePivot = explodeDuration / 2;
            //used to index the lists
            index = 0;
            //rotate pieces of tanks as explodes
            explodeRotation = 1;
            //check if the meshes should switch to falling
            hasExploded = false;
            //sound to play when exploding
            tankExplode = Game1.Instance.Content.Load<SoundEffect>("Sounds/Explode");
            //create instance
            iTankExplode = tankExplode.CreateInstance();
            //set to not loop
            iTankExplode.IsLooped = false;
            //set to true when tank had died
            removable = false;
            //Initilise direction which will be used to adjust to follow player
            direction = Vector3.Zero;
            //wheter the enemy has fired
            hasFired = false;
            //how long in between firing
            firingTimer = 0;
            //Shot when shooting
            fireShot = Game1.Instance.Content.Load<SoundEffect>("Sounds/CannonBlast");
            //instance of sound
            iFireShot = fireShot.CreateInstance();
            iFireShot.IsLooped = false;
            //used to decide hweter to take player health away
            reduceHealth = false;
        }

        
        public override void Update(GameTime gameTime)
        {

            if ((position - TankCabin.Tankposition).Length() < 4000)
            {
                range = Range.fireRange;
            }
            else if ((position - TankCabin.Tankposition).Length() < 15000)
            {
                range = Range.followRange;
            }
            else
                range = Range.outOf;



            Bsphere = getBoundingSphere(position);

            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector3 moveVector = Vector3.Zero;

            float moveFactor = speed * timeDelta;


            #region AI tank movement
            /*If required to change direction will stop movement and turn 180 degrees
             * clockwise
             * else, turning 30 degrees clockwise whilst moving forward*/
            if (isAlive)
            {
                switch (collision)
                {

                    //If enemy hits the edge of the terrain it rotates clockwise
                    case (CollisionType.Boundary):
                        {
                            counter2 += 1;

                            turning = Turning.clockwise;

                            rotationY -= 1;

                            if (counter2 >= 90)
                            {
                                if (counter2 >= 220)
                                    counter2 = 0;
                                num = rand.Next(1, 10);
                            }
                            moveVector.Z += moveFactor;

                            break;
                        }

                    //if it encounters an object it will turn clockwise or
                    //anticlockwise randomly
                    case (CollisionType.Object):
                        {
                            if (num % 2 == 0)
                            {
                                rotationY -= 1;
                                turning = Turning.clockwise;
                            }
                            else
                            {
                                turning = Turning.anticlockwise;
                                rotationY += 1;
                            }

                            moveVector.Z += moveFactor;
                            break;
                        }

                    case (CollisionType.Bullet):
                        {
                            break;
                        }

                    
                    default:
                        {
                            turning = Turning.straightAhead;
                            moveVector.Z += moveFactor;


                            if (counter >= 600)
                            {

                                if (num % 2 == 0)
                                {
                                    turning = Turning.anticlockwise;
                                    rotationY += 2;
                                }
                                else
                                {
                                    turning = Turning.clockwise;
                                    rotationY -= 2;
                                }

                                if (counter >= 630)
                                {
                                    counter = 0;

                                    num = rand.Next(1, 10);
                                }
                            }


                            break;
                        }
                }

                switch(range)
                {
                    //will follow player
                     case (Range.followRange):
                    {
                        Vector3 direction = calulateDirection();

                        double rotate = calculateRotation(rotation.Forward, direction, TankCabin.TankRotation.Right);

                        rotationY += (float)rotate;
                        position += direction;
                        
                        break;
                    }
                    //will fire on player
                    case (Range.fireRange):
                    {
                        reduceHealth = false;
                        Vector3 direction = calulateDirection();

                        double rotate = calculateRotation(rotation.Forward, direction, TankCabin.TankRotation.Right);

                        rotationY += (float)rotate;
                        position += direction;

                        if (!hasFired)
                        {
                            iTankExplode.Volume = Game1.Instance.volume;
                            //emitter and listener set to object and player posiitons
                            //used to create the sound from a specific direction
                            //divided by terrain scale just because......
                            emitter.Position = position / Game1.Instance.terrain.terrainScale;
                            listener.Position = TankCabin.Tankposition / Game1.Instance.terrain.terrainScale;
                            //using these positions, feed into soundInstance
                            iFireShot.Apply3D(listener, emitter);
                            iFireShot.Volume = 1f;
                            //play sound
                            iFireShot.Play();
                            hasFired = true;
                            reduceHealth = true;
                        }

                        
                        break;
                    }
                    case(Range.outOf):
                    {
                         break;
                    }
                }

                if (hasFired)
                {
                    firingTimer += gameTime.ElapsedGameTime.Milliseconds;

                    if (firingTimer > 3000)
                    {
                        hasFired = false;
                        firingTimer = 0;
                    }
                }

                rotation = Matrix.CreateRotationY(MathHelper.ToRadians(rotationY));

                position += Vector3.Transform(moveVector, rotation);

              
                rotateAmount = moveFactor / TankWheelRadius;

                wheelRollMatrixForward *= Matrix.CreateRotationX(rotateAmount);
                wheelRollMatrixBack *= Matrix.CreateRotationX(-rotateAmount);

                counter += num;
            }
            else
            {
                explodeTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (explodeTimer > 3000)
                {
                    removable = true;
                }
            }

            #endregion

            if (!isAlive && !hasExploded)
            {
                iTankExplode.Volume = Game1.Instance.volume;
                //emitter and listener set to object and player posiitons
                //used to create the sound from a specific direction
                //divided by terrain scale just because......
                emitter.Position = position / Game1.Instance.terrain.terrainScale;
                listener.Position = TankCabin.Tankposition / Game1.Instance.terrain.terrainScale;
                //using these positions, feed into soundInstance
                iTankExplode.Apply3D(listener, emitter);
                //play sound
                iTankExplode.Play();

                //reducing force each call
                explodeDuration *= 0.95f;

            }
            else if (!isAlive && hasExploded)
            {
                if (explodeDuration < 20)
                    //increasing force each call unless capped
                    explodeDuration *= 1.05f;
            }
        }

        //code taken from sample
        public void LoadBones(ContentManager content)
        {
            

            //Bones which control the wheels of tank
            leftBackWheelBone = myModel.Bones["l_back_wheel_geo"];
            rightBackWheelBone = myModel.Bones["r_back_wheel_geo"];
            leftFrontWheelBone = myModel.Bones["l_front_wheel_geo"];
            rightFrontWheelBone = myModel.Bones["r_front_wheel_geo"];

            //store original transform matrix for each animating bone 
            leftBackWheelTransform = leftBackWheelBone.Transform;
            rightBackWheelTransform = rightBackWheelBone.Transform;
            leftFrontWheelTransform = leftFrontWheelBone.Transform;
            rightFrontWheelTransform = rightFrontWheelBone.Transform;
        }



        public override void Render(Matrix view, Matrix projection, Vector3 position, Matrix orientation)
        {
            #region direction wheels rotate when moving
            switch (turning)
            {
                case (Turning.clockwise):
                    {
                        leftBackWheelBone.Transform = wheelRollMatrixForward * leftBackWheelTransform;
                        leftFrontWheelBone.Transform = wheelRollMatrixForward * leftFrontWheelTransform;
                        rightBackWheelBone.Transform = wheelRollMatrixBack * rightBackWheelTransform;
                        rightFrontWheelBone.Transform = wheelRollMatrixBack * rightFrontWheelTransform;
                        break;
                    }
                case (Turning.anticlockwise):
                    {
                        leftBackWheelBone.Transform = wheelRollMatrixBack * leftBackWheelTransform;
                        leftFrontWheelBone.Transform = wheelRollMatrixBack * leftFrontWheelTransform;
                        rightBackWheelBone.Transform = wheelRollMatrixForward * rightBackWheelTransform;
                        rightFrontWheelBone.Transform = wheelRollMatrixForward * rightFrontWheelTransform;
                        break;
                    }

                case (Turning.straightAhead):
                    {
                        leftBackWheelBone.Transform = wheelRollMatrixForward * leftBackWheelTransform;
                        leftFrontWheelBone.Transform = wheelRollMatrixForward * leftFrontWheelTransform;
                        rightBackWheelBone.Transform = wheelRollMatrixForward * rightBackWheelTransform;
                        rightFrontWheelBone.Transform = wheelRollMatrixForward * rightFrontWheelTransform;
                        break;
                    }
            }
            #endregion

            Matrix[] boneTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(boneTransforms);


            if (!isAlive && explosionDirections.Count < 1)
            {
                foreach (ModelMesh mesh in myModel.Meshes)
                {
                    //create list of random vectors for explosion adding the bullet direction so
                    //pieces fly away from player
                    Vector3 direction = new Vector3(rand.Next(-1, 1), 1, rand.Next(-1, 1)) + playerBulletDirection;
                    //add direction to list
                    explosionDirections.Add(direction);
                    //add position incrememnted by direction to list
                    explodedPositions.Add(direction + position);
                }
            }

            index = 0;
            explodeRotation += 5;

            foreach (ModelMesh mesh in myModel.Meshes)
            {

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    //if tank is alive it will be modelled as normal
                    //otherwise it will be model using the explosion positions
                    //given for each mesh

                    if (isAlive)
                    {
                        effect.World = boneTransforms[mesh.ParentBone.Index]
                           * Matrix.CreateScale(scale)
                           * Matrix.CreateRotationY(MathHelper.ToRadians(rotationY ))
                           * Matrix.CreateRotationX(MathHelper.ToRadians(rotationX ))
                           * orientation
                           * Matrix.CreateTranslation(position);
                    }

                    else
                    {


                        effect.World = boneTransforms[mesh.ParentBone.Index]
                           * Matrix.CreateScale(scale)
                           //added rotation as pieces of mesh fly away
                           * Matrix.CreateRotationY(MathHelper.ToRadians(rotationY + explodeRotation))
                           * Matrix.CreateRotationX(MathHelper.ToRadians(rotationX + explodeRotation))
                           * Matrix.CreateTranslation(explodedPositions[index]);

                            //if the meshes should continue moving up/outwards and the force has not dropped too low
                            if (explodeDuration > explodePivot && !hasExploded)
                            {
                                //multiplying direction by ( force * duration ) and adding to position
                                explodedPositions[index] += explosionDirections[index] * ( explodeDuration * explosionForce );
                            }
                            else
                            {
                                //same as above except
                                //direction will now change to include a downward direction
                                hasExploded = true;
                                explodedPositions[index] += (new Vector3(explosionDirections[index].X, -explosionDirections[index].Y, explosionDirections[index].Z) * ( explodeDuration * explosionForce ));
                            }
                        

                        index++;
                    }
                    

                    effect.View = view;
                    effect.Projection = projection;
                    effect.LightingEnabled = true;
                }
                mesh.Draw();
            }

        }

        //used to calculate the direction the tank will go when in range
         //of a player
        Vector3 calulateDirection()
        {
            // create 2 positions , player's and enemy tanks
            Vector3 nearSource = position;
            Vector3 farSource = TankCabin.Tankposition;

            // find the direction the vector that goes from the nearPoint to the farPoint
            // and normalize it....
            direction = farSource - nearSource;
            
            direction.Normalize();

            return direction;
        }

         //calulate rotation of the tank as it goes towards 
         //player. Returns a signed rotation so the tank will
         //know to turn left or right
        double calculateRotation(Vector3 origin, Vector3 destination, Vector3 destinationRight)
        {
            
            destination.Normalize();
            destinationRight.Normalize();

            float forwardDot = Vector3.Dot(origin, destination);
            float rightDot = Vector3.Dot(origin, destinationRight);

            //clamp to prevent rounding errors
            forwardDot = MathHelper.Clamp(forwardDot, -1.0f, 1.0f);

            double angle = Math.Acos(forwardDot);

            if (rightDot < 0.0f)
                angle *= -1.0f;

                return angle;
        }

        

        
    }
}
