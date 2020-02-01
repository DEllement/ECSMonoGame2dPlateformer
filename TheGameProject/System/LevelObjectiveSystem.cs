using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended.Entities;
using Newtonsoft.Json;
using TheGameProject.Components;
using TheGameProject.Entities;


namespace TheGameProject.System
{
    class LevelObjectiveSystem : MonoGame.Extended.Entities.Systems.EntityUpdateSystem
    {
        private ComponentMapper<PlayerDataComponent> _playerDatas;

        public LevelObjectiveSystem() : base(Aspect.All(typeof(CollectableItemComponent)))
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
            //AllRedGemCollected;
            bool AGGC= playerData.CollectedGreenGem ==playerData.totalGreenGem;
            bool ARGC= playerData.CollectedRedGem ==playerData.totalRedGem;
            bool AGoGC=playerData.CollectedGoldGem ==playerData.totalGoldGem;
            bool ABGC = playerData.CollectedBlueGem ==playerData.totalBlueGem;

            if (AGGC && ABGC && ARGC && AGoGC)
            {
                Console.WriteLine(1);
                player.Get<PlayerDataComponent>().allGemCollected = true;
            }

        }
    }
}
