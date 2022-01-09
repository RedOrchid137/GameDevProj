using GameTest1.Abstracts;
using GameTest1.Animations;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static GameTest1.Animation;

namespace GameTest1.Enemies
{
    public class SkeletonEnemy : Enemy
    {
        public SkeletonEnemy(Spritesheet spritesheet, Rectangle window, Level curlevel, Vector2 startingtile, Vector2 path, float scale = 1, float maxSpeed = 5) : base(spritesheet, window, curlevel, startingtile, path, scale, maxSpeed)
        {
            this.Acceleration = new Vector2(0.2f, 5);
            AddAnimation(AnimationType.Idle, new List<int> { 3 });
            AddAnimation(AnimationType.Run, new List<int> { 2 });
            AddAnimation(AnimationType.Attack, new List<int> { 0 });
            AddAnimation(AnimationType.Death, new List<int> { 1 });
            animationList[AnimationType.Death].Fps = 8;
            this.curAnimation = animationList[AnimationType.Idle];
            this.Direction = true;
        }
        public override void Update(GameTime gametime, Level curLevel, SpriteBatch sb)
        {
            base.Update(gametime, curLevel, sb);
            this.AttackRange = CollisionRectangle.Width * 4;
        }
    }
}
