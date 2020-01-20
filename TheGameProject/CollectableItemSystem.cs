using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended.Entities;
using Newtonsoft.Json;
using TheGameProject.Components;
using TheGameProject.Entities;
namespace TheGameProject
{
    class CollectableItemSystem : MonoGame.Extended.Entities.Systems.EntityUpdateSystem
    {
        private ComponentMapper<CollectableItemComponent> _CollectableItemComponents;
        private ComponentMapper<PlayerDataComponent> _playerDatas;

        public CollectableItemSystem():base(Aspect.All(typeof(CollectableItemComponent)))
        {
        }

        public override void Initialize(MonoGame.Extended.Entities.IComponentMapperService mapperService)
        {
            _CollectableItemComponents = mapperService.GetMapper<CollectableItemComponent>();
            _playerDatas = mapperService.GetMapper<PlayerDataComponent>();
        }

        private float delta = 0f;

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var player = GetEntity(_playerDatas.Components[0].PlayerId);
            var playerPhys = player.Get<PhysicComponent>();

            foreach (var CollComp in _CollectableItemComponents.Components)
            {
                if (CollComp!= null) {
                  //  Console.WriteLine(CollComp.BoundingBox);
                    if (CollComp.BoundingBox.Intersects(playerPhys.BottomSensorBoundingBox))
                        Console.WriteLine(CollComp.BoundingBox);
                } 
            }
        }
    }
}

