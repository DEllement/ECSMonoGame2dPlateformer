using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aether.Physics2D.Collision;
using Aether.Physics2D.Dynamics;
using Aether.Physics2D.Collision.Shapes;
using Aether.Physics2D.Common;
using Aether.Physics2D.Common.Maths;
using Aether.Physics2D.Common.PhysicsLogic;
using Aether.Physics2D.Controllers;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using Newtonsoft.Json;
using TheGameProject.Components;
using TheGameProject.Entities;
using World = Aether.Physics2D.Dynamics.World;

namespace TheGameProject.System
{
    public class AetherPhysicSystem : MonoGame.Extended.Entities.Systems.EntityUpdateSystem
    {
        private ComponentMapper<TransformComponent> _transformComponents;
        private ComponentMapper<PhysicComponent> _physicComponents;
        private ComponentMapper<PlayerDataComponent> _playerDatas;

        private World _world;

        private const float MAX_VELOCITY_X = 4f;
        private const float JUMP_MAX_VELOCITY_X = 1f;
        private const float JUMP_IMPULSE_Y = 1.5f;

        public AetherPhysicSystem(World world) : base(Aspect.All(typeof(PhysicComponent), typeof(TransformComponent)))
        {
            _world = world;
            _world.Gravity = new Vector2(0f, 10f);
        }

        public override void Initialize(MonoGame.Extended.Entities.IComponentMapperService mapperService)
        {
            _physicComponents = mapperService.GetMapper<PhysicComponent>();
            _playerDatas = mapperService.GetMapper<PlayerDataComponent>();
            _transformComponents = mapperService.GetMapper<TransformComponent>();
        }

        private float delta = 0f;

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var player = GetEntity(_playerDatas.Components[0].PlayerId);
            var playerPhys = player.Get<PhysicComponent>();
            var isJumping = player.Get<PlayerDataComponent>().IsJumping; 
            
            //Move Right
            var rightVelocity = Math.Abs(playerPhys.Body.LinearVelocity.Y) > 0.1f ? JUMP_MAX_VELOCITY_X : MAX_VELOCITY_X;
            if (player.Get<UserInputComponent>().IsRightDown && playerPhys.Body.LinearVelocity.X < rightVelocity)
                playerPhys.Body.ApplyForce(new Vector2(rightVelocity, 0.0f));

            //Move Left
            var leftVelocity = Math.Abs(playerPhys.Body.LinearVelocity.Y) > 0.1f ? -JUMP_MAX_VELOCITY_X : -MAX_VELOCITY_X;
            if (player.Get<UserInputComponent>().IsLeftDown && playerPhys.Body.LinearVelocity.X > leftVelocity)
                playerPhys.Body.ApplyForce(new Vector2(leftVelocity, 0.0f));

            // Check if the player is currently jump
            foreach (var physicComponent in _physicComponents.Components)
            {
                if (physicComponent != null && physicComponent != playerPhys && physicComponent.BoundingBox.Intersects(playerPhys.BottomSensorBoundingBox))
                {
                    if(playerPhys.Body.LinearVelocity.Y == 0.0f && !player.Get<UserInputComponent>().IsSpaceDown)
                        player.Get<PlayerDataComponent>().IsJumping = isJumping = false;
                    break;
                }
            }

            // Do Jump if possible
            if (!isJumping && player.Get<UserInputComponent>().IsSpaceDown)
            {
                playerPhys.Body.ApplyLinearImpulse(new Vector2(0f, -JUMP_IMPULSE_Y));
                player.Get<PlayerDataComponent>().IsJumping = isJumping = true;
            }
            // force faster fall
            if (isJumping && playerPhys.Body.LinearVelocity.Y > 0.5f)
                playerPhys.Body.LinearVelocity += new Vector2(0, 0.005f);

            _world.Step(delta);
         
            

            //Update All Transform Component
            foreach (var entityId in ActiveEntities)
            {
                var e = GetEntity(entityId);

                var newPos = e.Get<PhysicComponent>().Body.Position * PhysicComponent.physScale;

                e.Get<TransformComponent>().Position = new Microsoft.Xna.Framework.Vector2(newPos.X, newPos.Y);
                e.Get<TransformComponent>().Rotation = e.Get<PhysicComponent>().Body.Rotation;
            }
        }
    }
}
