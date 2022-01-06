using GameTest1.Abstracts;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.UI
{
    public class HealthBar : UIElement
    {
        public float Hearts { get; set; }
        public float Gap { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public Texture2D Full { get; set; }
        public Texture2D Half { get; set; }

        public HealthBar(Texture2D full, Texture2D half, Rectangle window, SpriteBatch sb,float hearts,float gap,float scale) : base(full, window, sb,scale)
        {
            this.Hearts = hearts;
            this.Gap = gap;
            this.Full = full;
            this.Half = half;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            float count = this.Hearts;

            for (int i = 0; i < Math.Round(Hearts); i++)
            {
                if (count == 0.5)
                {
                    spritebatch.Draw(Half, new Vector2(CurPosition.X + i * Width + i * Gap, CurPosition.Y), null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                }
                else
                {
                    count--;
                    spritebatch.Draw(Full, new Vector2(CurPosition.X + i * Width + i * Gap, CurPosition.Y), null, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                }
               
            }
        }

        public override void Update(GameTime gametime, Level curLevel, SpriteBatch sb)
        {
            Hearts = curLevel.Player.Lives;
        }
    }
}
