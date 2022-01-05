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

namespace GameTest1
{
    public class Character : Entity, IMobile, INeedsUpdate 
    {
        public bool IsSleeping { get; set; }
        public Character(Spritesheet spritesheet, Rectangle window,Level curlevel,Vector2 startingtile, IInputReader reader,float scale, float maxSpeed) : base(spritesheet, window, curlevel,startingtile, scale, maxSpeed)
        {
            this.Acceleration = new Vector2(0.2f,6.5f);
            this.InputReader = reader;
            AddAnimation(AnimationType.Idle,new List<int>{0,1});
            AddAnimation(AnimationType.Run, new List<int> {2});
            AddAnimation(AnimationType.Jump, new List<int> { 3 });
            AddAnimation(AnimationType.Sleep, new List<int> { 5 });
            AddAnimation(AnimationType.Crouch, new List<int> { 6});
            this.curAnimation = animationList[AnimationType.Idle];
        }

        public override void Update(GameTime gametime,Level curLevel,SpriteBatch sb)
        {
            //new Rectangle((int)Position.X, (int)Position.Y, (int)(this.curAnimation.CurrentFrame.HitBox.Width * Scale), (int)(this.curAnimation.CurrentFrame.HitBox.Height * Scale));

            //Update Location
            MovementManager.MoveCharacter(this,curLevel,sb);

            //Update Animation
            AnimationManager.setCurrentAnimationCharacter(this);
            this.curAnimation.Update(gametime);

            //Check + deal w / collisions
            //Offsets = ExtensionMethods.CalcOffsets(curAnimation.CurrentFrame.SourceRectangle, curAnimation.CurrentFrame.HitBox);
            //CollisionRectangle = new Rectangle((int)(CurPosition.X + Speed.X + Offsets.X / 2 * Scale), (int)(CurPosition.Y + Speed.Y + Offsets.Y * Scale), (int)(curAnimation.CurrentFrame.HitBox.Width * Scale), (int)(curAnimation.CurrentFrame.HitBox.Height * Scale));
            //curLevel.CheckCollision(this, sb);
            //this.CollisionCheckFull(Game2.ObjManager);
            //this.checkFloor();
            //CollisionManager.HandleCollisionsCharacter(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (FlipFlagX)
            {
                spriteBatch.Draw(_texture, CurPosition, curAnimation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0f);
                //spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch),new Vector2(CollisionRectangle.X, CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);
            }
            //new Vector2(CollisionRectangle.Width, 0)
            else
            {
                spriteBatch.Draw(_texture, CurPosition, curAnimation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                //spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch), new Vector2(CollisionRectangle.X, CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);
            }
        }
    }
}
