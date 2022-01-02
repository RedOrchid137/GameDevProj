using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameTest1
{
    public interface INeedsUpdate
    {
        public void Update(GameTime gametime,Level curLevel, SpriteBatch sb);
        public void Draw(SpriteBatch spritebatch);
    }
}