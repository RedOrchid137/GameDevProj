using GameTest1.Abstracts;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GameTest1.UI
{
    public class Score : Text
    { 

        public Score(Rectangle window,SpriteBatch sb,SpriteFont font,Color color) : base(window, sb,font,color)
        {
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
            //Rectangle = new Rectangle((int)CurPosition.X, (int)CurPosition.Y,Rectangle.Width,Rectangle.Height);
            //if (!string.IsNullOrEmpty(TextContent))
            //{
            //    var x = (Rectangle.X + (Rectangle.Width / 2)) - (Font.MeasureString(TextContent).X / 2);
            //    var y = (Rectangle.Y + (Rectangle.Height / 2)) - (Font.MeasureString(TextContent).Y / 2);

            //    spritebatch.DrawString(Font, TextContent, new Vector2(x, y), Color);
            //}
        }

        public override void Update(GameTime gametime, Level curLevel, SpriteBatch sb)
        {
            base.Update(gametime, curLevel, sb);
            TextContent = "Collected:  "+curLevel.Player.Score + "/" + curLevel.RequiredScore;
        }
    }
}
