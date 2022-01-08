using GameTest1.Abstracts;
using GameTest1.Animations;
using GameTest1.Engine;
using GameTest1.Inputs;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static GameTest1.Animation;

namespace GameTest1.Enemies
{
    public class HunterEnemy : Enemy
    {
        public bool ShotsFired { get; set; } = false;
        public int ArrowCount { get; set; }
        public Arrow CurArrow { get; set; }
        public Texture2D ArrowTexture { get; set; }
        public float ArrowSpeed { get; set; } = 10;
        public HunterEnemy(Spritesheet spritesheet, Texture2D arrowSprite, Rectangle window, Level curlevel, Vector2 startingtile, Vector2 path,int arrowcount, float scale = 1, float maxSpeed = 5) : base(spritesheet, window, curlevel, startingtile, path, scale, maxSpeed)
        {
            this.Acceleration = new Vector2(0.2f, 5);
            AddAnimation(AnimationType.Idle, new List<int> { 2 });
            AddAnimation(AnimationType.Run, new List<int> { 3 });
            animationList[AnimationType.Run].Fps = 8;
            AddAnimation(AnimationType.Jump, new List<int> { 6 });
            AddAnimation(AnimationType.Attack, new List<int> { 0 });
            animationList[AnimationType.Attack].Fps = 7;
            AddAnimation(AnimationType.Death, new List<int> { 4 });
            animationList[AnimationType.Death].Fps = 8;
            this.curAnimation = animationList[AnimationType.Idle];
            this.Direction = false;
            this.FlipFlagX = true;
            this.ArrowCount = arrowcount;
            this.ArrowTexture = arrowSprite;
        }
        public override void Update(GameTime gametime, Level curLevel, SpriteBatch sb)
        {
            if (ShotsFired && CurArrow==null&&ArrowCount>0)
            {
                CurArrow = new Arrow(ArrowTexture,this.Window,1.5f);
                CurArrow.Hunter = this;
                CurArrow.CurPosition = new Vector2(this.CurPosition.X, this.CurPosition.Y+this.curAnimation.CurrentFrame.SourceRectangle.Height/1.12f);
                CurArrow.FlipFlagX = this.FlipFlagX;
                ArrowCount--;
                Game2.SoundLibrary[GameBase.SoundType.Shot].Play();
            }
            else if(CurArrow != null)
            {
                MovementManager.MoveArrow(CurArrow,curLevel,sb);
            }
            else
            {
                ShotsFired = false;
            }
            this.AttackRange = this.FieldOfView.Width;
            base.Update(gametime, curLevel, sb);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (this.CurArrow != null)
            {
                CurArrow.Draw(spriteBatch);
            }
        }
    }
}
