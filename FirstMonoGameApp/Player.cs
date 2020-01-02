using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirstMonoGameApp
{
    class Player
    {
        public int Health { get; set; }

        Texture2D persoTexture;

        public void LoadContent(ContentManager content)
        {
            Health = 4;
            persoTexture = content.Load<Texture2D>("idle");
        }

        public void Update(GameTime gameTime) { 
        
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(persoTexture,
              position: new Vector2(100, 100), //x,y
              sourceRectangle: null,
              color: Color.White,
              rotation: 0.0f, //deg
              origin: new Vector2(persoTexture.Width / 2, persoTexture.Height / 2),
              scale: new Vector2(2, 2), //scaleX, scaleY
              effects: SpriteEffects.None, //Flip the image
              layerDepth: 0); // z-index

        }
    }
}
