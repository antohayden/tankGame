using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TankGame;


namespace TankGame.Environment
{

    public class Terrain
    {
        ContentManager Content;
        GraphicsDevice device;

        string terrain;
        
        //scaling of heightmap
        public int terrainScale = 400;

        //derived from heightmap
        int terrainWidth, terrainLength;

        //used for reducing noise in the plane
        public float minHeight;
        public float maxHeight;

        //map textures
        public Texture2D texture;

        float[,] heightData;
        VertexPositionNormalTexture[] vertices;

        public VertexPositionNormalTexture[] Vertices
        {
            get
            {
                return vertices;
            }
        }

        int[] indices;
        Effect effect;

        VertexBuffer terrainVertexBuffer;
        IndexBuffer terrainIndexBuffer;

        //constructor using heightmap img
        public Terrain(ContentManager Content, GraphicsDevice device, string terrainPath, string texturePath)
        {
            this.Content = Content;
            this.device = device;
            this.terrain = terrainPath;
            effect = Content.Load<Effect>("TerrainImg/Effect");

            this.texture = Content.Load<Texture2D>(texturePath);
        }

        //Takes in grayscale heightmap and stores the value of white as height in array
        private void LoadHeightData(Texture2D heightMap)
        {
            minHeight = float.MaxValue;
            maxHeight = float.MinValue;

            terrainWidth = heightMap.Width;
            terrainLength = heightMap.Height;

            //array to store colors of image.
            Color[] heightMapColors = new Color[terrainWidth * terrainLength];

            //taking array as parameter to store values
            heightMap.GetData(heightMapColors);

            heightData = new float[terrainWidth, terrainLength];


            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainLength; y++)
                {
                    //using value of red as RBG are all equal in greyscale
                    heightData[x, y] = heightMapColors[x + y * terrainWidth].R;
                    if (heightData[x, y] < minHeight) minHeight = heightData[x, y];
                    if (heightData[x, y] > maxHeight) maxHeight = heightData[x, y];
                }

            for (int x = 0; x < terrainWidth; x++)
                for (int y = 0; y < terrainLength; y++)
                    heightData[x, y] = (heightData[x, y] - minHeight) / (maxHeight - minHeight) * 10.0f;
        }

        private void SetUpTerrainVertices()
        {
            vertices =
                new VertexPositionNormalTexture[terrainWidth * terrainLength];

            for (int x = 0; x < terrainWidth; x++)
            {
                for (int z = 0; z < terrainLength; z++)
                {
                    vertices[x + (z * terrainWidth)].Position = new Vector3(x * terrainScale, heightData[x, z] *  terrainScale , -z * terrainScale );
                    vertices[x + (z * terrainWidth)].TextureCoordinate.X = (float) x  / 30;
                    vertices[x + (z * terrainWidth)].TextureCoordinate.Y = (float)z / 30;
                }
            }
        }

        private void SetUpTerrainIndices()
        {
            indices = new int[(terrainWidth - 1) * (terrainLength - 1) * 6];
            int counter = 0;
            for (int y = 0; y < terrainLength - 1; y++)
            {
                for (int x = 0; x < terrainWidth - 1; x++)
                {
                    int lowerLeft = x + y * terrainWidth;
                    int lowerRight = (x + 1) + y * terrainWidth;
                    int topLeft = x + (y + 1) * terrainWidth;
                    int topRight = (x + 1) + (y + 1) * terrainWidth;

                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = lowerLeft;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = lowerRight;
                }
            }
        }


        private void CalculateNormals()
        {
            //Set all normals to vector3
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal = Vector3.Zero;

            /* Next, I itierate through each triangle and calculate it's normal. 
             * This is done by taking 3 vertices which make a triangle. 
             * By subtracting one from another I can get 2 of their sides. 
             * I then calculate the normal via cross product of two vertices. 
             * This is then added to the normal of each vertice. 
             * Doing this for every triangle will mean that at any one vertice, 
             * it will have the average of each of it's connecting triangles.
             */
            for (int i = 0; i < indices.Length / 3; i++)
            {
                int index1 = indices[i * 3];
                int index2 = indices[i * 3 + 1];
                int index3 = indices[i * 3 + 2];    

                Vector3 side1 = vertices[index1].Position - vertices[index3].Position;
                Vector3 side2 = vertices[index1].Position - vertices[index2].Position;
                Vector3 normal = Vector3.Cross(side1, side2);

                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
                vertices[index3].Normal += normal;
            }

            //finally normalise each normal
            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal.Normalize();
        }

        private void CopyToTerrainBuffers()
        {
            terrainVertexBuffer = new VertexBuffer(device, VertexPositionNormalTexture.VertexDeclaration, vertices.Length,
                BufferUsage.WriteOnly);
            terrainVertexBuffer.SetData(vertices);

            terrainIndexBuffer = new IndexBuffer(device, typeof(int), indices.Length, BufferUsage.WriteOnly);
            terrainIndexBuffer.SetData(indices);
            
        }

        public void LoadVertices()
        {

            Texture2D heightMap = Content.Load<Texture2D>(terrain); 
            LoadHeightData(heightMap);
            SetUpTerrainVertices();
            SetUpTerrainIndices();
            CalculateNormals();
            CopyToTerrainBuffers();
        }

        public void Draw(GameTime gameTime, Matrix view, Matrix projection)
        {
            Matrix worldMatrix = Matrix.CreateTranslation(0, -50, 0);

            effect.CurrentTechnique = effect.Techniques["Textured"];
            effect.Parameters["xTexture"].SetValue(texture);
            effect.Parameters["xWorld"].SetValue(worldMatrix);
            effect.Parameters["xView"].SetValue(view);
            effect.Parameters["xProjection"].SetValue(projection);

            effect.Parameters["xEnableLighting"].SetValue(true);
            effect.Parameters["xAmbient"].SetValue(0.2f);
            effect.Parameters["xLightDirection"].SetValue(new Vector3(0.5f, -1, 0.5f));

            

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.Indices = terrainIndexBuffer;
                device.SetVertexBuffer(terrainVertexBuffer);       
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertices.Length, 0, indices.Length / 3);

            }
                
        }

    }
}