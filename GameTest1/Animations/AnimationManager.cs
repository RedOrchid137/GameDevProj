﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using static GameTest1.GameObject;

namespace GameTest1.Animations
{
    public class AnimationManager
    {
        private static KeyboardState state;
        public static Keys CurKey { get; set; } = Keys.None;
        private static Timer Timer = new Timer(10000);
        private static Keys lastKey = CurKey;
        private static Animation TimerAnimation = new Animation();
        public static void setCurrentAnimation(GameObject o)
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
