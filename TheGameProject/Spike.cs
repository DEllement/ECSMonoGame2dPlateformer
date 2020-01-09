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
        class Spike : BaseClass
        {
            public Spike(Texture2D boxTexture, Point2 position, Point size, bool isAffectedByGravity) : base(boxTexture, position, size, isAffectedByGravity)
            {
                IsAffectedByGravity = true;
            }
        }
    }
}
