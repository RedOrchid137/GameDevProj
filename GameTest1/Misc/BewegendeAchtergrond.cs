using GameTest1.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using GameTest1.Sprites;


namespace GameTest1.Misc
{
    public class BewegendeAchtergrond : Component
    {
        private bool _constanteVersnelling;

        private float _laag;

        private float _scrollingSpeed;

        private List<Sprite> _sprites;

        private readonly Character _charachter;

        private float _versnelling;

        public float Laag
        {
            get { return _laag; }
            set
            {
                _laag = value;

                foreach (var sprite in _sprites)
                    sprite.Laag = _laag;
            }
        }

        public BewegendeAchtergrond(Texture2D texture, Character player, float scrollingSpeed, bool constantSpeed = false)
          : this(new List<Texture2D>() { texture, texture }, player, scrollingSpeed, constantSpeed)
        {

        }

        public BewegendeAchtergrond(List<Texture2D> textures, Character player, float scrollingSpeed, bool constantSpeed = false)
        {
            _charachter = player;

            _sprites = new List<Sprite>();

            for (int i = 0; i < textures.Count; i++)
            {
                var texture = textures[i];

                _sprites.Add(new Sprite(texture)
                {
                    Position = new Vector2(i * texture.Width - Math.Min(i, i + 1), World.screenHeight - texture.Height),
                });
            }

            _scrollingSpeed = scrollingSpeed;

            _constanteVersnelling = constantSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            VersnellingToepassen(gameTime);

            CheckPosition();
        }

        private void VersnellingToepassen(GameTime gameTime)
        {
            _versnelling = (float)(_scrollingSpeed * gameTime.ElapsedGameTime.TotalSeconds);

            if (!_constanteVersnelling || _charachter.Speed.X > 0)
                _versnelling *= _charachter.Speed.X;

            foreach (var sprite in _sprites)
            {
                sprite.Position.X -= _versnelling;
            }
        }

        private void CheckPosition()
        {
            for (int i = 0; i < _sprites.Count; i++)
            {
                var sprite = _sprites[i];

                if (sprite.Rectangle.Right <= 0)
                {
                    var index = i - 1;

                    if (index < 0)
                        index = _sprites.Count - 1;

                    sprite.Position.X = _sprites[index].Rectangle.Right - (_versnelling * 2);
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var sprite in _sprites)
                sprite.Draw(gameTime, spriteBatch);
        }


    }
}
