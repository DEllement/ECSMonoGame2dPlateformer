using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using TheGameProject.Components;
using TheGameProject.Entities;

namespace TheGameProject.System
{
 
    public class InputSystem : EntityUpdateSystem
    {
        private ComponentMapper<UserInputComponent> _inputStates;
        private ComponentMapper<PlayerDataComponent> _playerDatas;

        public InputSystem() : base(Aspect.All(typeof(UserInputComponent))) //This will define WHICH Component is in activeEntities
        {

        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _inputStates = mapperService.GetMapper<UserInputComponent>();
            _playerDatas = mapperService.GetMapper<PlayerDataComponent>();

        }

        public override void Update(GameTime gameTime)
        {
            var player = GetEntity(_playerDatas.Components[0].PlayerId);
            var playerData = player.Get<PlayerDataComponent>();
            //condition for if even playing
            //TODO:pause Logic
           // if (!playerData.allGemCollected) {
                _inputStates.Components[0].IsLeftDown = Keyboard.GetState().IsKeyDown(Keys.A);
                _inputStates.Components[0].IsRightDown = Keyboard.GetState().IsKeyDown(Keys.D);
                _inputStates.Components[0].IsUpDown = Keyboard.GetState().IsKeyDown(Keys.W);
                _inputStates.Components[0].IsBottomDown = Keyboard.GetState().IsKeyDown(Keys.S);
                _inputStates.Components[0].IsSpaceDown = Keyboard.GetState().IsKeyDown(Keys.Space);
         //   }
           
        }
    }
}
