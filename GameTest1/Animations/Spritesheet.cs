using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTest1.Animations
{
    public class Spritesheet
    {
        private Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }



        private List<int> rowCounts;

        public List<int> RowCounts
        {
            get { return rowCounts; }
            set { rowCounts = value; }
        }
        private int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }
        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        private int spriteheight;

        public int Spriteheight
        {
            get { return spriteheight; }
            set { spriteheight = value; }
        }

        private int spritewidth;

        public int Spritewidth
        {
            get { return spritewidth; }
            set { spritewidth = value; }
        }
        private int max;

        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        public Spritesheet(Texture2D texture,List<int> rowcounts)
        {
            this.texture = texture;
            this.rowCounts = rowcounts;
            this.height = texture.Height;
            this.width = texture.Width;
            this.spriteheight = height / rowCounts.Count;
            this.max = rowCounts.Max();
            this.spritewidth = width / this.max;
        }


    }
}
