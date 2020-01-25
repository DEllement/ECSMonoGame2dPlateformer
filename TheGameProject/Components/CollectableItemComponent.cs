using System;
using System.Collections.Generic;
using System.Text;
using Aether.Physics2D.Common;
using Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;


namespace TheGameProject.Components
{
    public class CollectableItemComponent
    {
        public const float physScale = 100.0f; //Can change this on slow down the gravity or increase the speed

        public Point Size { get; set; }

        public Point CPosition { get; set; }
        public Rectangle BottomSensorBoundingBox =>
            new Rectangle((int)(CPosition.X * physScale),
                          (int)((CPosition.Y * physScale) + Size.Y),
                          (int)(Size.X),
                          10); //Need to Adjust

        public Rectangle BoundingBox;

        public CollectableItemComponent(Point2 position, Point size)
        {
            //ok i know its bad 
            int X =(int)position.X;
            int Y =(int)position.Y;

            Point pos;
            pos.X = X;
            pos.Y = Y;

            Size = size;
            
            BoundingBox = new Rectangle(pos, Size);
        }
    }
}
