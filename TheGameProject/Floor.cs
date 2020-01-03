using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirstMonoGameApp
{
    class Floor
    {
        public Texture2D floor;

        public Vector2 floorPosition;

        public void initInitialize()
        {
            floorPosition.X = 0;
            floorPosition.Y = 480;

        }

        public void LoadContent(ContentManager content)
        {
            floor = content.Load<Texture2D>("floor");
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, int x)
        {
            spriteBatch.Draw(floor,
              position: new Vector2(floorPosition.X+x*floor.Width*0.3f, floorPosition.Y), //x,y
              sourceRectangle: null,
              color: Color.White,
              rotation: 0.0f, //deg
              origin: new Vector2(0,floor.Height),
              scale: new Vector2(0.3f, 0.3f), //scaleX, scaleY
              effects: SpriteEffects.None, //Flip the image
              layerDepth: 0); // z-index
        }
    }
}
