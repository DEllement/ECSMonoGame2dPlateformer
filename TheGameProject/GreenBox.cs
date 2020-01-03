using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirstMonoGameApp
{
    class GreenBox
    {
        Texture2D greenBox;

        public Vector2 GPPosition;

        public void initInitialize()
        {
            GPPosition.X = 630;
            GPPosition.Y = 441;

        }

        public void LoadContent(ContentManager content)
        {
            greenBox = content.Load<Texture2D>("greenBlock");
        }

        public void Update(GameTime gameTime)
        {

        }

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

        }
    }
}
