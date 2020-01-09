using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;

namespace TheGameProject
{
    namespace FirstMonoGameApp
    {


        class Player : BaseClass
        {
            public bool IsLeftDown { get; set; }
            public bool IsRightDown { get; set; }
            public bool IsUpDown { get; set; }
            public bool IsBottomDown { get; set; }
            public bool IsSpaceDown { get; set; }

            //PlayerDataComponent
            public bool IsJumping { get; set; }

            public Point2 Velocity;

           public float JumpMax;

            public Player(Point2 position, Point size, bool isAffectedByGravity, Point2 velocity, float jumpMax) : base(boxTexture ,position, size, isAffectedByGravity)
            {
                IsAffectedByGravity = true;
                Velocity = velocity;
                JumpMax = jumpMax;
            }
        }
    }

}