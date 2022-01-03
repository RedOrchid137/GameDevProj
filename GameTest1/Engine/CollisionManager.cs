using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using GameTest1.GameObjects;
using GameTest1.World;
using Microsoft.Xna.Framework;
using static GameTest1.GameObject;

namespace GameTest1.Engine
{
    public class CollisionManager
    {
        public static bool CheckCollision(Rectangle rect1, Rectangle rect2)
        {
            //Collision circumstances: (rect1.Bottom==rect2.Top||rect1.Top == rect2.Bottom)&&(rect1.X)    || rect1.Left == rect2.Right|| rect1.Right == rect2.Left
            
            if (rect1.Intersects(rect2))
            {
                
                return true;
            }
            return false;
        }
        public static void HandleCollisionsCharacter(GameObject player)
        {
            
            int spriteheight = (int)(player.curAnimation.CurrentFrame.SourceRectangle.Height*player.Scale);
            int spritewidth = (int)(player.curAnimation.CurrentFrame.SourceRectangle.Height * player.Scale);
            int topcount = 0;
            foreach (var item in player.CollisionList)
            {

                //Checken of tiles in lijst wel degelijk colliden met character
                //Indien niet: verwijderen uit de lijst zodat character vrij kan bewegen
                bool t = CheckCollision(player.CollisionRectangle, item.Key.CollisionRectangle);
                if (!t)
                {
                    player.TileRectList.Remove(item.Key.CollisionRectangle);
                    player.CollisionList.Remove(item.Key);
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
                            player.Speed = new Vector2(player.Speed.X, 0);
                            player.Ground = item.Key.CollisionRectangle.Top;
                            player.CurPosition = new Vector2(player.CurPosition.X, player.Ground-spriteheight);                     
                            topcount++;
                            break;


                        //Char raakt een tile aan de zijkant, de positie wordt geupdate om te zorgen dat de char niet door de tile kan lopen
                        case CollisionType.Left:
                            player.Speed = new Vector2(0, player.Speed.Y);
                            player.CurPosition = new Vector2(item.Key.CollisionRectangle.Left  - spritewidth + player.Offsets.X, player.CurPosition.Y);
                            break;
                        case CollisionType.Right:
                            player.Speed = new Vector2(0, player.Speed.Y);
                            player.CurPosition = new Vector2(item.Key.CollisionRectangle.Right - player.Offsets.X, player.CurPosition.Y);
                            break;
                        case CollisionType.Bottom:
                            player.Speed = new Vector2(player.Speed.X, 0);
                            player.CurPosition = new Vector2(player.CurPosition.X, item.Key.CollisionRectangle.Bottom+player.Offsets.Y);
                            break;
                    }

                }

            }
            
            if (player.CollisionList.Count==0)
            {
                
            }
            if(topcount > 0)
            {
                player.onGround = true;
            }
            else
            {
                player.onGround = false;
            }
        }
        public static bool AnyCollisionsCharacter(GameObject player)
        {
            if (player.CollisionList.Count != 0)
            {
                return true;
            }
            return false;
        }
    }
}
