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

        public int maxJump;

        public bool isDead;
        public bool isDying;

        public bool isJumping;

        public Vector2 Velocity;

        public Vector2 PlPositionBFJump;
        public Vector2 PlPosition;

        Texture2D persoTexture;

        public void initInitialize()
        {
            Velocity.X = 500;
            Velocity.Y = 0;
            PlPosition.X = 180;
            PlPosition.Y = 407;
        }


        public void LoadContent(ContentManager content)
        {
            Health = 3;
            persoTexture = content.Load<Texture2D>("idle");
            maxJump = 100;
        }

        public void Update(bool isKeyLeftPressed, bool isKeyRightPressed, float delta) {
            
                if (isKeyLeftPressed)
                {
                PlPosition.X -= Velocity.X * delta;
                }

                if (isKeyRightPressed)
                {
                PlPosition.X += Velocity.X * delta;
                }

            if (isJumping)
            {
                PlPosition.Y -= Velocity.Y*delta;
                if (PlPosition.Y < PlPositionBFJump.Y - maxJump)
                    Velocity.Y = -420.0f;
                if (PlPosition.Y >= PlPositionBFJump.Y && Velocity.Y < 0)
                {
                    isJumping = false;
                    Velocity.Y = 0;
                }
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(persoTexture,
              position: new Vector2(PlPosition.X, PlPosition.Y), //x,y
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
