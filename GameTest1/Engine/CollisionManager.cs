using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using GameTest1.GameObjects;
using Microsoft.Xna.Framework;
using static GameTest1.GameObject;

namespace GameTest1.Engine
{
    public class CollisionManager
    {
        public static bool CheckCollision(Rectangle rect1, Rectangle rect2)
        {
            if (rect1.Intersects(rect2))
            {
                Debug.WriteLine("collision");
                return true;
            }
            return false;
        }
        public static void HandleCollisionsCharacter(Character player)
        {
            foreach (var item in player.CollisionList)
            {
                if (item.Key.GetType() == typeof(Static))
                {
                    switch (item.Value)
                    {
                        case CollisionType.Top:
                            player.Speed = new Vector2(player.Speed.X, 0);
                            player.CurPosition = new Vector2(player.CurPosition.X, item.Key.CollisionRectangle.Top - player.curAnimation.CurrentFrame.SourceRectangle.Height * player.Scale);
                            break;
                        case CollisionType.Left:
                            player.Speed = new Vector2(0, player.Speed.Y);
                            player.CurPosition = new Vector2(item.Key.CollisionRectangle.Left - player.curAnimation.CurrentFrame.SourceRectangle.Width * player.Scale+player.Offsets.X, player.CurPosition.Y);
                            break;
                        case CollisionType.Right:
                            player.Speed = new Vector2(0, player.Speed.Y);
                            player.CurPosition = new Vector2(item.Key.CollisionRectangle.Right - player.Offsets.X, player.CurPosition.Y);
                            break;
                        case CollisionType.Bottom:
                            player.Speed = new Vector2(player.Speed.X, 0);
                            player.CurPosition = new Vector2(player.CurPosition.X, item.Key.CollisionRectangle.Bottom);
                            break;
                    }
                }

            }
        }
    }
}
