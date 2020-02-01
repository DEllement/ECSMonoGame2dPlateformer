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

            foreach (var entityId in ActiveEntities)
            {
                var collItemEntity = GetEntity(entityId);
                var collItemComp = collItemEntity.Get<CollectableItemComponent>();
                if (!collItemComp.IsCollected && collItemComp.BoundingBox.Intersects(playerPhys.BoundingBox))
                {
                    collItemComp.IsCollected = true;
                    switch (collItemComp.ItemType)
                    {
                        case CollectibleItemType.RedGem:
                            player.Get<PlayerDataComponent>().CollectedRedGem++;
                            break;
                        case CollectibleItemType.GoldGem:
                            player.Get<PlayerDataComponent>().CollectedGoldGem++;
                            break;
                        case CollectibleItemType.BlueGem:
                            player.Get<PlayerDataComponent>().CollectedBlueGem++;
                            break;
                        case CollectibleItemType.GreenGem:
                            player.Get<PlayerDataComponent>().CollectedGreenGem++;
                            break;
                    }
                    //Remove the entity completly or...
                    //this.DestroyEntity(entityId);

                    //or Remove the visual component only...better cause we can use it in the objective system
                    collItemComp.IsCollected = true;
                    collItemEntity.Detach<VisualComponent>(); 
                }
            }
        }
    }
}

