using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GameTest1.GameObject;

namespace GameTest1
{
    public class Animation
    {
        private List<AnimationFrame> frames;
        private int curFrame;
        private AnimationFrame currentFrame;
        private int fps;
        internal bool Looping { get; set; }
        public AnimationType Type { get; set; }

        public int Fps
        {
            get { return fps; }
            set { fps = value; }
        }


        public Animation(int fps = 15)
        {
            frames = new List<AnimationFrame>();
            curFrame = 0;
            this.fps = fps;
        }

        internal AnimationFrame CurrentFrame { get { return currentFrame; } set { currentFrame = value; } }
        internal List<AnimationFrame> Frames { get { return frames; } set { frames = value; } }

        
        public void GetFramesFromTextureProperties(int width, int height, int maxnumberOfColummns, int rowsTotal,List<int>rowsCount,List<int> rowsNeeded)
        {
            int widthOfFrame = width / maxnumberOfColummns;
            int heightOfFrame = height / rowsTotal+1;

            int curX;
            int curY = rowsNeeded[0]*heightOfFrame;
            for (int i = 0; i < rowsNeeded.Count; i++)
            {
                curX = 0;
                curY += i * heightOfFrame;
                for (int j = 0; j < rowsCount[rowsNeeded[i]]; j++)
                {
                    frames.Add(new AnimationFrame(new Rectangle(curX,curY,widthOfFrame,heightOfFrame)));
                    curX += widthOfFrame;
                }
            }
        }

        private double secondCounter = 0;

        public void Update(GameTime gameTime)
        {
            CurrentFrame = frames[curFrame];

            secondCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (secondCounter >= 1d / fps)
            {
                curFrame++;
                secondCounter = 0;
            }

            if (curFrame >= frames.Count)
            {
                if(this.Type == AnimationType.Idle || this.Type == AnimationType.Run)
                {
                    curFrame = 0;
                }
                else
                {
                    curFrame = frames.Count-1;
                }
                
            }
        }
        public void Reset()
        {
            curFrame = 0;
        }

    }
}
