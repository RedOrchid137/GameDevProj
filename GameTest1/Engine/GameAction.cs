using GameTest1.Interfaces;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
namespace GameTest1.Engine
{
    public class GameAction<T>:IActor
    {
        public double ElapsedMilliSeconds { get; set; }

        public double ElapsedMilliSecondsInterval { get; set; }

        public double Duration { get; set; }

        public int RepeatInterval { get; set; }

        public double Interval { get; set; }

        public bool Repeat { get; set; }
        
        public double Start { get; set; } = 0;

        public double IntervalStart { get; set; } = 0;

        public bool Running { get; set; }

        public Func<T> toPerform;
        public GameAction(Func<T>toImplement,double duration,int repeatCount=0)
        {
            toPerform = toImplement;
            Duration = duration;
            if (repeatCount == 0)
            {
                Repeat = false;
            }
            else
            {
                RepeatInterval = repeatCount;
                Interval = Math.Round(Duration/RepeatInterval);
                Repeat = true;
            }
        }
        public void performAction()
        {
            toPerform();
        }

        public void Update(GameTime gametime)
        {
            if (Running==false&& Start != 0)
            {
                return;
            }
            double millisecs = gametime.TotalGameTime.TotalMilliseconds;
            if (Start == 0)
            {
                Start = millisecs;
                IntervalStart = millisecs;
                Running = true;
            }
            ElapsedMilliSeconds = millisecs - Start;

            if (Repeat)
            {
                ElapsedMilliSecondsInterval = millisecs - IntervalStart;
                if (ElapsedMilliSecondsInterval == 0)
                {
                    performAction();
                }
                if (ElapsedMilliSecondsInterval >= Interval)
                {
                    IntervalStart = millisecs;
                    performAction();
                }
            }
            else
            {
                performAction();
            }
            if (ElapsedMilliSeconds >= Duration)
            {
                Running = false;
            }
        }

    }
}
