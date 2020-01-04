using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirstMonoGameApp
{
    class Spike
    {
        public Texture2D spike;

        public Vector2 spikePosition;

        public Rectangle SpikeBoundingBox;

        public void initInitialize()
        {
            spikePosition.X = 350;
            spikePosition.Y = 441;

        }

        public void LoadContent(ContentManager content)
        {
            spike = content.Load<Texture2D>("pike02");
            SpikeBoundingBox = new Rectangle((int)spikePosition.X, (int)spikePosition.Y, 6 * spike.Width, 2 * spike.Height);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, int x)
        {
            spriteBatch.Draw(spike,
              position: new Vector2(spikePosition.X + x * spike.Width*2, spikePosition.Y), //x,y
              sourceRectangle: null,
              color: Color.White,
              rotation: 0.0f, //deg
              origin: new Vector2(0, spike.Height),
              scale: new Vector2(2, 2), //scaleX, scaleY
              effects: SpriteEffects.None, //Flip the image
              layerDepth: 0); // z-index
        }
    }
}
