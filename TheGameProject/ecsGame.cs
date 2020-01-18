using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;
using MonoGame.Extended;
using System.Timers;
using TheGameProject.System;
using TheGameProject.Entities;

namespace TheGameProject
{

    public class ecsGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private World _world;
        internal EntityFactory _entityFactory { get; private set; }
        private Aether.Physics2D.Dynamics.World _physWorld;

        //1.
        public ecsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
        }

        //2. This is the Game Object Init Method, init variables here if needed....
        protected override void Initialize()
        {
            _physWorld = new Aether.Physics2D.Dynamics.World();
            base.Initialize();
        }

        //3. This is called on boot, and its for pre-loading resources (this is for loading Image, Mesh and other external data)
        protected override void LoadContent()
        {
            Texture2D playerTexture;

            _world = new WorldBuilder()
                       .AddSystem(new InputSystem())
                       .AddSystem(new AetherPhysicSystem(_physWorld))
                       .AddSystem(new RenderSystem(graphics.GraphicsDevice))
                       .Build();

            _entityFactory = new EntityFactory(_world, _physWorld);
            //TODO: initialize entities here
            var player    = _entityFactory.CreatePlayer  (new Point2(0, Window.ClientBounds.Height/3), 
                                                          new Point(Content.Load<Texture2D>("idle").Width*2, 
                                                          Content.Load<Texture2D>("idle").Height*2), 
                                                          Content.Load<Texture2D>("idle"));
            var incaTile1 = _entityFactory.CreateIncaTile(new Point2(-100, Window.ClientBounds.Height- Content.Load<Texture2D>("floor").Height / 4), 
                                                          new Point(Content.Load<Texture2D>("floor").Width*12, 
                                                          Content.Load<Texture2D>("floor").Height/4), 
                                                          Content.Load<Texture2D>("floor"));
            var incaBox1  = _entityFactory.CreateIncaTile(new Point2(300, Window.ClientBounds.Height - Content.Load<Texture2D>("floor").Height/4- Content.Load<Texture2D>("inca_tile01").Height*2),
                                                          new Point(Content.Load<Texture2D>("inca_tile01").Width*2, Content.Load<Texture2D>("inca_tile01").Height*2),
                                                          Content.Load<Texture2D>("inca_tile01"));
            var incaBox2  = _entityFactory.CreateIncaTile(new Point2(550, Window.ClientBounds.Height - Content.Load<Texture2D>("floor").Height / 4 - Content.Load<Texture2D>("inca_tile01").Height*2),
                                                          new Point(Content.Load<Texture2D>("inca_tile01").Width * 2, Content.Load<Texture2D>("inca_tile01").Height*2), 
                                                          Content.Load<Texture2D>("inca_tile01"));
            var spike1    = _entityFactory.CreateSpike   (new Point2(300+ Content.Load<Texture2D>("inca_tile01").Width*2, Window.ClientBounds.Height - Content.Load<Texture2D>("floor").Height / 4 - Content.Load<Texture2D>("inca_tile01").Height*2),
                                                          new Point(Content.Load<Texture2D>("inca_tile01").Width * 2, Content.Load<Texture2D>("inca_tile01").Height*2),
                                                          Content.Load<Texture2D>("pike02"));
            var spike2    = _entityFactory.CreateSpike    (new Point2(300 + Content.Load<Texture2D>("inca_tile01").Width * 2*2, Window.ClientBounds.Height - Content.Load<Texture2D>("floor").Height / 4 - Content.Load<Texture2D>("inca_tile01").Height*2),
                                                          new Point(Content.Load<Texture2D>("inca_tile01").Width * 2, Content.Load<Texture2D>("inca_tile01").Height*2),
                                                          Content.Load<Texture2D>("pike02"));
            var spike3    = _entityFactory.CreateSpike   (new Point2(300 + Content.Load<Texture2D>("inca_tile01").Width * 2*3, Window.ClientBounds.Height - Content.Load<Texture2D>("floor").Height / 4 - Content.Load<Texture2D>("inca_tile01").Height*2),
                                                          new Point(Content.Load<Texture2D>("inca_tile01").Width * 2, Content.Load<Texture2D>("inca_tile01").Height*2),
                                                          Content.Load<Texture2D>("pike02"));

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _world.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);

            base.Draw(gameTime);
        }
    }


        
}
