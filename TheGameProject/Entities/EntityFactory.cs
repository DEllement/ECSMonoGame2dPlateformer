using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TheGameProject.Components;

namespace TheGameProject.Entities
{
    public class GameSharedVars { //GameSingleton
        public static int PlayerId;
    }


    class EntityFactory
    {
        private World _world;
        private Aether.Physics2D.Dynamics.World _physWorld;

        public EntityFactory(World world, Aether.Physics2D.Dynamics.World physWorld)
        {
            _world = world;
            _physWorld = physWorld;
        }

        public Entity CreateBox(Point2 position, Point size, Color color, bool isAffectedByGravity = false)
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

            entity.Attach(new PhysicComponent(position, size, true, true));
            entity.Attach(new VisualComponent(color));
            entity.Attach(new UserInputComponent());
            entity.Attach(new PlayerDataComponent(entity.Id)); //WE ALSO Assign the id in the special PlayerDataComponent
            GameSharedVars.PlayerId = entity.Id; //We make it globaly available (cheat)

            _physWorld.AddAsync(entity.Get<PhysicComponent>().Body);

            return entity;
        }
    }
}
