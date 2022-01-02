using GameTest1.Animations;
using GameTest1.Engine;
using GameTest1.Inputs;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GameTest1.GameObjects
{
    public class ObjectManager
    {



        private List<GameObject> _objectList;
        public List<GameObject> ObjectList { get { return _objectList; } set { _objectList = value; } }

        public ObjectManager()
        {
            _objectList = new List<GameObject>();
        }

        public void UpdateAll(GameTime time,Level curLevel,SpriteBatch sb)
        {
            foreach (var item in _objectList)
            {
                item.Update(time,curLevel,sb);
            }
        }
        public void DrawAll(SpriteBatch spriteBatch)
        {
            foreach (var item in _objectList)
            {
                item.Draw(spriteBatch);
            }
        }
    }
}
