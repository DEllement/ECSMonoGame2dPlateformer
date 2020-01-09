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
        class GreenBox: BaseClass
        {
            public GreenBox(Point2 position, Point size, bool isAffectedByGravity) : base(position, size, isAffectedByGravity)
            {
                IsAffectedByGravity = true;
            }
        }
    }

}

/*
public void Draw(SpriteBatch spriteBatch)
{
    spriteBatch.Draw(greenBox,
      position: new Vector2(GPPosition.X, GPPosition.Y), //x,y
      sourceRectangle: null,
      color: Color.White,
      rotation: 0.0f, //deg
      origin: new Vector2(0, greenBox.Height),
      scale: new Vector2(0.9f, 0.9f), //scaleX, scaleY
      effects: SpriteEffects.None, //Flip the image
      layerDepth: 0); // z-index

}*/