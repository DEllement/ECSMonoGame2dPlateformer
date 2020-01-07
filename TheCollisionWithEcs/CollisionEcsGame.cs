using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Sprites;

//http://docs.monogameextended.net/Features/Entities/
namespace TheCollisionWithEcs
{
    //Components
    public class VisualComponent
    {
        public Color Color { get; set; }

        public VisualComponent(Color color)
        {
            Color = color;
        }
    }

    public class PhysicComponent
    {
        public Point2 Position { get; set; }
        public Point Size { get; set; }
        public Rectangle BoundingBox => new Rectangle((int)Position.X,(int)Position.Y, Size.X, Size.Y);
        public bool IsRigid { get; set; }
        public bool IsAffectedByGravity { get; set; }
        public int X
        {
            get => (int)Position.X;
            set => Position = new Point2(value,Position.Y);
        }
        public int Y
        {
            get => (int)Position.Y;
            set => Position = new Point2(Position.X, value);
        }

        public PhysicComponent( Point2 position, Point size,  bool isAffectedByGravity=false)
        {
            Position = position;
            Size = size;
            IsRigid = true;
            IsAffectedByGravity = isAffectedByGravity;
        }
    }

    public class UserInputComponent
    {
        public bool IsLeftDown { get; set; }
        public bool IsRightDown { get; set; }
        public bool IsUpDown { get; set; }
        public bool IsBottomDown { get; set; }
        public bool IsSpaceDown { get; set; }
    }

    public class PlayerDataComponent
    {
        public int Life { get; set; }
        public bool IsJumping { get; set; }
    }

    class CollisionEcsGame : Game
    {
        private World _world;

        public static int playerId { get; set; }

        #region EntityFactory

        //Entities
        public Entity CreateBox(Point2 position, Point size, Color color, bool isAffectedByGravity=false)
        {
            var entity = _world.CreateEntity();
            entity.Attach(new PhysicComponent(position, size, isAffectedByGravity));
            entity.Attach(new VisualComponent(color));
            return entity;
        }

        public Entity CreatePlayer(Point2 position, Point size, Color color)
        {
            var entity = _world.CreateEntity();
           
            entity.Attach(new PhysicComponent(position, size, true));
            entity.Attach(new VisualComponent(color));
            entity.Attach(new UserInputComponent());
            entity.Attach(new PlayerDataComponent());

            return entity;
        }


        #endregion

        private GraphicsDeviceManager graphics;
        public CollisionEcsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
           

            base.Initialize();
        }
        protected override void LoadContent()
        {
            _world = new WorldBuilder()
             //  .AddSystem(new InputSystem())
                .AddSystem(new PhysicSystem())
                .AddSystem(new RenderSystem(graphics.GraphicsDevice))
                .Build();

            var player = CreatePlayer(new Point2(0, Window.ClientBounds.Height / 2), new Point(50, 50), Color.Yellow);
            playerId = player.Id;

            CreateBox(new Point2 { X=0, Y = Window.ClientBounds.Height - 10 }, new Point(Window.ClientBounds.Width,10), Color.Aqua);
            CreateBox(new Point2 { X=100, Y = Window.ClientBounds.Height - 200 }, new Point(100,200), Color.Red);
            CreateBox(new Point2 { X=600, Y = Window.ClientBounds.Height - 100 }, new Point(100,100), Color.Orange);
            CreateBox(new Point2 { X=100, Y = 0 }, new Point(10,10), Color.Indigo, true);
            CreateBox(new Point2 { X=300, Y = 0 }, new Point(10,10), Color.Indigo, true);
            CreateBox(new Point2 { X=500, Y = 0 }, new Point(10,10), Color.Indigo, true);
            CreateBox(new Point2 { X=550, Y = 0 }, new Point(10,10), Color.Indigo, true);
            CreateBox(new Point2 { X=700, Y = 0 }, new Point(10,10), Color.Indigo, true);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            
            _world.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);

