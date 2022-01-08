using GameTest1.Animations;
using GameTest1.Inputs;
using GameTest1.Interfaces;
using GameTest1.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using GameTest1.Engine;
using GameTest1.World;
using static GameTest1.Animation;

namespace GameTest1.Entities
{
    public class Character : Entity, IMobile, INeedsUpdate 
    {
        public bool EndGame { get; set; } 
        public bool Hit { get; set; }
        public int Score { get; set; }
        public Character(Spritesheet spritesheet, Rectangle window,Level curlevel,Vector2 startingtile, IInputReader reader,float scale, float maxSpeed) : base(spritesheet, window, curlevel,startingtile, scale, maxSpeed)
        {
            this.Acceleration = new Vector2(0.2f,6.3f);
            this.InputReader = reader;
            AddAnimation(AnimationType.Idle,new List<int>{0,1});
            AddAnimation(AnimationType.Run, new List<int> {2});
            AddAnimation(AnimationType.Jump, new List<int> { 3 });
            AddAnimation(AnimationType.Hit, new List<int> { 4 });
            AddAnimation(AnimationType.Death, new List<int> { 6 });
            this.curAnimation = animationList[AnimationType.Idle];
            this.Lives = 3;
            this.Alive = true;
        }

        public override void Update(GameTime gametime,Level curLevel,SpriteBatch sb)
        {
            //new Rectangle((int)Position.X, (int)Position.Y, (int)(this.curAnimation.CurrentFrame.HitBox.Width * Scale), (int)(this.curAnimation.CurrentFrame.HitBox.Height * Scale));

            //Update Location
            MovementManager.MoveCharacter(this,curLevel,sb);

            //Update Animation
            AnimationManager.setCurrentAnimationCharacter(this);
            this.curAnimation.Update(gametime);

            //Update LifeCycle
            if (Lives<=0)
            {
                this.Alive = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Color Drawcolor = Color.White;
            if (this.Hit)
            {
                Drawcolor = Color.Red;
            }

            if (FlipFlagX)
            {
                spriteBatch.Draw(_texture, CurPosition, curAnimation.CurrentFrame.SourceRectangle, Drawcolor, 0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0f);
                spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch),new Vector2(CollisionRectangle.X, CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);
            }
            else
            {
                spriteBatch.Draw(_texture, CurPosition, curAnimation.CurrentFrame.SourceRectangle, Drawcolor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch), new Vector2(CollisionRectangle.X, CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);
            }
        }
        public void TakeDamage(float amt)
        {
            Lives -= amt;
            if (Lives<=0)
            {
                Alive = false;
            }
            Game2.SoundLibrary[GameBase.SoundType.CharHit].Play();
        }
        public void Heal(float amt)
        {
            
            if (Lives < 3)
            {
                Game2.SoundLibrary[GameBase.SoundType.Heal].Play();
                Lives += amt;
            }
            else
            {
                Lives = 3;
            }
            
        }
    }
}
