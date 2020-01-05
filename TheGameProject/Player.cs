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

        public Rectangle PlBoundingBox;

        public bool isColidingWithTop;
        public bool isColidingWithBottom;
        public bool isColidingWithLeft;
        public bool isColidingWithRight;

        public Vector2 Velocity;

        public Vector2 PlPositionBFJump;
        public Vector2 PlPosition;

        public int floorLevel;

        Texture2D persoTexture;

        public void initInitialize()
        {
            
            Velocity.X = 500;
            Velocity.Y = 0;
            PlPosition.X = 180;
            PlPosition.Y = 407;
            floorLevel = 407;
            isColidingWithTop=
            isColidingWithBottom=
            isColidingWithLeft=
            isColidingWithRight=
            isDead=
            isJumping=
            isDying
            =false;
    }


        public void LoadContent(ContentManager content)
        {
            Health = 3;
            persoTexture = content.Load<Texture2D>("idle");
            maxJump = 100;
        }

        public void Update(bool isKeyLeftPressed, bool isKeyRightPressed, float delta) {
           PlBoundingBox = new Rectangle((int)PlPosition.X, (int)PlPosition.Y, 2 * persoTexture.Width, 2 * persoTexture.Height);
            Console.WriteLine(PlBoundingBox.Right + " " + delta);

            if (!isColidingWithLeft)
            {
                if (isKeyLeftPressed)
                {
                    PlPosition.X -= Velocity.X * delta;
                }
            }
            if (!isColidingWithRight)
            {
                if (isKeyRightPressed)
                {
                    PlPosition.X += Velocity.X * delta;
                }
            }
            if (isJumping)
            {
                if (!isColidingWithBottom)
                {
                    PlPosition.Y -= Velocity.Y * delta;
                    if (PlPosition.Y < PlPositionBFJump.Y - maxJump)
                        Velocity.Y = -420.0f;
                    if (PlPosition.Y >= PlPositionBFJump.Y && Velocity.Y < 0)
                    {
                        isJumping = false;
                        Velocity.Y = 0;
                    }
                }
                else
                {

                }
            }

            if (isDying)
            {
                System.Threading.Thread.Sleep(2000);
                initInitialize();
                Console.WriteLine("afte");
                Console.WriteLine(isDying);
            }

         // Console.WriteLine(PlBoundingBox.Right);
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
        public void DrawDying(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(persoTexture,
              position: new Vector2(PlPosition.X, PlPosition.Y), //x,y
              sourceRectangle: null,
              color: Color.Red,
              rotation: 0.0f, //deg
              origin: new Vector2(persoTexture.Width / 2, persoTexture.Height / 2),
              scale: new Vector2(2, 2), //scaleX, scaleY
              effects: SpriteEffects.None, //Flip the image
              layerDepth: 0); // z-index
        }
    }
}
