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

        public SpriteFont Tutorial { get; private set; }


        private string tutorialText = "Collect all raspberries and find the exit door to progress.\nJump on top of enemies to kill them.\nKeep an eye on your health, better not jump into the lava.\nHeal using the lightblue health pools.\nGet extra distance on a jump using the floating blueberries.";
        public MenuState(GameBase spel, GraphicsDevice graphicsDevice, ContentManager content) : base(spel, graphicsDevice, content )
        {

            Tutorial = spel.Content.Load<SpriteFont>("Font/TutFont");
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
            spriteBatch.DrawString(Tutorial,tutorialText, new Vector2(Level.screenWidth/2-350, Level.screenHeight - 200), Color.White);
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
