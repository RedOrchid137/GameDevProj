using GameTest1.Animations;
using GameTest1.Extensions;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static GameTest1.Animation;

namespace GameTest1.Entities
{
    public class Collectible : Entity
    {
        public enum CollectibleType {Raspberry, BlueBerry, StrawBerry}


        public CollectibleType Type { get; set; }
        public bool PickedUp { get; set; } = false;
        public Collectible(Spritesheet spritesheet, Rectangle window, Level curlevel, Vector2 startingtile,CollectibleType type, float scale = 1, float maxSpeed = 0) : base(spritesheet, window, curlevel, startingtile, scale, maxSpeed)
        {
            AddAnimation(AnimationType.Idle, new List<int> {0});
            curAnimation = animationList[AnimationType.Idle];
            this.Type = type;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, CurPosition, curAnimation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gametime, Level curLevel, SpriteBatch sb)
        {
            this.curAnimation.Update(gametime);
            this.Offsets = ExtensionMethods.CalcOffsets(this.curAnimation.CurrentFrame.SourceRectangle, this.curAnimation.CurrentFrame.HitBox);
            this.CollisionRectangle = new Rectangle((int)(this.CurPosition.X + this.Speed.X + this.Offsets.X / 2 * this.Scale), (int)(this.CurPosition.Y + this.Speed.Y + this.Offsets.Y * this.Scale), (int)(this.curAnimation.CurrentFrame.HitBox.Width * this.Scale), (int)(this.curAnimation.CurrentFrame.HitBox.Height * this.Scale));
        }
    }
}
