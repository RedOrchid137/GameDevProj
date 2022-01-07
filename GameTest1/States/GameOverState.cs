using GameTest1.Abstracts;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.States
{
    public class GameOverState : Status
    {
        

        private Texture2D background;

        public Texture2D Background
        {
            get { return background; }
            set { background = value; }
        }
        private float scale;

        public float Scale
        {
            get { return scale; }
        }


        Text CenterText { get; set; }

        public GameOverState(GameBase spel, GraphicsDevice graphicsDevice, ContentManager content,Texture2D background) : base(spel, graphicsDevice, content)
        {
            Background = background;
            scale = Level.screenWidth / Background.Width;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(Background, Vector2.Zero, null, Color.White,0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
            spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            return;
        }
    }
}
