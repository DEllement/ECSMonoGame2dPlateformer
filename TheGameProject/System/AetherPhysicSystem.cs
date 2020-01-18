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

            //Move Right
            if (player.Get<UserInputComponent>().IsRightDown)
                playerPhys.Body.ApplyForce(new Vector2(10f, 0.0f));

            //Move Left
            if (player.Get<UserInputComponent>().IsLeftDown)
                playerPhys.Body.ApplyForce(new Vector2(-10f, 0.0f));

            // Check if the player is currently jump
            foreach (var physicComponent in _physicComponents.Components)
            {
                if (physicComponent != playerPhys && physicComponent.BoundingBox.Intersects(playerPhys.BottomSensorBoundingBox))
                {
                    player.Get<PlayerDataComponent>().IsJumping = false;
                    break;
                }
            }

            // Do Jump if possible
            if (!player.Get<PlayerDataComponent>().IsJumping && player.Get<UserInputComponent>().IsSpaceDown)
            {
                playerPhys.Body.ApplyLinearImpulse(new Vector2(0f, -2f));
                player.Get<PlayerDataComponent>().IsJumping = true;
            }

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
