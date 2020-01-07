using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirstMonoGameApp
{
    class IncaTile
    {

        public Vector2 ITPosition;

        public Rectangle ITBoundingBox;
      
        Texture2D incaTile;

        public void initInitialize()
        {
            ITPosition.X = 290;
            ITPosition.Y = 441;

        }

        public void LoadContent(ContentManager content)
        {
            incaTile = content.Load<Texture2D>("inca_tile01");
            ITBoundingBox = new Rectangle ((int)ITPosition.X, (int)ITPosition.Y, 2 *incaTile.Width, 2 * incaTile.Height);
        }

        public void Draw(SpriteBatch spriteBatch, int x)
        {
            spriteBatch.Draw(incaTile,
              position: new Vector2(ITPosition.X+x, ITPosition.Y), //x,y
              sourceRectangle: null,
              color: Color.White,
              rotation: 0.0f, //deg
              origin: new Vector2(0, incaTile.Height),
              scale: new Vector2(2, 2), //scaleX, scaleY
              effects: SpriteEffects.None, //Flip the image
              layerDepth: 0); // z-index
        }

    }
}
