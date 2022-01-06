using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using GameTest1.Abstracts;
using GameTest1.Entities;
using GameTest1.World;
using Microsoft.Xna.Framework;
using static GameTest1.Entity;

namespace GameTest1.Engine
{
    public class CollisionManager
    {

        internal static Action DoThis { get; set; }

        public static bool CheckCollision(Rectangle rect1, Rectangle rect2)
        {
            //Collision circumstances: (rect1.Bottom==rect2.Top||rect1.Top == rect2.Bottom)&&(rect1.X)    || rect1.Left == rect2.Right|| rect1.Right == rect2.Left

            if (rect1.Intersects(rect2))
            {

                return true;
            }
            return false;
        }
        public static void HandleCollisionsCharacter(Entity entity)
        {
            var player = entity as Character;
            float spriteheight = (entity.curAnimation.CurrentFrame.SourceRectangle.Height * entity.Scale);
            float spritewidth = (entity.curAnimation.CurrentFrame.SourceRectangle.Height * entity.Scale);
            int topcount = 0;
            int enemycount = 0;
            foreach (var item in entity.CollisionList)
            {
                if (item.Key.GetType().BaseType == typeof(Enemy)&&player != null)
                {
                    enemycount++;
                }
                var tile = item.Key as Tile;
                //Checken of tiles in lijst wel degelijk colliden met character
                //Indien niet: verwijderen uit de lijst zodat character vrij kan bewegen
                if (!CheckCollision(entity.CollisionRectangle, item.Key.CollisionRectangle))
                {
                    entity.TileRectList.Remove(item.Key.CollisionRectangle);
                    entity.CollisionList.Remove(item.Key);
                }
                if (item.Key.GetType() == typeof(Tile))
                {
                    switch (item.Value)
                    {
                        //Type van collision is top, de rectangle raakt de andere aan de bovenkant
                        //Dit wil zeggen dat wanneer de tile solid is, de character er op moet blijven staan
                        //De Ground variabele geeft aan wat de y-waarde van de huidige "vloer" voor char is
                        //Met een boolean onGround kan nagegaan worden of de char op de grond staat en of deze dus moet vallen of niet
                        case CollisionType.Top:
                            entity.Speed = new Vector2(entity.Speed.X, 0);
                            entity.Ground = tile.CollisionRectangle.Top;
                            entity.CurPosition = new Vector2(entity.CurPosition.X, entity.Ground - spriteheight);
                            topcount++;
                            break;


                        //Char raakt een tile aan de zijkant, de positie wordt geupdate om te zorgen dat de char niet door de tile kan lopen
                        case CollisionType.Left:
                            entity.Speed = new Vector2(0, entity.Speed.Y);
                            //entity.CurPosition = new Vector2(item.Key.CollisionRectangle.Left  - spritewidth + entity.Offsets.X-5, entity.CurPosition.Y);
                            entity.CurPosition = new Vector2(entity.CurPosition.X - tile.IntersectSurface.Width, entity.CurPosition.Y);
                            break;
                        case CollisionType.Right:
                            entity.Speed = new Vector2(0, entity.Speed.Y);
                            //entity.CurPosition = new Vector2(item.Key.CollisionRectangle.Right - entity.Offsets.X+5, entity.CurPosition.Y);
                            entity.CurPosition = new Vector2(entity.CurPosition.X + tile.IntersectSurface.Width, entity.CurPosition.Y);
                            break;
                        case CollisionType.Bottom:
                            entity.Speed = new Vector2(entity.Speed.X, 0);
                            //entity.CurPosition = new Vector2(entity.CurPosition.X, item.Key.CollisionRectangle.Bottom+entity.Offsets.Y);
                            entity.CurPosition = new Vector2(entity.CurPosition.X, entity.CurPosition.Y + tile.IntersectSurface.Height);
                            break;
                    }
                    if (tile.Id == 98 && player != null)
                    {
                        if (player.Hit == false)
                        {
                            player.Lives--;
                            player.Hit = true;
                        }
                        enemycount++;
                    }
                    else if (tile.Id == 4 && player != null)
                    {
                        if (player.Hit == false)
                        {
                            player.Lives -= 0.5f;
                            player.Hit = true;
                        }
                        enemycount++;
                    }
                }

                else if (item.Key.GetType().BaseType == typeof(Enemy)&& entity.GetType() == typeof(Character))
                {
                    var enemy = item.Key as Enemy;

                    if (enemy.Attacking&&!player.Hit)
                    {
                        player.Lives--;
                        player.Hit = true;
                    }
                }


            }
            if (topcount > 0)
            {
                entity.onGround = true;
            }
            else
            {
                entity.onGround = false;
            }
            if (enemycount==0 && player != null)
            {
                player.Hit = false;
            }
        }

        public Object playerLivesFull(Character player)
        {

        }

        public static bool AnyCollisionsCharacter(Entity entity)
        {
            if (entity.CollisionList.Count != 0)
            {
                return true;
            }
            return false;
        }
    }
}
