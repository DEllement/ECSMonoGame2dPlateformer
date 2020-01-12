using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Text;
using TheGameProject.Components;

namespace TheGameProject.System
{
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
                var color = entity.Get<VisualComponent>().SpriteTexture;

                _spriteBatch.Draw(color, physComp.BoundingBox, null,Color.White, physComp.Body.Rotation, new Vector2(0, 0), SpriteEffects.None, 0f);
                //if (physComp.BottomSensor != null)
                //    _spriteBatch.Draw(fillTexture, physComp.BottomSensorBoundingBox, null, Color.BlueViolet, physComp.Body.Rotation, new Vector2(0, 0), SpriteEffects.None, 0f);
            }

            //FIXME: declare myFont, make it work
            //_spriteBatch.DrawString(myFont, "Hello Centered", new Vector2(_graphicsDevice.DisplayMode.Width/2,_graphicsDevice.DisplayMode.Height/2), Color.DarkGray);

            _spriteBatch.End();
        }
    }
}
