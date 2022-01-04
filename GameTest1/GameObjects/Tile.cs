using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.GameObjects
{
    public class Tile : Static
    {
        public int Id { get; set; }
        public Rectangle IntersectSurface { get; set; }
        public Tile(Texture2D texture, Rectangle window, float scale,int id) : base(texture, window, scale)
        {
            this.Texture = texture;
            this.Window = window;
            this.Scale = scale;
            this.Id = id;
        }
    }
}
