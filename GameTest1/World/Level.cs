﻿using GameTest1.Extensions;
using GameTest1.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GameTest1.World
{
    public class Level
    {
        public static float screenWidth = Screen.PrimaryScreen.Bounds.Width/2;
        public static float screenHeight = Screen.PrimaryScreen.Bounds.Height/2;
        public TiledMap Map { get; set; }
        public TiledMapTileLayer CollisionLayer { get; set; }
        public Rectangle curCollisionRectangle { get; set; }
        public int TileWidth{ get;set;} 
        public Level(TiledMap map, Character player, int tilewidth)
        {
            this.Map = map;
            this.CollisionLayer = this.Map.TileLayers[0];
            TileWidth = tilewidth;
        }
        public Level(TiledMap map, int tilewidth)
        {
            this.Map = map;
            this.CollisionLayer = this.Map.TileLayers[0];
            TileWidth = tilewidth;
        }

        public void CheckCollision(GameObject entity, SpriteBatch sb)
        {
            List<TiledMapTile?> tilelist = new List<TiledMapTile?>();
            List<ushort> xvals = new List<ushort>();
            TiledMapTile? tile = null;
            ushort tx1 = (ushort)(entity.CollisionRectangle.X / TileWidth);
            ushort tx2 = (ushort)((entity.CollisionRectangle.X+ entity.CollisionRectangle.Width) / TileWidth);
            ushort tx3 = (ushort)((entity.CollisionRectangle.X+entity.CollisionRectangle.Width/2) / TileWidth);
            xvals.Add(tx1);
            xvals.Add(tx2);
            xvals.Add(tx3);
            ushort tybot = (ushort)(entity.CollisionRectangle.Bottom / TileWidth);
            ushort tytop = (ushort)(entity.CollisionRectangle.Top / TileWidth);


            //check bot
            foreach (var item in xvals)
            {
                Debug.WriteLine("Checking xval :   ---" + item);
                CollisionLayer.TryGetTile(item, tybot, out tile);
                if (tile.HasValue&&!tilelist.Contains(tile))
                {
                    tilelist.Add(tile);
                }             
            }

            //check top
            foreach (var item in xvals)
            {
                Debug.WriteLine("Checking xval :   ---" + item);
                CollisionLayer.TryGetTile(item, tytop, out tile);
                if (tile.HasValue && !tilelist.Contains(tile))
                {
                    tilelist.Add(tile);
                }
            }
            foreach (var item in tilelist)
            {
                Vector2 tileOrigin = new Vector2(item.Value.X * TileWidth, item.Value.Y * TileWidth);
                curCollisionRectangle = new Rectangle((int)tileOrigin.X, (int)tileOrigin.Y, TileWidth, TileWidth);
                if (!Game2.tilelist.Contains(curCollisionRectangle))
                {
                    Debug.WriteLine("adding tile at: "+ curCollisionRectangle.X + " " +curCollisionRectangle.Y);
                    Game2.tilelist.Add(curCollisionRectangle);
                }
                entity.CollisionCheckTile(curCollisionRectangle, sb, item.Value.GlobalIdentifier);
            }

        }

    }
}