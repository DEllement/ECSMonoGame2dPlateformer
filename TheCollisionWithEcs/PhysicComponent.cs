using System;
using System.Collections.Generic;
using System.Text;
using Aether.Physics2D.Collision.Shapes;
using Aether.Physics2D.Common;
using Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace TheCollisionWithEcs
{
    public class PhysicComponent
    {
        public Body Body { get; set; }
        public Aether.Physics2D.Common.Maths.Vector2 Position
        {
            get => Body.Position;
            set => Body.Position = value;
        }

        public Point Size { get; set; }
        public Rectangle BoundingBox => new Rectangle((int)Position.X,(int)Position.Y, Size.X, Size.Y);
        public bool IsRigid { get; set; }
        public bool IsAffectedByGravity { get; set; }
       
        public PhysicComponent( Point2 position, Point size,  bool isAffectedByGravity=false)
        {
            Size = size;
            IsRigid = true;
            IsAffectedByGravity = isAffectedByGravity;

            var vec2Pos = new Aether.Physics2D.Common.Maths.Vector2(position.X, position.Y);
           
            Body = new Body();
            Body.CreatePolygon(new Vertices(new[] {
                new Aether.Physics2D.Common.Maths.Vector2(0f, 0f),
                new Aether.Physics2D.Common.Maths.Vector2((float)size.X, 0f),
                new Aether.Physics2D.Common.Maths.Vector2((float)size.X, (float)size.Y),
                new Aether.Physics2D.Common.Maths.Vector2(0f, (float)size.Y),
                new Aether.Physics2D.Common.Maths.Vector2(0f, 0f),
            }), 1f);
            Body.Position = vec2Pos;
            Body.Mass = 0.01f;

            if (IsAffectedByGravity)
                Body.BodyType = BodyType.Dynamic;
            else
                Body.BodyType = BodyType.Static;

            Body.SetRestitution(0.0f);
            Body.SetFriction(0.5f);
        }
    }

}
