using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GameTest1.Abstracts;
using GameTest1.Enemies;
using GameTest1.Entities;
using GameTest1.Extensions;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using static GameTest1.Entities.Collectible;
using static GameTest1.Entity;

namespace GameTest1.Engine
{
    public class CollisionManager
    {
        internal static List<GameAction<Object>> ActionList;
        internal static GameAction<Object> LavaDamage { get; set; }
        internal static GameAction<Object> HealPool { get; set; }

        internal const int lavaID = 98;
        internal const int hotBlockID = 4;
        internal const int healPoolID = 236;
        internal const int levelExitID = 104;

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
                var obj = item.Key;

                var tile = item.Key as Tile;
                var enemy = item.Key as Enemy;
                var collectible = item.Key as Collectible;
                //Checken of tiles in lijst wel degelijk colliden met character
                //Indien niet: verwijderen uit de lijst zodat character vrij kan bewegen
                if (!CheckCollision(entity.CollisionRectangle, obj.CollisionRectangle))
                {
                    entity.TileRectList.Remove(obj.CollisionRectangle);
                    entity.CollisionList.Remove(obj);
                }

                if (tile!=null)
                {
                    switch (item.Value)
                    {
                        //Type van collision is top, de rectangle raakt de andere aan de bovenkant
                        //Dit wil zeggen dat de character er op moet blijven staan
                        //De Ground variabele geeft aan wat de y-waarde van de huidige "vloer" voor char is
                        //Met een boolean onGround kan nagegaan worden of de char op de grond staat en of deze dus moet vallen of niet
                        case CollisionType.Top:
                            entity.Speed = new Vector2(entity.Speed.X, 0);
                            entity.Ground = obj.CollisionRectangle.Top;
                            entity.CurPosition = new Vector2(entity.CurPosition.X, entity.Ground - spriteheight);
                            topcount++;
                            break;
                        //Char raakt een tile aan de zijkant, de positie wordt geupdate om te zorgen dat de char niet door de tile kan lopen
                        case CollisionType.Left:
                            entity.Speed = new Vector2(0, entity.Speed.Y);
                            //entity.CurPosition = new Vector2(item.Key.CollisionRectangle.Left  - spritewidth + entity.Offsets.X-5, entity.CurPosition.Y);
                            entity.CurPosition = new Vector2(entity.CurPosition.X - obj.IntersectSurface.Width, entity.CurPosition.Y);
                            break;
                        case CollisionType.Right:
                            entity.Speed = new Vector2(0, entity.Speed.Y);
                            //entity.CurPosition = new Vector2(item.Key.CollisionRectangle.Right - entity.Offsets.X+5, entity.CurPosition.Y);
                            entity.CurPosition = new Vector2(entity.CurPosition.X + obj.IntersectSurface.Width, entity.CurPosition.Y);
                            break;
                        case CollisionType.Bottom:
                            entity.Speed = new Vector2(entity.Speed.X, 0);
                            //entity.CurPosition = new Vector2(entity.CurPosition.X, item.Key.CollisionRectangle.Bottom+entity.Offsets.Y);
                            entity.CurPosition = new Vector2(entity.CurPosition.X, entity.CurPosition.Y + obj.IntersectSurface.Height);
                            break;
                    }
                }
               
                if (tile != null)
                {
                    if (tile.Id == lavaID && player != null)
                    {
                        if (player.Hit == false)
                        {
                            player.Hit = true;
                            Func<Object> func = playerDamageFull;
                            LavaDamage = new GameAction<Object>(func, 1500, 3);
                        }

                        enemycount++;
                    }
                    else if (tile.Id == hotBlockID && player != null)
                    {
                        if (player.Hit == false)
                        {
                            player.Hit = true;
                            Func<Object> func = playerDamageHalf;
                            LavaDamage = new GameAction<Object>(func, 6000, 6);
                        }
                        enemycount++;
                    }
                    if (tile.Id == healPoolID && player != null)
                    {
                        if (!player.Interacting)
                        {
                            Func<Object> func = playerHealHalf;
                            HealPool = new GameAction<Object>(func, 6000, 6);
                            player.Interacting = true;
                        }
                        interactcount++;
                    }
                    if (tile.Id == levelExitID && player != null)
                    {
                        //Game2.Victory = true;
                        //Game2.CurPlayer.EndGame = true;
                        if (player.Score >= Game2.CurLevel.RequiredScore&&!Game2.Switching)
                        {
                            Game2.SwitchLevel();
                        }
                    }
                }

