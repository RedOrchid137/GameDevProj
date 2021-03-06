using GameTest1.Abstracts;
using GameTest1.Animations;
using GameTest1.Enemies;
using GameTest1.Entities;
using GameTest1.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GameTest1.Entity;

namespace GameTest1
{
    public class Animation
    {
        public enum AnimationType { Run, Jump, Idle, Crouch, Attack, Block, Sleep, Death, Damage,Hit }

        private List<AnimationFrame> frames;
        private int curFrame;
        private AnimationFrame currentFrame;
        private AnimationFrame previousFrame;
        private int fps;
        internal bool Looping { get; set; }
        public AnimationType Type { get; set; }
        public Entity Owner { get; set; }
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
        internal AnimationFrame PreviousFrame { get { return previousFrame; } set { previousFrame = value; } }
        internal List<AnimationFrame> Frames { get { return frames; } set { frames = value; } }


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
                    Rectangle sourcerect = new Rectangle(curX + 1, curY + 1, widthOfFrame-1, heightOfFrame-1);
                    AnimationFrame curFrame = new AnimationFrame(sourcerect);
                    Rectangle smallestrect = CalculateBounds.GetSmallestRectangleFromTexture(ExtensionMethods.Crop(sheet.Texture, new Rectangle(curX, curY, widthOfFrame, heightOfFrame)));
                    Rectangle boundingbox = new Rectangle(curX + smallestrect.X, curY + smallestrect.Y, smallestrect.Width, smallestrect.Height);
                    curFrame.HitBox = boundingbox;
                    frames.Add(curFrame);
                    curX += widthOfFrame;
                }
            }
        }

        private double secondCounter = 0;

        public void Update(GameTime gameTime)
        {

            if (Owner.GetType() == typeof(HunterEnemy) && this.Type == AnimationType.Attack && (Owner as HunterEnemy).Attacking == false)
            {
                Reset();
            }

            CurrentFrame = frames[curFrame];

            secondCounter += gameTime.ElapsedGameTime.TotalSeconds;

            if (secondCounter >= 1d / fps)
            {
                curFrame++;
                secondCounter = 0;
            }

            if (curFrame >= frames.Count)
            {
                if(this.Type == AnimationType.Idle || this.Type == AnimationType.Run|| this.Type == AnimationType.Attack|| this.Type == AnimationType.Jump||this.Type == AnimationType.Hit)
                {
                    if (Owner.GetType()==typeof(HunterEnemy)&& this.Type == AnimationType.Attack&& (Owner as HunterEnemy).ShotsFired == false)
                    {
                        (Owner as HunterEnemy).ShotsFired = true;
                    }
                    else if((Owner as HunterEnemy)!=null)
                    {
                        (Owner as HunterEnemy).ShotsFired = false;
                    }
                    curFrame = 0;
                }
                else
                {
                    curFrame = frames.Count-1;
                }
                if (Owner.GetType().BaseType == typeof(Enemy) && this.Type == AnimationType.Death)
                {
                    (Owner as Enemy).ToBeRemoved = true;
                    Game2.SoundLibrary[GameBase.SoundType.EnemyHit].Play();
                }
                if (Owner.GetType() == typeof(Character) && this.Type == AnimationType.Death)
                {
                    (Owner as Character).EndGame = true;
                }
            }
        }
        public void Reset()
        {
            curFrame = 0;
        }

    }
}
