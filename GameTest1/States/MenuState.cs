using GameTest1.Abstracts;
using GameTest1.Knop;
using GameTest1.States;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.States
{
    public class MenuState : Status
    {
        private List<Component> _spelComponenten;

        public Knoppen StartKnop { get; private set; }

        public Knoppen ExitKnop { get; private set; }
        public MenuState(GameBase spel, GraphicsDevice graphicsDevice, ContentManager content) : base(spel, graphicsDevice, content )
        {
            var buttonTexture = _content.Load<Texture2D>("Knop/Button");
            var buttonFont = _content.Load<SpriteFont>("Font/Font");

            StartKnop = new Knoppen(buttonTexture, buttonFont)
            {
                Positie = new Vector2(Level.screenWidth/2-buttonTexture.Width, Level.screenHeight / 2-buttonTexture.Height),
                
                Text = "New Game",
            };

            StartKnop.Klik += StartKnop_Klik;

            ExitKnop = new Knoppen(buttonTexture, buttonFont)
            {
                Positie = new Vector2(Level.screenWidth / 2 - buttonTexture.Width, Level.screenHeight / 2+100 - buttonTexture.Height),
                Text = "Exit",
            };

            ExitKnop.Klik += ExitKnop_Klik;

            _spelComponenten = new List<Component>()
            {
                StartKnop,
                ExitKnop,
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _spelComponenten)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
        private void StartKnop_Klik(object sender, EventArgs e)
        {

            _spel.CurrentState = _spel.GameState;
        }
        private void ExitKnop_Klik(object sender, EventArgs e)
        {
            _spel.Exit();
        }


        public override void Update(GameTime gameTime)
        {
            foreach (var component in _spelComponenten)
                component.Update(gameTime);
        }
    }
}
