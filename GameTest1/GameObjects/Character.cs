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

namespace GameTest1
{
    public class Character : GameObject, IMobile, INeedsUpdate 
    {

        public bool JumpFlag { get; set; }
        public bool IsSleeping { get; set; }
        public Character(Spritesheet spritesheet, SpriteBatch spritebatch, Rectangle window, IInputReader reader,float scale, float maxSpeed) : base(spritesheet, spritebatch, window, scale, maxSpeed)
        {
            this.Acceleration = new Vector2(0.2f,5f);
            this.InputReader = reader;
            AddAnimation(AnimationType.Idle,new List<int>{0,1});
            AddAnimation(AnimationType.Run, new List<int> {2});
            AddAnimation(AnimationType.Jump, new List<int> { 3 });
            AddAnimation(AnimationType.Sleep, new List<int> { 5 });
            AddAnimation(AnimationType.Crouch, new List<int> { 6});
            this.curAnimation = animationList[AnimationType.Idle];
        }

        public override void Update(GameTime gametime)
        {

            //new Rectangle((int)Position.X, (int)Position.Y, (int)(this.curAnimation.CurrentFrame.HitBox.Width * Scale), (int)(this.curAnimation.CurrentFrame.HitBox.Height * Scale));
            
            //Update Location
            MovementManager.MoveCharacter(this);
            
            //Update Animation
            AnimationManager.setCurrentAnimation(this);
            this.curAnimation.Update(gametime);

            //Check+deal w/ collisions
            Offsets = ExtensionMethods.CalcOffsets(curAnimation.CurrentFrame.SourceRectangle, curAnimation.CurrentFrame.HitBox);
            CollisionRectangle = new Rectangle((int)(CurPosition.X + Speed.X + Offsets.X / 2 * Scale), (int)(CurPosition.Y + Speed.Y + Offsets.Y * Scale), (int)(curAnimation.CurrentFrame.HitBox.Width * Scale), (int)(curAnimation.CurrentFrame.HitBox.Height * Scale));
            this.CollisionCheck(Game1.ObjManager);
            CollisionManager.HandleCollisionsCharacter(this);
        }

        public override void Draw()
        {
            _spriteBatch.Begin();
            if (FlipFlagX)
            {
                _spriteBatch.Draw(_texture, CurPosition, curAnimation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0f);
                _spriteBatch.Draw(ExtensionMethods.BlankTexture(_spriteBatch),new Vector2(CollisionRectangle.X,CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);
            }
            //new Vector2(CollisionRectangle.Width, 0)
            else
            {
                _spriteBatch.Draw(_texture, CurPosition, curAnimation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                _spriteBatch.Draw(ExtensionMethods.BlankTexture(_spriteBatch), new Vector2(CollisionRectangle.X, CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);
            }
            _spriteBatch.End();
        }
    }
}
