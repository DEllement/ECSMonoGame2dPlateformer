using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TheGameProject.Components;

namespace TheGameProject.Entities
{
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
            entity.Attach(new TransformComponent(position, 0, Vector2.One));
            entity.Attach(new PhysicComponent(position, size, isAffectedByGravity));
            entity.Attach(new VisualComponent(color));

            _physWorld.AddAsync(entity.Get<PhysicComponent>().Body);

            return entity;
        }

        public Entity CreatePlayer(Point2 position, Point size, Texture2D texture)
        {
            var scale = 2.0f;

            var entity = _world.CreateEntity();
            entity.Attach(new TransformComponent(position, 0, new Vector2(scale,scale)));
            entity.Attach(new PhysicComponent(position, new Point((int)(size.X*scale), (int)(size.Y*scale)), true, true));
            entity.Attach(new VisualComponent(texture));
            entity.Attach(new UserInputComponent());
            entity.Attach(new PlayerDataComponent(entity.Id)); //WE assign the id in the special PlayerDataComponent

            _physWorld.AddAsync(entity.Get<PhysicComponent>().Body);

            return entity;
        }
        public Entity CreateIncaTile(Point2 position, Point size, Texture2D texture, Vector2 scale)
        {
            var entity = _world.CreateEntity();

            entity.Attach(new TransformComponent(position, 0, scale));
            entity.Attach(new PhysicComponent(position, size, false, true));
            entity.Attach(new VisualComponent(texture));

            _physWorld.AddAsync(entity.Get<PhysicComponent>().Body);

            return entity;
        }

        public Entity CreateSpike(Point2 position, Point size, Texture2D texture)
        {
            var entity = _world.CreateEntity();

            entity.Attach(new TransformComponent(position, 0, Vector2.One));
            entity.Attach(new PhysicComponent(position, size, false, true));
            entity.Attach(new VisualComponent(texture));

            return entity;
        }
    }
}
