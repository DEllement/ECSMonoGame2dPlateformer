using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace FirstMonoGameApp
{

    public class MyFuckingGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public bool isKeyLeftPressed;
        public bool isKeyRightPressed;

        Player player;
        IncaTile incaTile;
        Floor floor;
        Spike spike;
        GreenBox greenBox;

        TimeSpan deltaTime;

        //1.
        public MyFuckingGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //2. This is the Game Object Init Method, init variables here if needed....
        protected override void Initialize()
        {

            player = new Player();
            incaTile = new IncaTile();
            floor = new Floor();
            spike = new Spike();
            greenBox = new GreenBox();


            greenBox.initInitialize();
            floor.initInitialize();
            player.initInitialize();
            spike.initInitialize();
            incaTile.initInitialize();

            base.Initialize();
        }

        //3. This is called on boot, and its for pre-loading resources (this is for loading Image, Mesh and other external data)
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(Content);
            incaTile.LoadContent(Content);
            floor.LoadContent(Content);
            spike.LoadContent(Content);
            greenBox.LoadContent(Content);

        }

        #region GameLoop

        //4. This is the update method called many time per second, delta infos is available in gameTime.ElapsedGameTime
        protected override void Update(GameTime gameTime)
        {


            KeyboardState CurrentKeyboardState = Keyboard.GetState();


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            deltaTime = gameTime.ElapsedGameTime;
            float delta = deltaTime.Milliseconds;
            delta = delta / 1000;

            //Bad
            if (player.PlBoundingBox.Right < 450)
            {

                if (player.PlBoundingBox.Right >= 298 && player.PlBoundingBox.Bottom >= 416 && player.PlBoundingBox.Right < 350)
                {
                    player.isColidingWithRight = true;
                }
                else
                {
                    player.isColidingWithRight = false;
                }

                if (player.PlBoundingBox.Right >= 400 && player.PlBoundingBox.Bottom >= 416 && player.PlBoundingBox.Right < 420)
                {
                    player.isColidingWithLeft = true;
                }
                else
                {
                    player.isColidingWithLeft = false;
                }

                if (player.PlBoundingBox.Right > 306 && player.PlBoundingBox.Bottom <= 415 && player.PlBoundingBox.Bottom >= 410 && player.PlBoundingBox.Right < 410)
                {
                    player.PlPositionBFJump = player.PlPosition;
                    player.isJumping = false;
                }

                if (!player.isJumping && player.PlBoundingBox.Right < 306 && player.PlPosition.Y != 407)
                {
                    player.PlPositionBFJump.Y = player.floorLevel;
                    player.isJumping = true;
                    player.Velocity.Y = -420;
                }


                if (!player.isJumping && player.PlBoundingBox.Right > 415 && player.PlPosition.Y != 407)
                {
                    player.PlPositionBFJump.Y = player.floorLevel;
                    player.isJumping = true;
                    player.Velocity.Y = -420;
                }


            }
            if (player.PlBoundingBox.Right > 490)
            {
                if (player.PlBoundingBox.Right >= 505 && player.PlBoundingBox.Bottom >= 416 && player.PlBoundingBox.Right < 550)
                {
                    player.isColidingWithRight = true;
                }
                else
                {
                    player.isColidingWithRight = false;
                }

                if (player.PlBoundingBox.Right >= 590 && player.PlBoundingBox.Bottom >= 416 && player.PlBoundingBox.Right < 615)
                {
                    player.isColidingWithLeft = true;
                }
                else
                {
                    player.isColidingWithLeft = false;
                }

                if (player.PlBoundingBox.Right > 505 && player.PlBoundingBox.Bottom <= 415 && player.PlBoundingBox.Bottom >= 410 && player.PlBoundingBox.Right < 615)
                {
                    player.PlPositionBFJump = player.PlPosition;
                    player.isJumping = false;
                }

                if (!player.isJumping && player.PlBoundingBox.Right < 505 && player.PlPosition.Y != 407)
                {
                    player.PlPositionBFJump.Y = player.floorLevel;
                    player.isJumping = true;
                    player.Velocity.Y = -420;
                }


                if (!player.isJumping && player.PlBoundingBox.Right > 615 && player.PlPosition.Y != 407)
                {
                    player.PlPositionBFJump.Y = player.floorLevel;
                    player.isJumping = true;
                    player.Velocity.Y = -420;
                }


            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                isKeyLeftPressed = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                isKeyRightPressed = true;
            }

            if(Keyboard.GetState().IsKeyDown(Keys.Up)&& !player.isJumping)
            {
                player.Velocity.Y = 420;
                player.isJumping = true;
                player.PlPositionBFJump = player.PlPosition;
            }

            player.Update(isKeyLeftPressed, isKeyRightPressed, delta);
            //Console.WriteLine(player.PlBoundingBox.Right);
            if (player.PlBoundingBox.Intersects(spike.SpikeBoundingBox)) //Good
            {
                player.isDying=true;
            }

            isKeyLeftPressed = false;
            isKeyRightPressed = false;

            base.Update(gameTime);
        }

        //5. This is the Render Method of the Game object
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black); //This paint the background black

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            for (int x = 0; x < 21; x++)
            {
                floor.Draw(spriteBatch,x);
            
            }
            
            incaTile.Draw(spriteBatch,0);
            incaTile.Draw(spriteBatch,200);
            
            for (int x = 0; x < 3; x++)
            {
                spike.Draw(spriteBatch, x);
            }

            greenBox.Draw(spriteBatch);

          //  Console.WriteLine(player.isDying);
            if (!player.isDying)
            {
                player.Draw(spriteBatch);
            }
            else
            {
             //   Console.WriteLine(1234);
                player.DrawDying(spriteBatch);
            }
            spriteBatch.End();
           
            base.Draw(gameTime);
            
        }

        #endregion
    }

    
    class Program {
        static void Main(string[] args)
        {
            MyFuckingGame game = new MyFuckingGame();

            game.Run(); //This will launch the window and start the engine
        }
    }
}



/* if (player.PlBoundingBox.Intersects(incaTile.ITBoundingBox))
 {
     player.isColidingWithRight=true;
 }
 else
 {
     player.isColidingWithRight = false;
 }*/
