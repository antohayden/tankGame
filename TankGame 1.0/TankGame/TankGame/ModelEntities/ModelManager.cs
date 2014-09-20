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
using TankGame;

namespace TankGame.ModelEntities
{

    public enum CollisionType
    {
        None,
        Boundary,
        Object,
        Bullet,
        FiringRange,
        ViewingRange
    };

    //handles collision and bounding shapes of models
    public class ModelManager
    {

        //Lists to store bounding boxes of models
        public Dictionary<string, BoundingBox> boundingBoxList;
        public Dictionary<string, BoundingSphere> boundingSphereList;

        //map boundaries
        public BoundingBox boundaries;
        public VertexPositionNormalTexture[] mapVertices;

        public ModelManager(VertexPositionNormalTexture[] mapVertices)
        {
            this.mapVertices = mapVertices;
            boundingBoxList = new Dictionary<string, BoundingBox>();
            boundingSphereList = new Dictionary<string, BoundingSphere>();
        }

        //create boundaries of map
        public void SetUpBoundaries(float scale, float minHeight, float maxHeight)
        {
            Vector3[] boundaryPoints = new Vector3[2];

            /* Create a bounding box large enough to enclose a large portion of the terrain */

            boundaryPoints[0] = new Vector3(scale * 10.5f, (scale * -1.1f) * minHeight, scale * -10.5f);
            boundaryPoints[1] = new Vector3(( mapVertices[mapVertices.Length - 1].Position.X) - scale * 10.5f ,
                                            ( scale * 1.1f) * maxHeight,
                                            ( mapVertices[mapVertices.Length - 1].Position.Z ) + scale * 10.5f  );
            boundaries = BoundingBox.CreateFromPoints(boundaryPoints);
            
        }

        //simple collision check between objects and tank
        public CollisionType CheckCollision(BoundingSphere sphere, string name)
        {

            foreach (KeyValuePair<string, BoundingSphere> x in boundingSphereList)
            {
                if (x.Value.Intersects(sphere) && x.Key != name)

                    if (x.Key == "bullet" )
                        return CollisionType.Bullet;
                    else
                        return CollisionType.Object;
            }

            foreach (KeyValuePair<string, BoundingBox> y in boundingBoxList)
            {
                if (y.Value.Intersects(sphere) && y.Key != name)
                    if (y.Key == "bullet")
                        return CollisionType.Bullet;
                    else
                        return CollisionType.Object;
            }

            //if the BB does NOT contain
            if (boundaries.Contains(sphere) != ContainmentType.Contains)
                return CollisionType.Boundary;
            
            return CollisionType.None;
        }

        public void AddSphere ( string name, BoundingSphere sphere)
        {
            boundingSphereList.Add(name, sphere);
        }

        public void AddBox(string name, BoundingBox box)
        {
            boundingBoxList.Add(name, box);
        }


    }
}
