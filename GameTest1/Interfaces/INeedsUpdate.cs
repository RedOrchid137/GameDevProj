using Microsoft.Xna.Framework;

namespace GameTest1
{
    public interface INeedsUpdate
    {
        public void Update(GameTime gametime);
        public void Draw();
    }
}