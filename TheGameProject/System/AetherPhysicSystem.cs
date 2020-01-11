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
using Newtonsoft.Json;
using TheGameProject.Components;
using TheGameProject.Entities;

namespace TheGameProject.System
{
    public class AetherPhysicSystem : MonoGame.Extended.Entities.Systems.EntityUpdateSystem
    {
        

        private MonoGame.Extended.Entities.ComponentMapper<PhysicComponent> _physicComponentsMapper;

        private World _world;

        private Body playerJumpZone;

        public AetherPhysicSystem(World world) : base(MonoGame.Extended.Entities.Aspect.All(typeof(PhysicComponent)))
        {
            _world = world;
            _world.Gravity = new Vector2(0f, 10f);
        }

        public override void Initialize(MonoGame.Extended.Entities.IComponentMapperService mapperService)
        {
            _physicComponentsMapper = mapperService.GetMapper<PhysicComponent>();


        }

        private float deltaVelocityYAtZero = 10000;
        private float delta = 0f;

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var player = GetEntity(GameSharedVars.PlayerId); //CHEAT!!!!
            var playerPhys = player.Get<PhysicComponent>();

            //Move Right
            if (player.Get<UserInputComponent>().IsRightDown)
                playerPhys.Body.ApplyForce(new Vector2(2f, 0.0f));

            //Move Left
            if (player.Get<UserInputComponent>().IsLeftDown)
                playerPhys.Body.ApplyForce(new Vector2(-2f, 0.0f));

            // Check if the player is currently jump
            foreach (var physicComponent in _physicComponentsMapper.Components)
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
                playerPhys.Body.ApplyLinearImpulse(new Vector2(0f, -3f));
                player.Get<PlayerDataComponent>().IsJumping = true;
            }

            _world.Step(delta);
        }
    }
}
