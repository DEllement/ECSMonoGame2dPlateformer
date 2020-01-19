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
        public Color color;
        public int CollectedGem;
      public CollectableItemComponent(Color Color){
            color = Color;
        }
    }
}
