using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended.Entities;
using Newtonsoft.Json;
using TheGameProject.Components;
using TheGameProject.Entities;


namespace TheGameProject.System
{
    class PlayerStateSystem : MonoGame.Extended.Entities.Systems.EntityUpdateSystem
    {
        private ComponentMapper<PlayerDataComponent> _playerDatas;

        public PlayerStateSystem() : base(Aspect.All(typeof(CollectableItemComponent)))
        {
        }

        public override void Initialize(MonoGame.Extended.Entities.IComponentMapperService mapperService)
        {
            _playerDatas = mapperService.GetMapper<PlayerDataComponent>();
        }


        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var player = GetEntity(_playerDatas.Components[0].PlayerId);
            var playerData = player.Get<PlayerDataComponent>();

            if(player.Get<PlayerDataComponent>().totalRedGem==0 && player.Get<PlayerDataComponent>().totalGoldGem == 0
            && player.Get<PlayerDataComponent>().totalBlueGem == 0 && player.Get<PlayerDataComponent>().totalGreenGem == 0)
            {
                player.Get<PlayerDataComponent>().allGemCollected = true;
            }
        }
    }
}
