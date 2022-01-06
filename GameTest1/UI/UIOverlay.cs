using GameTest1.Abstracts;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.UI
{
    public class UIOverlay:INeedsUpdate
    {
        private List<UIElement> _elements;
        public void Draw(SpriteBatch spritebatch)
        {
            DrawElements(spritebatch);
        }

        public UIOverlay()
        {
            this._elements = new List<UIElement>();
        }


        public void Update(GameTime gametime, Level curLevel, SpriteBatch sb)
        {
            foreach (var element in _elements)
            {
                element.Update(gametime, curLevel, sb);
            }
        }

        public void DrawElements(SpriteBatch spritebatch)
        {
            foreach (var element in _elements)
            {
                element.Draw(spritebatch);
            }
        }

        public void AddElement(Vector2 position, UIElement element)
        {
            element.CurPosition = position;
            _elements.Add(element);
        }
    }
}
