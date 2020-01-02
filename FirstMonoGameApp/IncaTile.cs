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
      
        Texture2D incaTileTexture;
        
        public void LoadContent(ContentManager content)
        {
            incaTileTexture = content.Load<Texture2D>("inca_tile01");
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(incaTileTexture,
              position: new Vector2(150, 150), //x,y
              sourceRectangle: null,
              color: Color.White,
              rotation: 0.0f, //deg
              origin: new Vector2(incaTileTexture.Width / 2, incaTileTexture.Height / 2),
              scale: new Vector2(2, 2), //scaleX, scaleY
              effects: SpriteEffects.None, //Flip the image
              layerDepth: 0); // z-index

        }

    }
}
