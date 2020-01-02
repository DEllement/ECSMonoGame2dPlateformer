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
       


        Player player;
        IncaTile TextureIncaTile;
        //1.
        public MyFuckingGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //2. This is the Game Object Init Method, init variables here if needed....
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            player = new Player();
            TextureIncaTile = new IncaTile();
            
            player.Health = 1000;
            
            base.Initialize();
        }

        //3. This is called on boot, and its for pre-loading resources (this is for loading Image, Mesh and other external data)
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player.LoadContent(Content);
            TextureIncaTile.LoadContent(Content);
            // TODO: use this.Content to load your game content here



            // TODO: use this.Content to load your game content here
        }

        #region GameLoop

        //4. This is the update method called many time per second, delta infos is available in gameTime.ElapsedGameTime
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
        

            base.Update(gameTime);
        }

        //5. This is the Render Method of the Game object
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black); //This paint the background black

            // TODO: Add your drawing code here
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            player.Draw(spriteBatch);

            TextureIncaTile.Draw(spriteBatch);

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
