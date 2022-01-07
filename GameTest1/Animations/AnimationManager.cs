using GameTest1.Abstracts;
using GameTest1.Enemies;
using GameTest1.Entities;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;
using static GameTest1.Animation;
using static GameTest1.Entity;

namespace GameTest1.Animations
{
    public class AnimationManager
    {
        private static KeyboardState state;
        private static Animation CurAnimation = new Animation();
        public static void setCurrentAnimationCharacter(Character o)
        {
            if (!o.Alive)
            {
                o.curAnimation = o.animationList[AnimationType.Death];
                return;
            }
            state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Up))
            {
                CurAnimation.Type = AnimationType.Jump;
            }
            else if (o.Speed.X != 0)
            {
                CurAnimation.Type = AnimationType.Run;
            }
            else
            {
                CurAnimation.Type = AnimationType.Idle;
            }

            if (o.Hit==true)
            {
                CurAnimation.Type = AnimationType.Hit;
            }
            o.curAnimation = o.animationList[CurAnimation.Type];
        }
        public static void setCurrentAnimationEnemy(Enemy o)
        {
            if (!o.Alive)
            {
                o.curAnimation = o.animationList[AnimationType.Death];
                return;
            }
            var hunter = o as HunterEnemy;
            if (o.Chasing==true && o.distanceToPlayer(o.CurLevel.Player.CollisionRectangle)<o.AttackRange&&!(hunter != null && hunter.ArrowCount <= 0))
            {      
                o.Attacking = true;
                o.curAnimation = o.animationList[AnimationType.Attack];
                return;
            }
            else if (o.Speed.Y < 0)
            {
                o.curAnimation = o.animationList[AnimationType.Jump];
            }
            else if (o.Speed.X!=0)
            {
                o.curAnimation = o.animationList[AnimationType.Run];
            }
            else
            {
                o.curAnimation = o.animationList[AnimationType.Idle];
            }
            o.Attacking = false;
        }
        public static void setCurrentAnimationObject(Collectible c)
        {

        }
    }
}
