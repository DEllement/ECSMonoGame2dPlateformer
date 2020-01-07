using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

//http://docs.monogameextended.net/Features/Entities/
namespace TheCollisionWithEcs
{
    public class InputSystem : EntityUpdateSystem
    {
        public InputSystem() : base(Aspect.All(typeof(UserInputComponent))) //This will define WHICH Component is in activeEntities
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {

        }

        public override void Update(GameTime gameTime)
        {
            var player = GetEntity(CollisionEcsGame.playerId);
            player.Get<UserInputComponent>().IsLeftDown = Keyboard.GetState().IsKeyDown(Keys.D);
            player.Get<UserInputComponent>().IsRightDown = Keyboard.GetState().IsKeyDown(Keys.A);
            player.Get<UserInputComponent>().IsUpDown = Keyboard.GetState().IsKeyDown(Keys.W);
            player.Get<UserInputComponent>().IsBottomDown = Keyboard.GetState().IsKeyDown(Keys.S);
            player.Get<UserInputComponent>().IsSpaceDown = Keyboard.GetState().IsKeyDown(Keys.Space);

        }
    }
}