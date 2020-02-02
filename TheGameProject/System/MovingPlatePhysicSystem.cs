using System;
using System.Collections.Generic;
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


namespace TheGameProject.Entities
{
    class MovingPlatePhysicSystem : MonoGame.Extended.Entities.Systems.EntityUpdateSystem
    {
        private ComponentMapper<VerticalMovingPlateComponent> _platePhys;

        public MovingPlatePhysicSystem() : base(Aspect.All(new[]{
                                                                    typeof(VerticalMovingPlateComponent),
                                                                    typeof(PhysicComponent)
                                                                }))
        {}

        public override void Initialize(MonoGame.Extended.Entities.IComponentMapperService mapperService)
        {
            _platePhys = mapperService.GetMapper<VerticalMovingPlateComponent>();
        }


        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            foreach (var eId in ActiveEntities)
            {
                var plate = GetEntity(eId);
                plate.Get<TransformComponent>().Position += new Microsoft.Xna.Framework.Vector2 (0f,0.05f);
                //Console.WriteLine( plate.Get<TransformComponent>().Position);
            }
        }
    }
}
