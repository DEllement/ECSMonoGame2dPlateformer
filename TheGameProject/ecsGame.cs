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
using TheGameProject.Components;

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
            graphics.SynchronizeWithVerticalRetrace = false ;
            Content.RootDirectory = "Content";
            IsFixedTimeStep = false;
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
            var playerIdleTexture = Content.Load<Texture2D>("idle");
            var incaTile1Texture  = Content.Load<Texture2D>("floor");
            var incaTileBox01     = Content.Load<Texture2D>("inca_tile01_a");
            var incaPlate01Texture= Content.Load<Texture2D>("inca_tile15_a");
            var GreenGemTexture = Content.Load<Texture2D>("gem_green");
            var RedGemTexture = Content.Load<Texture2D>("gem_red");
            var BlueGemTexture = Content.Load<Texture2D>("gem_blue");
            var GoldGemTexture = Content.Load<Texture2D>("gem_gold");
            var youWin = Content.Load<SpriteFont>("YouWin");


            _world = new WorldBuilder()
                       .AddSystem(new InputSystem())
                       .AddSystem(new AetherPhysicSystem(_physWorld))
                       .AddSystem(new CollectableItemSystem())
                       .AddSystem(new PlayerStateSystem())
                       .AddSystem(new RenderSystem(graphics.GraphicsDevice, youWin))
                       .Build();

            _entityFactory = new EntityFactory(_world, _physWorld);

           

           //Player
            var player = _entityFactory.CreatePlayer(new Point2(0, Window.ClientBounds.Height / 3), new Point(playerIdleTexture.Width, playerIdleTexture.Height), playerIdleTexture);
            Console.WriteLine(player.Get<PhysicComponent>().BoundingBox.X);

            //Floor
            var incaTile1Scale = new Vector2(.25f, .25f);
            var floorHeight = incaTile1Texture.Height * incaTile1Scale.Y;
            var floorPosition = new Point2(0, Window.ClientBounds.Height - floorHeight);
            var floor = _entityFactory.CreateFloor(floorPosition, new Point(Window.ClientBounds.Width, (int)floorHeight), incaTile1Texture);

            //Boxes
            var boxScale = new Vector2(2f, 2f);
            var boxSize = new Point((int)(incaTileBox01.Width* boxScale.X), (int)(incaTileBox01.Height* boxScale.Y));
            var incaTile1 = _entityFactory.CreateIncaTile(new Point2(200, floorPosition.Y-boxSize.Y), boxSize, incaTileBox01, boxScale);
            var incaTile2 = _entityFactory.CreateIncaTile(new Point2(600, floorPosition.Y-boxSize.Y), boxSize, incaTileBox01, boxScale);

            //Plates
            var PlateSize = new Point((int)(incaPlate01Texture.Width * boxScale.X), (int)(incaPlate01Texture.Height * boxScale.Y));
            var incaPlate1 = _entityFactory.CreatePlate(new Point2(320,320 ), PlateSize, incaPlate01Texture, boxScale);
            var incaPlate2 = _entityFactory.CreatePlate(new Point2(420,240 ), PlateSize, incaPlate01Texture, boxScale);


            //Gems
            var gemSize = new Point((int)(RedGemTexture.Width * boxScale.X), (int)(RedGemTexture.Height * boxScale.Y));

            var GreenGem01 = _entityFactory.CreateGem(new Point2(700, floorPosition.Y - boxSize.Y+10), gemSize, RedGemTexture, boxScale, CollectibleItemType.RedGem);

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
