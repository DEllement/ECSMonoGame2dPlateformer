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
using System.Timers;

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
        private Aether.Physics2D.Dynamics.World _physWorld;

        public static int playerId { get; set; }

        #region EntityFactory

        //Entities
        public Entity CreateBox(Point2 position, Point size, Color color, bool isAffectedByGravity=false)
        {
            var entity = _world.CreateEntity();
            entity.Attach(new PhysicComponent(position, size, isAffectedByGravity));
            entity.Attach(new VisualComponent(color));

            _physWorld.AddAsync(entity.Get<PhysicComponent>().Body);

            return entity;
        }

        public Entity CreatePlayer(Point2 position, Point size, Color color)
        {
            var entity = _world.CreateEntity();
           
            entity.Attach(new PhysicComponent(position, size, true,true));
            entity.Attach(new VisualComponent(color));
            entity.Attach(new UserInputComponent());
            entity.Attach(new PlayerDataComponent());

            _physWorld.AddAsync(entity.Get<PhysicComponent>().Body);

            return entity;
        }


        #endregion

        private GraphicsDeviceManager graphics;
        public CollisionEcsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;

            Content.RootDirectory = "Content";
        }
  

        protected override void Initialize()
        {
            _physWorld = new Aether.Physics2D.Dynamics.World();

            base.Initialize();
        }
        protected override void LoadContent()
        {
            _world = new WorldBuilder()
                .AddSystem(new InputSystem())
                .AddSystem(new AetherPhysicSystem(_physWorld))
                .AddSystem(new RenderSystem(graphics.GraphicsDevice))
                .Build();

            var player = CreatePlayer(new Point2(0, Window.ClientBounds.Height / 2), new Point(50, 50), Color.Yellow);
            playerId = player.Id;
       
            //Sample Layout
            CreateBox(new Point2 { X=0, Y = Window.ClientBounds.Height - 25 }, new Point(Window.ClientBounds.Width,25), Color.Aqua);
            CreateBox(new Point2 { X=100, Y = Window.ClientBounds.Height - 200 }, new Point(100,200), Color.Red);
            CreateBox(new Point2 { X=600, Y = Window.ClientBounds.Height - 100 }, new Point(100,100), Color.Orange);
            CreateBox(new Point2 { X=100, Y = 0 }, new Point(10,10), Color.Indigo, true);
            CreateBox(new Point2 { X=300, Y = 0 }, new Point(50,50), Color.Indigo, true);
            CreateBox(new Point2 { X=500, Y = 0 }, new Point(10,10), Color.Indigo, true);
            CreateBox(new Point2 { X=550, Y = 0 }, new Point(10,10), Color.Indigo, true);
            CreateBox(new Point2 { X=700, Y = 0 }, new Point(10,10), Color.Indigo, true);

            //Just to test the physic engine...
            Random rnd = new Random(0);
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += (sender, args) => 
            {
                var e = CreateBox(new Point2 { X=rnd.Next(0,600), Y = 0 }, new Point(rnd.Next(5,30),rnd.Next(5,30)), Color.FromNonPremultiplied(
                                                                                                                                rnd.Next(0,255),
                                                                                                                                rnd.Next(0,255),
                                                                                                                                rnd.Next(0,255),255), true);
                e.Get<PhysicComponent>().Body.FixedRotation = false;
            };
            timer.Start();
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

    }

  
    /*
    public class PhysicSystem : EntityUpdateSystem
    {
        private ComponentMapper<PhysicComponent> _physicComponentsMapper;
        private float velocityY = 100;
        private float velocityX = 100;
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

            Console.WriteLine(delta);

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
                    var willCollideWith = physicsComponents.FirstOrDefault(box => box != obj && box != player.Get<PhysicComponent>() && box.IsRigid && targetBB.Intersects(box.BoundingBox));
                    if (willCollideWith != null)
                    {
                        obj.Y = willCollideWith.Y - obj.Size.Y;
                        if (obj == player.Get<PhysicComponent>())
                            isPlayerCollidingWithAFloor = true;
                    }
                });

                physicsComponents.RemoveAll(physComp=> physComp == player.Get<PhysicComponent>());

                //move left
                if (player.Get<UserInputComponent>().IsLeftDown)
                {
                    var bb = player.Get<PhysicComponent>().BoundingBox;
                    var nextX = (int) (player.Get<PhysicComponent>().Position.X + velocityX * delta);
                    var targetBB = new Rectangle(nextX,(int)player.Get<PhysicComponent>().Position.Y, bb.Width,bb.Height);

                    //Can Player go left?
                    var willCollideWith = physicsComponents.FirstOrDefault(box => box.IsRigid && targetBB.Intersects(box.BoundingBox));
                    if (willCollideWith == null)
                        player.Get<PhysicComponent>().Position.X = nextX;
                    //else
                    //    player.Get<PhysicComponent>().X = willCollideWith.X + bb.Width;
                

                    
                }

                //move right
                if (player.Get<UserInputComponent>().IsRightDown)
                {
                    var bb = player.Get<PhysicComponent>().BoundingBox;
                    var nextX = (int) (player.Get<PhysicComponent>().Position.X - velocityX * delta);
                    var targetBB = new Rectangle(nextX,(int)player.Get<PhysicComponent>().Position.Y, bb.Width,bb.Height);

                    //Can Player go left?
                    var willCollideWith = physicsComponents.FirstOrDefault(box => box.IsRigid && targetBB.Intersects(box.BoundingBox));
                    if (willCollideWith == null)
                        player.Get<PhysicComponent>().X = nextX;
                    //else
                    //    player.Get<PhysicComponent>().X = willCollideWith.X + bb.Width;
                }

                //jumping
                if (player.Get<UserInputComponent>().IsSpaceDown && !player.Get<PlayerDataComponent>().IsJumping && isPlayerCollidingWithAFloor)
                {
                    var bb = player.Get<PhysicComponent>().BoundingBox;
                    var nextY = (int) (player.Get<PhysicComponent>().Position.Y - (velocityY * 2) * delta);
                    var targetBB = new Rectangle((int) player.Get<PhysicComponent>().X, nextY, bb.Width, bb.Height);

                    var canJump = !physicsComponents.Any(box => box.IsRigid && targetBB.Intersects(box.BoundingBox));
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
                    var targetBB = new Rectangle((int) player.Get<PhysicComponent>().X, nextY, bb.Width, bb.Height);

                    var isColliding = !physicsComponents.Any(box => box.IsRigid && targetBB.Intersects(box.BoundingBox));
                    if (isColliding && player.Get<PhysicComponent>().Y > jumpMaxY)
                        player.Get<PhysicComponent>().Y = nextY;
                    else
                        player.Get<PlayerDataComponent>().IsJumping = false;
                }

            }
        }
    }
    */

    //Render System
    public class RenderSystem : EntityDrawSystem
    {
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;

        private Texture2D fillTexture;
        //private ComponentMapper<VisualComponent> _colorComponentsMapper;
        //private ComponentMapper<PhysicComponent> _physicComponentMapper;

        public RenderSystem(GraphicsDevice graphicsDevice)
            : base(Aspect.All( 
                typeof(VisualComponent),
                typeof(PhysicComponent)))
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

                var physComp = entity.Get<PhysicComponent>();
                var color = entity.Get<VisualComponent>().Color;

                _spriteBatch.Draw(fillTexture, physComp.BoundingBox, null, color, physComp.Body.Rotation, new Vector2(0,0), SpriteEffects.None, 0f);
                if(physComp.BottomSensor != null)
                    _spriteBatch.Draw(fillTexture, physComp.BottomSensorBoundingBox, null, Color.BlueViolet, physComp.Body.Rotation, new Vector2(0,0), SpriteEffects.None, 0f);
            }

            //FIXME: declare myFont, make it work
            //_spriteBatch.DrawString(myFont, "Hello Centered", new Vector2(_graphicsDevice.DisplayMode.Width/2,_graphicsDevice.DisplayMode.Height/2), Color.DarkGray);

            _spriteBatch.End();
        }
    }
}