using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
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
            entity.Attach(new VisualComponent(texture));
            entity.Attach(new PhysicComponent(position, new Point((int)(size.X * scale.X), (int)(size.Y * scale.Y)), false, true));

            _physWorld.AddAsync(entity.Get<PhysicComponent>().Body); //Important

            return entity;
        }

        public Entity CreateSpike(Point2 position, Point size, Texture2D texture)
        {
            var entity = _world.CreateEntity();

            entity.Attach(new TransformComponent(position, 0, Vector2.One));
            entity.Attach(new VisualComponent(texture));

            return entity;
        }

        public Entity CreateFloor(Point2 position, Point size, Texture2D texture)
        {
            var floor = _world.CreateEntity();
            floor.Attach(new TransformComponent(position, 0, Vector2.One));

            var sprites = new List<ChildSprite>();
            var incaTile1Scale = new Vector2(.25f, .25f);
            var incaTile1Size = new Point((int)(texture.Width * incaTile1Scale.X), (int)(texture.Height * incaTile1Scale.X));
            var howManyTileToFileScreen = size.X / incaTile1Size.X;
            for (var i = 0; i < howManyTileToFileScreen; i++)
            {
                sprites.Add(new ChildSprite(new Sprite(texture),
                                            new Point2(i * (incaTile1Size.X), 0f),
                                            incaTile1Size));
            }
            floor.Attach(new VisualComponent(sprites));

            //Floor Physic
            floor.Attach(new PhysicComponent(position, new Point( size.X, incaTile1Size.Y )));
            _physWorld.AddAsync(floor.Get<PhysicComponent>().Body);

            return floor;
        }
    }
}
