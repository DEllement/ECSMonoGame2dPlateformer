using System;
using System.Collections.Generic;
using System.Text;
using Aether.Physics2D.Collision.Shapes;
using Aether.Physics2D.Common;
using Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

using Vector2 = Aether.Physics2D.Common.Maths.Vector2; //Important to not have to specify the namepace cause there is 2 Vector2 one in XNA and other in Aether


namespace TheGameProject.Components
{
    public class PhysicComponent
    {
        public const float physScale = 50.0f; //Can change this on slow down the gravity or increase the speed

        public Point Size { get; set; }

        public Body Body { get; set; }
        public Rectangle BottomSensorBoundingBox => 
            new Rectangle((int)(Body.Position.X*physScale),
                          (int)((Body.Position.Y*physScale)+Size.Y),
                          (int)(Size.X),
                          10); //Need to Adjust

        public Rectangle BoundingBox => new Rectangle((int)Body.Position.X,(int)Body.Position.Y, Size.X, Size.Y);
        public bool IsRigid { get; set; }
        public bool IsAffectedByGravity { get; set; }
       
        public PhysicComponent( Point2 position, Point size, bool isAffectedByGravity=false, bool isRigid=true)
        {
            Size = size;
            IsRigid = isRigid;
            IsAffectedByGravity = isAffectedByGravity;

            var vec2Pos = new Vector2(position.X, position.Y)/physScale;
           
            Body = new Body();
            Body.CreatePolygon(new Vertices(new[] {
                new Vector2(0f, 0f),
                new Vector2((float)size.X/physScale, 0f),
                new Vector2((float)size.X/physScale, (float)size.Y/physScale),
                new Vector2(0f, (float)size.Y/physScale),
                new Vector2(0f, 0f),
            }), 1f);
            Body.Position = vec2Pos;
            Body.Mass = 2.0f;
            Body.FixedRotation = true;
            Body.Inertia = 1f;
        
            if (IsAffectedByGravity) {
                Body.BodyType = IsRigid ? BodyType.Dynamic : BodyType.Kinematic;
            }
            else
                Body.BodyType = BodyType.Static;

            Body.SetRestitution(0.0f);
            Body.SetFriction(0.1f);

        }

    }

}
