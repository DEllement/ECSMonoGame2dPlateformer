using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aether.Physics2D.Dynamics;
using Aether.Physics2D.Collision.Shapes;
using Aether.Physics2D.Common.Maths;
using MonoGame.Extended;
using Newtonsoft.Json;

namespace TheCollisionWithEcs
{
    public class AetherPhysicSystem : MonoGame.Extended.Entities.Systems.EntityUpdateSystem
    {
        private MonoGame.Extended.Entities.ComponentMapper<PhysicComponent> _physicComponentsMapper;
        private float velocityY = 100;
        private float velocityX = 100;
        private int jumpMaxY = 0;

        private World _world;

        public AetherPhysicSystem(World world) : base(MonoGame.Extended.Entities.Aspect.All( typeof(PhysicComponent)))
        {
            _world = world;
            _world.Gravity = new Aether.Physics2D.Common.Maths.Vector2(0f, 50f);
        }

        public override void Initialize(MonoGame.Extended.Entities.IComponentMapperService mapperService)
        {
            _physicComponentsMapper = mapperService.GetMapper<PhysicComponent>();

            
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            

            var player = GetEntity(CollisionEcsGame.playerId); //CHEAT!!!!
            bool isPlayerCollidingWithAFloor = false;

         
            //Check if we are on our feet
            if (player.Get<PhysicComponent>().Body.ContactList != null)
            {
                if(player.Get<PlayerDataComponent>().IsJumping && //Just check if jumping and player is touching the floor
                   Math.Abs(player.Get<PhysicComponent>().Body.ContactList.Other.Position.Y - (player.Get<PhysicComponent>().Body.Position.Y + player.Get<PhysicComponent>().Size.Y)) < 2f)
                    player.Get<PlayerDataComponent>().IsJumping = false;
            }

            

            if (!player.Get<PlayerDataComponent>().IsJumping && player.Get<UserInputComponent>().IsRightDown)
            {
                player.Get<PhysicComponent>().Body.ApplyForce(new Vector2(9999999f, 0));
            }

            if (!player.Get<PlayerDataComponent>().IsJumping && player.Get<UserInputComponent>().IsLeftDown)
            {
                player.Get<PhysicComponent>().Body.ApplyForce(new Vector2(-9999999f, 0));
            }

            if (!player.Get<PlayerDataComponent>().IsJumping && player.Get<UserInputComponent>().IsSpaceDown)
            {
                player.Get<PhysicComponent>().Body
                    .ApplyLinearImpulse(new Vector2(0f,-99999999f));
                player.Get<PlayerDataComponent>().IsJumping = true;
            }

            _world.Step(delta);


            /*
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
                        player.Get<PhysicComponent>().X = nextX;
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

            }*/
        }
    }
}
