using GameTest1.Animations;
using GameTest1.Engine;
using GameTest1.Inputs;
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

        public void UpdateAll(GameTime time)
        {
            foreach (var item in _objectList)
            {
                item.Update(time);
            }
        }
        public void DrawAll()
        {
            foreach (var item in _objectList)
            {
                item.Draw();
            }
        }
        public void CollisionCheck()
        {
            foreach (var item in _objectList)
            {
                for (int i = 0; i < _objectList.Count; i++)
                {
                    if (_objectList[i] != item)
                    {
                        if(CollisionManager.CheckCollision(item.CollisionRectangle, _objectList[i].CollisionRectangle))
                        {
                            item.IsColliding = true;
                            _objectList[i].IsColliding = true;
                        }
                        else
                        {
                            item.IsColliding = false;
                            _objectList[i].IsColliding = false;
                        }
                    }
                }
            }
        }
    }
}
