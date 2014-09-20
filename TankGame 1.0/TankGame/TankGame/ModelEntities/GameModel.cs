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
    class GameModel
    {

        public Vector3 position;
        public Vector3 target { get; set; }
        public Vector3 UpVector { get; set; }
        public float speed { get; set; }

        public float rotationX { get; set; }
        public float rotationY { get; set; }
        public float scale { get; set; }
        public bool isAlive { get; set; }

        protected Model myModel;
        private ContentManager Content;
        protected Matrix[] transforms;
        public BoundingSphere Bsphere;

        public GameModel(ContentManager Content)
        {
            //constructor
            this.Content = Content;
            this.target = Vector3.Zero;
            this.rotationX = 0;
            this.rotationY = 0;
            this.isAlive = true;
            this.scale = 1;
        }

        public void Load(string modelAsset, Vector3 position)
        {
            this.position = position;
            myModel = Content.Load<Model>(modelAsset);
            transforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(transforms);
            Bsphere = getBoundingSphere(position);
        }

        public virtual void Render(Matrix view, Matrix projection, Vector3 position, Matrix orientation)
        {

            //ISROT - Identity, Scale, Rotation, Orbit, Translate

            foreach (ModelMesh mesh in myModel.Meshes)
            {
               
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    
                    effect.World = transforms[mesh.ParentBone.Index]
                        * Matrix.CreateScale(scale)
                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotationY))
                        * Matrix.CreateRotationX(MathHelper.ToRadians(rotationX))
                        * orientation
                        * Matrix.CreateTranslation(position);

                    effect.View = view;
                    effect.Projection = projection;
                    effect.LightingEnabled = true;
                }
                mesh.Draw();
            }
        }

        public BoundingSphere getBoundingSphere(Vector3 position)
        {
            BoundingSphere bs = new BoundingSphere();
            
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                if (bs.Radius == 0)
                    bs = mesh.BoundingSphere;
                else
                    bs = BoundingSphere.CreateMerged(bs, mesh.BoundingSphere);
            }

            bs.Center = position;
            bs.Radius *= scale;

            return bs;
        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        //takes in terrain being used and a reference to the objects orientation
        public float getHeightOrientation ( Terrain terrain, ref Matrix orientation)
        {
            //integer division to get cell of heightmap the tank is on
            int X = (int)position.X / (int)terrain.terrainScale;
            int Z = (int)Math.Abs(position.Z) / (int)terrain.terrainScale;

            //257 is width of heightmap, replace with variable if creating more maps

            //create indices to find all four vertices to make quad
            float vertexIndex = ((Z + 1) * 257) + X;            //topleft
            float vertexIndex2 = (Z * 257) + X;                 //bottomleft
            float vertexIndex3 = ((Z + 1) * 257) + (X + 1);     //topright
            float vertexIndex4 = Z * 257 + (X + 1);             //bottomRight

            //Normalise X and Z to find how far I am from the edge
            float xNormalised = (position.X % terrain.terrainScale) / terrain.terrainScale;
            float zNormalised = 1 + ((position.Z % terrain.terrainScale) / terrain.terrainScale);

            float topLeft = terrain.Vertices[(int)vertexIndex].Position.Y;
            float bottomLeft = terrain.Vertices[(int)vertexIndex2].Position.Y;
            float topRight = terrain.Vertices[(int)vertexIndex3].Position.Y;
            float bottomRight = terrain.Vertices[(int)vertexIndex4].Position.Y;

            float topHeight = MathHelper.Lerp(topLeft, topRight, xNormalised);
            float bottomHeight = MathHelper.Lerp(bottomLeft, bottomRight, xNormalised);

            float height = MathHelper.Lerp(topHeight, bottomHeight, zNormalised);

            Vector3 topNormal = Vector3.Lerp(terrain.Vertices[(int)vertexIndex].Normal,
                                            terrain.Vertices[(int)vertexIndex3].Normal,
                                            xNormalised);

            Vector3 bottomNormal = Vector3.Lerp(terrain.Vertices[(int)vertexIndex2].Normal,
                                                  terrain.Vertices[(int)vertexIndex4].Normal,
                                                  xNormalised);

            Vector3 normal = Vector3.Lerp(topNormal, bottomNormal, zNormalised);
            normal.Normalize();

            UpVector = normal;

            orientation.Up = normal;

            orientation.Right = Vector3.Cross(target, orientation.Up);
            orientation.Right.Normalize();

            orientation.Forward = Vector3.Cross(orientation.Up, orientation.Right);
            orientation.Forward.Normalize();

            target = orientation.Forward;

            return height;
        }

        public float getHeightOrientation(Terrain terrain, TankCabin tankCabin, ref Matrix orientation)
        {

            //integer division to get cell of heightmap the tank is on
            int X = (int)TankCabin.Tankposition.X / (int)terrain.terrainScale;
            int Z = (int)Math.Abs(TankCabin.Tankposition.Z) / (int)terrain.terrainScale;

            //257 is width of heightmap
            float vertexIndex = ((Z + 1) * 257) + X;            //topleft
            float vertexIndex2 = (Z * 257) + X;                 //bottomleft
            float vertexIndex3 = ((Z + 1) * 257) + (X + 1);     //topright
            float vertexIndex4 = Z * 257 + (X + 1);             //bottomRight

            float xNormalised = (TankCabin.Tankposition.X % terrain.terrainScale) / terrain.terrainScale;
            float zNormalised = 1 + ((TankCabin.Tankposition.Z % terrain.terrainScale) / terrain.terrainScale);

            float topLeft = terrain.Vertices[(int)vertexIndex].Position.Y;
            float bottomLeft = terrain.Vertices[(int)vertexIndex2].Position.Y;
            float topRight = terrain.Vertices[(int)vertexIndex3].Position.Y;
            float bottomRight = terrain.Vertices[(int)vertexIndex4].Position.Y;

            float topHeight = MathHelper.Lerp(topLeft, topRight, xNormalised);
            float bottomHeight = MathHelper.Lerp(bottomLeft, bottomRight, xNormalised);

            float height = MathHelper.Lerp(topHeight, bottomHeight, zNormalised);

            Vector3 topNormal = Vector3.Lerp(terrain.Vertices[(int)vertexIndex].Normal,
                                            terrain.Vertices[(int)vertexIndex3].Normal,
                                            xNormalised);

            Vector3 bottomNormal = Vector3.Lerp(terrain.Vertices[(int)vertexIndex2].Normal,
                                                  terrain.Vertices[(int)vertexIndex4].Normal,
                                                  xNormalised);

            Vector3 normal = Vector3.Lerp(topNormal, bottomNormal, zNormalised);
            normal.Normalize();

            orientation.Up = normal;

            orientation.Right = Vector3.Cross(TankCabin.tankForward, orientation.Up);
            orientation.Right.Normalize();

            orientation.Forward = Vector3.Cross(orientation.Up, orientation.Right);
            orientation.Forward.Normalize();

            return height;

        }

    }
}
