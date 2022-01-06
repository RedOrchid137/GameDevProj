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
        internal static List<GameAction<Object>> ActionList;
        internal static GameAction<Object> LavaDamage { get; set; }
        internal static GameAction<Object> HealPool { get; set; }

        public static bool CheckCollision(Rectangle rect1, Rectangle rect2)
        {
            //Collision circumstances: (rect1.Bottom==rect2.Top||rect1.Top == rect2.Bottom)&&(rect1.X)    || rect1.Left == rect2.Right|| rect1.Right == rect2.Left

            if (rect1.Intersects(rect2)||Touch(rect1,rect2))
            {

                return true;
            }
            return false;
        }

        public static bool Touch(Rectangle rect1, Rectangle rect2)
        {
            if (rect1.Left>rect2.Right||rect1.Top>rect2.Bottom||rect1.Right<rect2.Left||rect1.Bottom<rect2.Top)
            {
                return false;
            }
            return true;
        }
        public static void HandleCollisionsCharacter(Entity entity)
        {
            ActionList = new List<GameAction<Object>>();
            ActionList.Add(LavaDamage);
            ActionList.Add(HealPool);
            var player = entity as Character;
            float spriteheight = (entity.curAnimation.CurrentFrame.SourceRectangle.Height * entity.Scale);
            float spritewidth = (entity.curAnimation.CurrentFrame.SourceRectangle.Height * entity.Scale);
            int topcount = 0;
            int enemycount = 0;
            int interactcount = 0;
            foreach (var item in entity.CollisionList)
            {
                //if (item.Key.GetType().BaseType == typeof(Enemy)&&player != null)
                //{
                //    enemycount++;
                //}
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
                        //Dit wil zeggen dat de character er op moet blijven staan
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
                            player.Hit = true;
                            Func<Object> func = playerDamageFull;
                            LavaDamage = new GameAction<Object>(func, 1500,3);
                        }
                        
                        enemycount++;
                    }
                    else if (tile.Id == 4 && player != null)
                    {
                        if (player.Hit == false)
                        {
                            player.Hit = true;
                            Func<Object> func = playerDamageHalf;
                            LavaDamage = new GameAction<Object>(func, 6000, 6);
                        }
                        enemycount++;
                    }
                    if(tile.Id == 274 && player != null)
                    {
                        if (!player.Interacting)
                        {
                            Func<Object> func = playerHealHalf;
                            HealPool = new GameAction<Object>(func, 6000, 6);
                            player.Interacting = true;
                        }
                        interactcount++;
                    }
                    if (tile.Id == 104 && player != null)
                    {
                        if (player.Score >= Game2.CurLevel.RequiredScore)
                        {
                            Game2.lvlCount++;
                            if (Game2.lvlCount == Game2.lvlAmount)
                            {
                                Game2.self.Exit();
                            }
                            Game2.CurLevel = Game2.lvlList[Game2.lvlCount];
                            Game2.CurLevel.Player.StartingTile = Game2.CurLevel.StartingTile;
                        }
                    }
                }

                else if (item.Key.GetType().BaseType == typeof(Enemy)&& player!=null)
                {
                    var enemy = item.Key as Enemy;

                    if (enemy.Attacking&&!player.Hit)
                    {
                        player.Lives--;
                        player.Hit = true;
                    }
                    Debug.WriteLine(item.Value);
                    if (item.Value == CollisionType.Top)
                    {
                        enemy.Alive = false;
                    }
                    enemycount++;
                }
                else if (item.Key.GetType() == typeof(Collectible) && player != null)
                {
                    (item.Key as Collectible).PickedUp = true;
                    if (!player.Interacting)
                    {
                        player.Score += 1;
                        player.Lives += 0.5f;
                        player.Interacting = true;
                    }              
                    interactcount++;
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
                if (LavaDamage != null )
                {
                    LavaDamage = null;
                }
            }
            if (interactcount == 0 && player != null)
            {
                player.Interacting = false;
                if (HealPool != null)
                {
                    HealPool = null;
                }
            }
        }
        public static Object playerDamageFull()
        {
            Debug.WriteLine("Called Full");
            var player = Game2.CurLevel.Player;
            if (player.Hit == true)
            {
                player.Lives--;
            }
            return null;
        }
        public static Object playerDamageHalf()
        {
            Debug.WriteLine("Called Half");
            var player = Game2.CurLevel.Player;
            if (player.Hit == true)
            {
                player.Lives-=0.5f;
            }
            return null;
        }
        public static Object playerHealHalf()
        {
            Debug.WriteLine("Called Halfheal");
            var player = Game2.CurLevel.Player;
            player.Lives += 0.5f;
            return null;
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
