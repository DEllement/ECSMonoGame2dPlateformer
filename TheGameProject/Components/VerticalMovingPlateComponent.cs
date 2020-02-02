using System;
using System.Collections.Generic;
using System.Text;
using Aether.Physics2D.Common;
using Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;


namespace TheGameProject.Components
{
    public class VerticalMovingPlateComponent
    {
       public bool PlateGoingUp { get; set; }
       public bool PlateGoingDown { get; set; }
       public int Velocity { get; set; }
       public int MaxUp { get; set; }
       public int MaxDown { get; set; }

        public VerticalMovingPlateComponent()
        {
           
        }
    }
}
