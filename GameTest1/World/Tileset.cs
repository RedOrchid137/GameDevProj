using GameTest1.Extensions;
using GameTest1.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameTest1.World
{
    public class Tileset
    {
        public enum TileType { Floor, Wall, Tree, Trap, Platform, Hanging, Stone}

        public Dictionary<TileType, Static> Tiles;

        private Texture2D _texture;


        private void getTilesFromString(string tilestring)
        {
            List<string> tilelist = tilestring.Split(';').ToList();
            Rectangle currect = new Rectangle();
            Texture2D curtexture;
            foreach (var tile in tilelist)
            {
                currect = getRectangleFromString(tile.Substring(1));
                curtexture = ExtensionMethods.Crop(_texture, currect);
                switch (tile.ToLower()[0])
                {
                    case 'f':
                        Tiles.Add(TileType.Floor, new Static(curtexture, currect,1));
                        break;
                    case 'p':
                        Tiles.Add(TileType.Platform, new Static(curtexture, currect, 1));
                        break;
                    case 't':
                        Tiles.Add(TileType.Tree, new Static(curtexture, currect, 1));
                        break;
                    case 'h':
                        Tiles.Add(TileType.Hanging, new Static(curtexture, currect, 1));
                        break;
                    case 's':
                        Tiles.Add(TileType.Stone, new Static(curtexture, currect, 1));
                        break;
                    case 'x':
                        Tiles.Add(TileType.Trap, new Static(curtexture, currect, 1));
                        break;
                }
            }
        }
        private Rectangle getRectangleFromString(string rectstring)
        {
            List<string> Coords = rectstring.Split(',').ToList();
            Rectangle r = new Rectangle(int.Parse(Coords[0]),int.Parse(Coords[1]),int.Parse(Coords[2]),int.Parse(Coords[3]));
            return r;
        }

        public Tileset(Texture2D texture, string tilestring)
        {
            this._texture = texture;
            Tiles =  new Dictionary<TileType, Static>();
            getTilesFromString(tilestring);
        }
        

    }
}
