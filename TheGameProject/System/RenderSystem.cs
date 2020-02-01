using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Collections.Generic;
using System.Text;
using TheGameProject.Components;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content;

namespace TheGameProject.System
{
    public class RenderSystem : EntityDrawSystem
    {
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private SpriteFont YouWin;
        private Texture2D fillTexture;
        private ContentManager Content;


        public RenderSystem(GraphicsDevice graphicsDevice, ContentManager content)
            : base(Aspect.All(
                typeof(VisualComponent),
                typeof(TransformComponent)))
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            Content = content;
        }

        
        public override void Initialize(IComponentMapperService mapperService)
        {
            fillTexture = new Texture2D(_graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            fillTexture.SetData<Color>(new Color[] { Color.White });
            YouWin = Content.Load<SpriteFont>("YouWin");

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
                if (entity.Get<VisualComponent>() == null)
                    continue;

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


           _spriteBatch.DrawString(YouWin, "You Win", new Vector2(380, 200), Color.Red);
            _spriteBatch.End();
        }
    }
}