            base.Draw(gameTime);
        }

        //InputSystem
        private void UpdateInputs()
        {
            var player = _world.GetEntity(playerId);

            player.Get<UserInputComponent>().IsLeftDown   = Keyboard.GetState().IsKeyDown(Keys.D);
            player.Get<UserInputComponent>().IsRightDown  = Keyboard.GetState().IsKeyDown(Keys.A);
            player.Get<UserInputComponent>().IsUpDown     = Keyboard.GetState().IsKeyDown(Keys.W);
            player.Get<UserInputComponent>().IsBottomDown = Keyboard.GetState().IsKeyDown(Keys.S);
            player.Get<UserInputComponent>().IsSpaceDown  = Keyboard.GetState().IsKeyDown(Keys.Space);
        }
    }

    public class InputSystem : EntityUpdateSystem
    {
        public override void Initialize(IComponentMapperService mapperService)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            var player = GetEntity(CollisionEcsGame.playerId);
            player.Get<UserInputComponent>().IsLeftDown = Keyboard.GetState().IsKeyDown(Keys.D);
            player.Get<UserInputComponent>().IsRightDown = Keyboard.GetState().IsKeyDown(Keys.A);
            player.Get<UserInputComponent>().IsUpDown = Keyboard.GetState().IsKeyDown(Keys.W);
            player.Get<UserInputComponent>().IsBottomDown = Keyboard.GetState().IsKeyDown(Keys.S);
            player.Get<UserInputComponent>().IsSpaceDown = Keyboard.GetState().IsKeyDown(Keys.Space);
            base.Update(gameTime);
        }
    }

    public class PhysicSystem : EntityUpdateSystem
    {
        private ComponentMapper<PhysicComponent> _physicComponentsMapper;
        private float velocityY = 300;
        private float velocityX = 300;
        private int jumpMaxY = 0;

        public PhysicSystem() : base(Aspect.All( typeof(PhysicComponent)))
        {

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _physicComponentsMapper = mapperService.GetMapper<PhysicComponent>();
        }

        public override void Update(GameTime gameTime)
        {
            float delta = (float) gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            var player = GetEntity(CollisionEcsGame.playerId); //CHEAT!!!!
            bool isPlayerCollidingWithAFloor = false;

            foreach (var entityId in ActiveEntities)
            {
                // draw your entities
                var entity = GetEntity(entityId);
                if (!entity.Has<PhysicComponent>())
                    continue;

                var physicsComponents = _physicComponentsMapper.Components.ToList();

                //Gravity
                physicsComponents.Where(b=>b.IsAffectedByGravity)
                                 .ToList()
                                 .ForEach(obj =>
                {
                    var bb = obj.BoundingBox;
                    var nextY = (int) (obj.Position.Y + velocityY * delta);

                    var targetBB = new Rectangle((int)obj.Position.X,nextY,bb.Width,bb.Height);
                    obj.Y = nextY;
                    var willCollideWith = physicsComponents.FirstOrDefault(box => box != obj && box.IsRigid && targetBB.Intersects(box.BoundingBox));
                    if (willCollideWith != null)
                    {
                        obj.Y = willCollideWith.Y - obj.Size.Y;
                        if (obj == player.Get<PhysicComponent>())
                            isPlayerCollidingWithAFloor = true;
                    }
                });

                //move left
                if (player.Get<UserInputComponent>().IsLeftDown)
                {
                    var bb = player.Get<PhysicComponent>().BoundingBox;
                    var nextX = (int) (player.Get<PhysicComponent>().Position.X + velocityX * delta);
                    //var targetBB = new Rectangle(nextX,(int)player.Position.Y,bb.Width,bb.Height);

                    //Can Player go left?
                    if (!physicsComponents.Any(box =>  box.IsRigid && bb.Intersects(box.BoundingBox)))
                        player.Get<PhysicComponent>().X = nextX;
                }

                //move right
                if (player.Get<UserInputComponent>().IsRightDown)
                {
                    var bb = player.Get<PhysicComponent>().BoundingBox;
                    var nextX = (int) (player.Get<PhysicComponent>().Position.X - velocityX * delta);
                   // var targetBB = new Rectangle(nextX,(int)player.Position.Y,bb.Width,bb.Height);

                    //Can Player go right?
                    if (!physicsComponents.Any(box =>  box.IsRigid && bb.Intersects(box.BoundingBox)))
                        player.Get<PhysicComponent>().X = nextX;
                }

                //jumping
                if (player.Get<UserInputComponent>().IsSpaceDown && !player.Get<PlayerDataComponent>().IsJumping && isPlayerCollidingWithAFloor)
                {
                    var bb = player.Get<PhysicComponent>().BoundingBox;
                    var nextY = (int) (player.Get<PhysicComponent>().Position.Y - (velocityY * 2) * delta);
                    //var targetBB = new Rectangle((int) player.Position.X, nextY, bb.Width, bb.Height);

                    var canJump = !physicsComponents.Any(box => box.IsRigid && bb.Intersects(box.BoundingBox));
                    if (canJump)
                    {
                        player.Get<PlayerDataComponent>().IsJumping = true;
                        jumpMaxY = player.Get<PhysicComponent>().Y - 200;
                    }
                }

                if (player.Get<PlayerDataComponent>().IsJumping)
                {
                    var bb = player.Get<PhysicComponent>().BoundingBox;
                    var nextY = (int) (player.Get<PhysicComponent>().Position.Y - (velocityY * 2) * delta);
                    //var targetBB = new Rectangle((int) player.Position.X, nextY, bb.Width, bb.Height);

                    var isColliding = !physicsComponents.Any(box => box.IsRigid && bb.Intersects(box.BoundingBox));
                    if (isColliding && player.Get<PhysicComponent>().Y > jumpMaxY)
                        player.Get<PhysicComponent>().Y = nextY;
                    else
                        player.Get<PlayerDataComponent>().IsJumping = false;
                }

            }
        }
    }


    //Render System
    public class RenderSystem : EntityDrawSystem
    {
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;

        private Texture2D fillTexture;
        //private ComponentMapper<VisualComponent> _colorComponentsMapper;
        //private ComponentMapper<PhysicComponent> _physicComponentMapper;

        public RenderSystem(GraphicsDevice graphicsDevice)
            : base(Aspect.All( typeof(VisualComponent), typeof(PhysicComponent)))
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            //_colorComponentsMapper = mapperService.GetMapper<VisualComponent>();
            //_physicComponentMapper = mapperService.GetMapper<PhysicComponent>();

            fillTexture = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            fillTexture.SetData<Color>(new Color[] { Color.White });
        }

        public override void Draw(GameTime gameTime)
        {
            _graphicsDevice.Clear(Color.Black); //This paint the background black

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (var entityId in ActiveEntities)
            {
                // draw your entities
                var entity = GetEntity(entityId);
                if (!entity.Has<VisualComponent>())
                    continue;

                var bb = entity.Get<PhysicComponent>().BoundingBox;
                var color = entity.Get<VisualComponent>().Color;

                _spriteBatch.Draw(fillTexture, bb , color);
            }

            _spriteBatch.End();
        }
    }
}