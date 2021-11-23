using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Rectangle = System.Drawing.Rectangle;

namespace GameTest1.GameObjects
{
    class Blok
    {
        public Texture2D _texture { get; set; }
        public Vector2 Positie { get; set; }
        public Rectangle CollisionRectangle { get; set; }

        public Blok(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            Positie = position;
            CollisionRectangle = new Rectangle((int)Positie.X, (int)Positie.Y, 128, 64);

        }
        public void Draw(SpriteBatch spriteBach)
        {
            spriteBach.Draw(_texture, Positie, Color.White);
        }

        public void Update()
        {

        }
    }
}
