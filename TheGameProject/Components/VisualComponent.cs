using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;


namespace TheGameProject.Components
{
    public class VisualComponent
    {
        public Texture2D SpriteTexture;
        public Color Color;

        public VisualComponent(Texture2D sprite)
        {
            SpriteTexture = sprite;
        }
        public VisualComponent(Color color)
        {
            Color = color;
        }
    }
}
