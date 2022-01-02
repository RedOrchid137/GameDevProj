using GameTest1.Knop;
using GameTest1.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.State
{
    public class MenuState : Status
    {
        private List<Component> _spelComponenten;
        public MenuState(GameBase spel, GraphicsDevice graphicsDevice, ContentManager content) : base(spel, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("Knop/Button");
            var buttonFont = _content.Load<SpriteFont>("Font/Font");

            var startKnop = new Knoppen(buttonTexture, buttonFont)
            {
                Positie = new Vector2(400, 200),
                Text = "New Game",
            };

            startKnop.Klik += StartKnop_Klik;

            var exitKnop = new Knoppen(buttonTexture, buttonFont)
            {
                Positie = new Vector2(400, 300),
                Text = "Exit",
            };

            exitKnop.Klik += ExitKnop_Klik;

            _spelComponenten = new List<Component>()
            {
                startKnop,
                exitKnop,
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

            _spel.WisselStatus(new GameState(_spel, _graphicsDevice, _content));
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
