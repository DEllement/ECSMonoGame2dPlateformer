using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace TheCollision
{
    //Entities
    public class Box
    {
        //Visual Component
        public Color Color { get; set; }

        //Physic Component
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

        public Box( Point2 position, Point size, Color color, bool isAffectedByGravity=false)
        {
            Color = color;
            Position = position;
            Size = size;
            IsRigid = true;
            IsAffectedByGravity = isAffectedByGravity;
        }
    }

    public class Player : Box
    {
        //InputComponent
        public bool IsLeftDown { get; set; }
        public bool IsRightDown { get; set; }
        public bool IsUpDown { get; set; }
        public bool IsBottomDown { get; set; }
        public bool IsSpaceDown { get; set; }

        //PlayerDataComponent
        public bool IsJumping { get; set; }

        public Player(Point2 position, Point size, Color color) : base(position, size, color)
        {
            IsAffectedByGravity = true;
        }
    }


    class CollisionGame : Game
    {
        private Texture2D fillTexture;


        private GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        private List<Box> boxes;
        private List<Box> gravityObjects;
        private Player player;


        public CollisionGame()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            player = new Player(new Point2(0,Window.ClientBounds.Height/2), new Point(50,50), Color.Yellow);

            boxes = new List<Box>();
            boxes.Add(new Box(new Point2 { X=0, Y = Window.ClientBounds.Height - 10 }, new Point(Window.ClientBounds.Width,10), Color.Aqua));
            boxes.Add(new Box(new Point2 { X=100, Y = Window.ClientBounds.Height - 200 }, new Point(100,200), Color.Red));
            boxes.Add(new Box(new Point2 { X=600, Y = Window.ClientBounds.Height - 100 }, new Point(100,100), Color.Orange));

            boxes.Add(new Box(new Point2 { X=100, Y = 0 }, new Point(10,10), Color.Indigo, true));
            boxes.Add(new Box(new Point2 { X=300, Y = 0 }, new Point(10,10), Color.Indigo, true));
            boxes.Add(new Box(new Point2 { X=500, Y = 0 }, new Point(10,10), Color.Indigo, true));
            boxes.Add(new Box(new Point2 { X=550, Y = 0 }, new Point(10,10), Color.Indigo, true));
            boxes.Add(new Box(new Point2 { X=700, Y = 0 }, new Point(10,10), Color.Indigo, true));

            gravityObjects = boxes.Where(box => box.IsAffectedByGravity).ToList();
            gravityObjects.Add(player);

            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            fillTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            fillTexture.SetData<Color>(new Color[] { Color.White });

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            UpdateInputs();
            UpdatePhysics(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            UpdateRender(gameTime);

            base.Draw(gameTime);
        }

        //InputSystem
        private void UpdateInputs()
        {
            player.IsLeftDown   = Keyboard.GetState().IsKeyDown(Keys.D);
            player.IsRightDown  = Keyboard.GetState().IsKeyDown(Keys.A);
            player.IsUpDown     = Keyboard.GetState().IsKeyDown(Keys.W);
            player.IsBottomDown = Keyboard.GetState().IsKeyDown(Keys.S);
            player.IsSpaceDown  = Keyboard.GetState().IsKeyDown(Keys.Space);
        }

        
        //Physics System
        private float velocityY = 300;
        private float velocityX = 300;
        private int jumpMaxY = 0;

        private void UpdatePhysics(GameTime gameTime)
        {
            float delta = (float) gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            bool isPlayerCollidingWithAFloor = false;

            //Gravity
            gravityObjects.ForEach(obj =>
            {
                var bb = obj.BoundingBox;
                var nextY = (int) (obj.Position.Y + velocityY * delta);

                var targetBB = new Rectangle((int)obj.Position.X,nextY,bb.Width,bb.Height);
                obj.Y = nextY;
                var willCollideWith = boxes.FirstOrDefault(box => box != obj && box.IsRigid && targetBB.Intersects(box.BoundingBox));
                if (willCollideWith != null)
                {
                    obj.Y = willCollideWith.Y - obj.Size.Y;
                    if (obj == player)
                        isPlayerCollidingWithAFloor = true;
                }
            });

            //move left
            if (player.IsLeftDown)
            {
                    
                    var bb = player.BoundingBox;
                    var nextX = (int)(player.Position.X + velocityX * delta);

                    var targetBB = new Rectangle(nextX, (int)player.Position.Y, bb.Width, bb.Height);
                    player.X = nextX;
                    var willCollideWith = boxes.FirstOrDefault(box => box.IsRigid && targetBB.Intersects(box.BoundingBox));
                    if (willCollideWith != null)
                    {
                        player.X = willCollideWith.X - player.Size.X;
                       
                    }
                
            }

            //move right
            if (player.IsRightDown)
            {
                var bb = player.BoundingBox;
                var nextX = (int)(player.Position.X - velocityX * delta);
                var targetBB = new Rectangle(nextX, (int)player.Position.Y, bb.Width, bb.Height);
                player.X = nextX;
                var willCollideWith = boxes.FirstOrDefault(box => box.IsRigid && targetBB.Intersects(box.BoundingBox));
                if (willCollideWith != null)
                {
                    Console.WriteLine(123);
                    player.X = willCollideWith.X +willCollideWith.Size.X;

                }
            }

            //jumping
            if (player.IsUpDown && !player.IsJumping && isPlayerCollidingWithAFloor)
            {
                var bb = player.BoundingBox;
                var nextY = (int) (player.Position.Y - (velocityY * 2) * delta);
                //var targetBB = new Rectangle((int) player.Position.X, nextY, bb.Width, bb.Height);

                var canJump = !boxes.Any(box => box.IsRigid && bb.Intersects(box.BoundingBox));
                if (canJump)
                {
                    player.IsJumping = true;
                    jumpMaxY = player.Y - 200;
                }
            }

            if (player.IsJumping)
            {
                var bb = player.BoundingBox;
                var nextY = (int) (player.Position.Y - (velocityY * 2) * delta);
                //var targetBB = new Rectangle((int) player.Position.X, nextY, bb.Width, bb.Height);

                var isColliding = !boxes.Any(box => box.IsRigid && bb.Intersects(box.BoundingBox));
                if (isColliding && player.Y > jumpMaxY)
                    player.Y = nextY;
                else
                    player.IsJumping = false;
            }

        }

        //Render System
        private void UpdateRender(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black); //This paint the background black

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            boxes.ForEach(box =>
            {
                spriteBatch.Draw(fillTexture, box.BoundingBox , box.Color);
            });
            spriteBatch.Draw(fillTexture, player.BoundingBox , player.Color);

            spriteBatch.End();
        }
    }
}
