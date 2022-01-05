using GameTest1.Abstracts;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Timers;
using static GameTest1.Entity;

namespace GameTest1.Animations
{
    public class AnimationManager
    {
        private static KeyboardState state;
        public static Keys CurKey { get; set; } = Keys.None;
        private static Timer Timer = new Timer(10000);
        private static Keys lastKey = CurKey;
        private static Animation TimerAnimation = new Animation();
        public static void setCurrentAnimationCharacter(Entity o)
        {
            o.curAnimation = o.animationList[TimerAnimation.Type];
            state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Up))
                {
                TimerAnimation.Type = AnimationType.Jump;
                CurKey = Keys.Up;
                }
                else if (state.IsKeyDown(Keys.Down))
                {
                TimerAnimation.Type = AnimationType.Crouch;
                CurKey = Keys.Down;
            }
                else if (o.Speed.X != 0)
                {
                TimerAnimation.Type = AnimationType.Run;
                CurKey = Keys.Right;
            }
            else if(!(o as Character).IsSleeping)
            {
                CurKey = Keys.None;
                TimerAnimation.Type = AnimationType.Idle;
                Timer.Start();
            }
            if(lastKey != CurKey)
            {
                if (o.curAnimation.Type != AnimationType.Idle)
                {
                    Timer.Stop();
                }
                o.curAnimation.Reset();
            }
            lastKey = CurKey;

        }
        public static void setCurrentAnimationEnemy(Enemy o)
        {
            if (o.Chasing == true)
            {
                Debug.WriteLine("Stats-------");
                Debug.WriteLine(o.distanceToPlayer(o.CurLevel.Player.CollisionRectangle));
                Debug.WriteLine(o.CollisionRectangle.Width);
            }

            if (o.Chasing==true && o.distanceToPlayer(o.CurLevel.Player.CollisionRectangle)<o.AttackRange)
            {
                o.curAnimation = o.animationList[AnimationType.Attack];
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
        }


        public static void setTimer()
        {
            Timer.Elapsed += OnTimerElapse;
            Timer.AutoReset = true;
        }
        private static void OnTimerElapse(Object source, ElapsedEventArgs e)
        {
            TimerAnimation.Type = AnimationType.Sleep;
        }
    }
}
