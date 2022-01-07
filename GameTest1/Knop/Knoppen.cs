using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using static GameTest1.GameBase;

namespace GameTest1.Knop
{
    public class Knoppen : Component
    {
        private MouseState _huidigeMuis;

        private SpriteFont _font;

        private bool _isHovering;

        private MouseState _vorigeMuis;

        private Texture2D _texture;

        public event EventHandler Klik;
        public Vector2 Positie { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Positie.X, (int)Positie.Y, _texture.Width, _texture.Height);
            }
        }
        public bool Clicked { get; private set; }

        public Color PenKleur { get; set; }

        public string Text { get; set; }

        public Knoppen(Texture2D texture, SpriteFont font)
        {
            _texture = texture;

            _font = font;

            PenKleur = Color.Black;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var kleur = Color.White;

            if (_isHovering)
                kleur = Color.Gray;

            spriteBatch.Draw(_texture, Rectangle, kleur);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenKleur);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _vorigeMuis = _huidigeMuis;
            _huidigeMuis = Mouse.GetState();

            var mouseRectangle = new Rectangle(_huidigeMuis.X, _huidigeMuis.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_huidigeMuis.LeftButton == ButtonState.Released && _vorigeMuis.LeftButton == ButtonState.Pressed)
                {
                    Klik?.Invoke(this, new EventArgs());
                    GameBase.SoundLibrary[SoundType.Click].Play();
                }
            }
        }
    }
}
