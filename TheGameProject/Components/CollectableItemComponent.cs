using System;
using System.Collections.Generic;
using System.Text;
using Aether.Physics2D.Common;
using Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;


namespace TheGameProject.Components
{
    public enum CollectibleItemType
    {
        BlueGem,
        RedGem,
        GreenGem,
        GoldGem
    }

    public class CollectableItemComponent
    {
        public Point Size { get; set; }
        public Point Position { get; set; }

        public bool IsCollected { get; set; }
        public CollectibleItemType ItemType { get; set; }

        public Rectangle BoundingBox => new Rectangle(Position, Size);

        public CollectableItemComponent(Point2 position, Point size, CollectibleItemType itemType)
        {
            ItemType = itemType;
            Position = new Point((int)position.X, (int)position.Y); 
            Size = size;
        }
    }
}
