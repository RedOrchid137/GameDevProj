using GameTest1.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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


        //public void GetFramesFromTextureProperties(int width, int height, int maxnumberOfColummns, int rowsTotal,List<int>rowsCount,List<int> rowsNeeded)
        //{
        //    int widthOfFrame = width / maxnumberOfColummns;
        //    int heightOfFrame = height / rowsTotal;

        //    int curX;
        //    int curY = rowsNeeded[0]*heightOfFrame;
        //    for (int i = 0; i < rowsNeeded.Count; i++)
        //    {
        //        curX = 0;
        //        curY += i * heightOfFrame;
        //        for (int j = 0; j < rowsCount[rowsNeeded[i]]; j++)
        //        {
        //            //new Rectangle(curX,curY,widthOfFrame,heightOfFrame
        //            frames.Add(new AnimationFrame(new Rectangle(CalculateBounds.GetSmallestRectangleFromTexture())));
        //            curX += widthOfFrame;
        //        }
        //    }
        //}
        private Texture2D Crop(Texture2D image, Rectangle source)
        {
            //var graphics = image.GraphicsDevice;
            //var ret = new RenderTarget2D(graphics, source.Width, source.Height);
            //Texture2D retimg = (Texture2D)ret;
            //return retimg
            Texture2D cropTexture = new Texture2D(Game1.Graphics.GraphicsDevice, source.Width, source.Height);
            Color[] data = new Color[source.Width * source.Height];
            image.GetData(0, source, data, 0, data.Length);
            cropTexture.SetData(data);
            return cropTexture;
        }
        public void GetFramesFromTextureProperties(Spritesheet sheet, List<int> rowsNeeded)
        {
            int widthOfFrame = sheet.Width / sheet.Max;
            int heightOfFrame = sheet.Height / sheet.RowCounts.Count;

            int curX;
            int curY = rowsNeeded[0] * heightOfFrame;
            for (int i = 0; i < rowsNeeded.Count; i++)
            {
                curX = 0;
                curY += i * heightOfFrame;
                for (int j = 0; j < sheet.RowCounts[rowsNeeded[i]]; j++)
                {
                    //new Rectangle(curX,curY,widthOfFrame,heightOfFrame
                    Rectangle framerect = CalculateBounds.GetSmallestRectangleFromTexture(Crop(sheet.Texture, new Rectangle(curX, curY, widthOfFrame, heightOfFrame)));
                    Rectangle sourcerect = new Rectangle(curX + framerect.X,curY+framerect.Y,framerect.Width,framerect.Height);
                    frames.Add(new AnimationFrame(sourcerect));
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
