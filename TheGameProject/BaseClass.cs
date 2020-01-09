using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;

namespace TheGameProject
{
    class BaseClass
    {


        //Physic Component\
        public Texture2D BoxTexture;

        public Point2 Position { get; set; }
        public Point Size { get; set; }
        public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, Size.X, Size.Y);
        public bool IsRigid { get; set; }
        public bool IsAffectedByGravity { get; set; }
        public int X
        {
            get => (int)Position.X;
            set => Position = new Point2(value, Position.Y);
        }
        public int Y
        {
            get => (int)Position.Y;
            set => Position = new Point2(Position.X, value);
        }

        public BaseClass(Texture2D BoxTexture, Point2 position, Point size, bool isAffectedByGravity = false)
        {
            Position = position;
            Size = size;
            IsRigid = true;
            IsAffectedByGravity = isAffectedByGravity;
            boxTexture = BoxTexture;
        }


    }

   
}
