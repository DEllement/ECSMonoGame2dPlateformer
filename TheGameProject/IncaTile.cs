using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;

namespace TheGameProject { 

    namespace FirstMonoGameApp
{
        class IncaTile: BaseClass
              {
                  public IncaTile( Point2 position, Point size, bool isAffectedByGravity) : base(position, size, isAffectedByGravity=true)
              {
            }
        }
    }
}