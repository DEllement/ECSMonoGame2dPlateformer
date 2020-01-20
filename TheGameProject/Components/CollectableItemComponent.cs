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
        public Body Body { get; set; }
        public Rectangle BottomSensorBoundingBox =>
            new Rectangle((int)(Body.Position.X * physScale),
                          (int)((Body.Position.Y * physScale) + Size.Y),
                          (int)(Size.X),
                          10); //Need to Adjust

        public Rectangle BoundingBox => new Rectangle((int)(Body.Position.X * physScale), (int)(Body.Position.Y * physScale), Size.X, Size.Y);

        public CollectableItemComponent(Point2 position, Point size)
        {
            Size = size;

            Body = new Body();

            Body.FixedRotation = true;

            Body.BodyType = BodyType.Static;
        }
    }
}
