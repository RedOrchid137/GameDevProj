using GameTest1.Abstracts;
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

namespace GameTest1.Entities
{
    public class ObjectManager
    {



        private List<Entity> _objectList;
        public List<Entity> ObjectList { get { return _objectList; } set { _objectList = value; } }

        public ObjectManager()
        {
            _objectList = new List<Entity>();
        }

        public void UpdateAll(GameTime time,Level curLevel,SpriteBatch sb)
        {
            foreach (var item in _objectList)
            {
                item.Update(time,curLevel,sb);
            }
            if (CollisionManager.ActionList!= null)
            {
                foreach (var action in CollisionManager.ActionList)
                {
                    if (action != null)
                    {
                        action.Update(time);
                    }
                }
            }
            RemovePickedUpItems();
            RemoveDeadEnemies();
        }
        public void DrawAll(SpriteBatch spriteBatch)
        {
            foreach (var item in _objectList)
            {
                item.Draw(spriteBatch);
            }
        }

        public void RemovePickedUpItems()
        {
            _objectList.RemoveAll(o => o.GetType() == typeof(Collectible) && (o as Collectible).PickedUp);     
        }
        public void RemoveDeadEnemies()
        {
            _objectList.RemoveAll(o => o.GetType().BaseType == typeof(Enemy) && !(o as Enemy).Alive);
        }
    }
}
