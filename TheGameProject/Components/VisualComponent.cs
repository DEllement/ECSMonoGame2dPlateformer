using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;


namespace TheGameProject.Components
{
    public enum VisualComponentType
    {
        Color,
        Texture,
        Sprite,
        SpritesList,
        Text
    }

    public class ChildSprite
    {
        public Sprite sprite { get; set; }
        public Vector2 position { get; set; }
        public Point size { get; set; }

        public ChildSprite(Sprite sprite, Vector2 position, Point size)
        {
            this.sprite = sprite;
            this.position = position;
            this.size = size;
        }
    }

    public class VisualComponent
    {
        public VisualComponentType VisualComponentType { get; private set; }
        public Texture2D SpriteTexture;
        public Color Color;
        public Sprite Sprite;
        public List<ChildSprite> SpritesList;

        public VisualComponent(Texture2D sprite)
        {
            SpriteTexture = sprite;
            VisualComponentType = VisualComponentType.Texture;
        }
        public VisualComponent(Color color)
        {
            Color = color;
            VisualComponentType = VisualComponentType.Color;
        }
        public VisualComponent(Sprite sprite)
        {
            Sprite = sprite;
            VisualComponentType = VisualComponentType.Sprite;
        }
        public VisualComponent(List<ChildSprite> spritesList)
        {
            SpritesList = spritesList;
            VisualComponentType = VisualComponentType.SpritesList;
        }
    }
}
