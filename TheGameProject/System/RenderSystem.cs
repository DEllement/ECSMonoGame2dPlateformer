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
                typeof(TransformComponent)))
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
                var t = entity.Get<TransformComponent>();

                Rectangle dest;
                switch (entity.Get<VisualComponent>().VisualComponentType)
                {
                    /*case VisualComponentType.Color: 

                        _spriteBatch.Draw(fillTexture, t.Position, null,entity.Get<VisualComponent>().Color, t.Rotation, new Vector2(0, 0), SpriteEffects.None, 0f);
                        break;*/
                    case VisualComponentType.Texture:
                        dest = new Rectangle( t.Position.ToPoint(), new Point( (int)(entity.Get<VisualComponent>().SpriteTexture.Width*t.Scale.X),
                                                                               (int)(entity.Get<VisualComponent>().SpriteTexture.Height*t.Scale.Y) )); //TODO: scale size...
                        _spriteBatch.Draw(entity.Get<VisualComponent>().SpriteTexture, dest, null,Color.White, t.Rotation, new Vector2(0, 0), SpriteEffects.None, 0f);
                        break;
                    case VisualComponentType.Sprite:
                        dest = new Rectangle( t.Position.ToPoint(), new Point( (int)(entity.Get<VisualComponent>().SpriteTexture.Width*t.Scale.X),
                                                                               (int)(entity.Get<VisualComponent>().SpriteTexture.Height*t.Scale.Y) )); //TODO: scale size...
                        _spriteBatch.Draw(entity.Get<VisualComponent>().SpriteTexture, dest, null, entity.Get<VisualComponent>().Sprite.Color, t.Rotation, entity.Get<VisualComponent>().Sprite.Origin, entity.Get<VisualComponent>().Sprite.Effect, entity.Get<VisualComponent>().Sprite.Depth );
                        break;
                    case VisualComponentType.SpritesList:

                        entity.Get<VisualComponent>().SpritesList.ForEach(v =>
                        {
                            var rect = new Rectangle((int)(v.position.X*t.Scale.X + t.Position.X),
                                                     (int)(v.position.Y*t.Scale.Y + t.Position.Y),
                                                     (int)(v.size.X+t.Scale.X),
                                                     (int)(v.size.Y+t.Scale.Y));
                            _spriteBatch.Draw(v.sprite.TextureRegion.Texture, rect , null,Color.White, t.Rotation, new Vector2(0, 0), SpriteEffects.None, 0f);
                        });
                        break;
                }

                
                
                //UnComment to see the Bottom Sensor of the player
                /*if (entity.Has<PlayerDataComponent>())
                {
                    Console.WriteLine(entity.Get<PhysicComponent>().BottomSensorBoundingBox);

                    _spriteBatch.Draw(fillTexture, entity.Get<PhysicComponent>().BottomSensorBoundingBox, null,
                        Color.BlueViolet, entity.Get<PhysicComponent>().Body.Rotation, new Vector2(0, 0),
                        SpriteEffects.None, 0f);
                }*/
            }

            //FIXME: declare myFont, make it work
            //_spriteBatch.DrawString(myFont, "Hello Centered", new Vector2(_graphicsDevice.DisplayMode.Width/2,_graphicsDevice.DisplayMode.Height/2), Color.DarkGray);

            _spriteBatch.End();
        }
    }
}