                else if (enemy != null && player != null)
                {

                    if (enemy.Attacking && !player.Hit)
                    {
                        player.TakeDamage(1);
                        player.Hit = true;
                        if (enemy.FlipFlagX)
                        {
                            player.Speed = new Vector2(player.Speed.X-2, player.Speed.Y);
                        }
                        else
                        {
                            player.Speed = new Vector2(player.Speed.X + 2, player.Speed.Y);
                        }

                    }
                    if (item.Value == CollisionType.Top)
                    {
                        enemy.Alive = false;
                        player.Speed = new Vector2(player.Speed.X, 0);
                        player.Ground = obj.CollisionRectangle.Top;
                        player.CurPosition = new Vector2(player.CurPosition.X, player.Ground - spriteheight);
                        topcount++;
                    }
                    enemycount++;
                }
                else if (collectible != null && player != null)
                {
                    
                    if (!player.Interacting && collectible.Type==CollectibleType.Raspberry)
                    {
                        collectible.PickedUp = true;
                        Game2.SoundLibrary[GameBase.SoundType.Collect].Play();
                        player.Score += 1;
                        player.Heal(0.5f);
                        player.Interacting = true;
                    }
                    else if (!player.Interacting && collectible.Type == CollectibleType.BlueBerry)
                    {
                        Game2.SoundLibrary[GameBase.SoundType.SpeedUp].Play();
                        player.Speed = new Vector2(player.Speed.X, player.Speed.Y - 4f);
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
            if (enemycount == 0 && player != null)
            {
                player.Hit = false;
                if (LavaDamage != null)
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

        public static void HandleCollisionsProjectile(Arrow a)
        {
            foreach (var item in a.CollisionList)
            {
                var tile = item.Key as Tile;
                var player = item.Key as Character;

                if (tile != null)
                {
                    a.Hunter.CurArrow = null;
                }
                else if (player!=null)
                {
                    a.Hunter.CurArrow = null;
                    player.TakeDamage(1);
                    player.Hit = true;
                }
            }
        }

        #region EntityCollisionChecks
        public static void CollisionCheckFull(ObjectManager man,Entity en)
        {
            foreach (var item in man.ObjectList)
            {
                for (int i = 0; i < man.ObjectList.Count; i++)
                {
                    if (item != en)
                    {
                        if (CollisionManager.CheckCollision(en.CollisionRectangle, item.CollisionRectangle))
                        {
                            Rectangle intersectSurface = Rectangle.Intersect(en.CollisionRectangle, item.CollisionRectangle);

                            CollisionType test = new CollisionType();
                            var topdist = en.CollisionRectangle.Bottom - item.CollisionRectangle.Top;
                            var botdist = item.CollisionRectangle.Bottom - en.CollisionRectangle.Top;
                            var leftdist = en.CollisionRectangle.Right - item.CollisionRectangle.Left;
                            var rightdist = item.CollisionRectangle.Right - en.CollisionRectangle.Left;
                            int min = new[] { topdist, botdist, leftdist, rightdist }.Min();

                            if (min == topdist)
                            {
                                test = CollisionType.Top;
                            }
                            else if (min == botdist)
                            {
                                test = CollisionType.Bottom;
                            }
                            else if (min == leftdist)
                            {
                                test = CollisionType.Left;
                            }
                            else
                            {
                                test = CollisionType.Right;
                            }
                            //if (en.CollisionRectangle.Bottom > item.CollisionRectangle.Top && en.CollisionRectangle.Top < item.CollisionRectangle.Top && en.Speed.Y >= 0 && intersectSurface.Width > intersectSurface.Height)
                            //{
                            //    test = CollisionType.Top;
                            //}
                            //else if (en.CollisionRectangle.Top < item.CollisionRectangle.Bottom && en.CollisionRectangle.Bottom > item.CollisionRectangle.Bottom && en.Speed.Y < 0 && intersectSurface.Width > intersectSurface.Height)
                            //{
                            //    test = CollisionType.Bottom;
                            //}
                            //else if (en.CollisionRectangle.Right > item.CollisionRectangle.Left && en.CollisionRectangle.Right < item.CollisionRectangle.Right && en.Speed.X > 0 && intersectSurface.Width < intersectSurface.Height)
                            //{
                            //    test = CollisionType.Left;
                            //}
                            //else if (en.CollisionRectangle.Left < item.CollisionRectangle.Right && en.CollisionRectangle.Left > item.CollisionRectangle.Left && en.Speed.X < 0 && intersectSurface.Width < intersectSurface.Height)
                            //{
                            //    test = CollisionType.Right;
                            //}
                            if ((item as Enemy) != null)
                            {
                                (item as Enemy).IntersectSurface = intersectSurface;
                            }
                            en.CollisionList[item] = test;
                        }
                    }
                }
            }
        }
        public static void CollisionCheckTile(Rectangle CollisionRectangle, SpriteBatch sb, int id,Entity en)
        {
            if (en.TileRectList.Contains(CollisionRectangle))
            {
                return;
            }
            var item = new Tile(ExtensionMethods.BlankTexture(sb), CollisionRectangle, 1, id);
            item.CollisionRectangle = CollisionRectangle;

            if (CollisionManager.CheckCollision(en.CollisionRectangle, CollisionRectangle))
            {
                Rectangle intersectSurface = Rectangle.Intersect(en.CollisionRectangle, item.CollisionRectangle);
                CollisionType test = new CollisionType();
                var topdist = en.CollisionRectangle.Bottom - item.CollisionRectangle.Top;
                var botdist = item.CollisionRectangle.Bottom - en.CollisionRectangle.Top;
                var leftdist = en.CollisionRectangle.Right - item.CollisionRectangle.Left;
                var rightdist = item.CollisionRectangle.Right - en.CollisionRectangle.Left;
                int min = new[] { topdist, botdist, leftdist, rightdist }.Min();

                if (min == topdist)
                {
                    test = CollisionType.Top;
                }
                else if (min == botdist)
                {
                    test = CollisionType.Bottom;
                }
                else if (min == leftdist)
                {
                    test = CollisionType.Left;
                }
                else
                {
                    test = CollisionType.Right;
                }



                //if (en.CollisionRectangle.Bottom > item.CollisionRectangle.Top && en.CollisionRectangle.Top < item.CollisionRectangle.Bottom && en.Speed.Y > 0 && intersectSurface.Width > intersectSurface.Height)
                //{
                //    test = CollisionType.Top;
                //}
                //else if (en.CollisionRectangle.Top < item.CollisionRectangle.Bottom && en.CollisionRectangle.Bottom > item.CollisionRectangle.Top && en.Speed.Y<0 && intersectSurface.Width > intersectSurface.Height)
                //{
                //    test = CollisionType.Bottom;
                //}
                //else if(en.CollisionRectangle.Right > item.CollisionRectangle.Left && en.CollisionRectangle.Left < item.CollisionRectangle.Right && en.Speed.X>0 && intersectSurface.Width < intersectSurface.Height)
                //{
                //    test = CollisionType.Left;
                //}
                //else if(en.CollisionRectangle.Left < item.CollisionRectangle.Right && en.CollisionRectangle.Right > item.CollisionRectangle.Left && en.Speed.X < 0 && intersectSurface.Width < intersectSurface.Height)
                //{ 
                //    test = CollisionType.Right;
                //}
                item.IntersectSurface = intersectSurface;
                en.CollisionList[item] = test;
                if (!en.TileRectList.Contains(CollisionRectangle))
                {
                    en.TileRectList.Add(CollisionRectangle);
                }
            }
        }
        #endregion

        public static Object playerDamageFull()
        {
            var player = Game2.CurLevel.Player;
            if (player.Hit == true)
            {
                player.TakeDamage(1);
            }
            return null;
        }
        public static Object playerDamageHalf()
        {
            var player = Game2.CurLevel.Player;
            if (player.Hit == true)
            {
                player.TakeDamage(0.5f);
            }
            return null;
        }
        public static Object playerHealHalf()
        {
            var player = Game2.CurLevel.Player;
            player.Heal(0.5f);
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
