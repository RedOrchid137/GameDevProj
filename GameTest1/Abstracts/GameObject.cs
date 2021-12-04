using GameTest1.Animations;
using GameTest1.Engine;
using GameTest1.Extensions;
using GameTest1.GameObjects;
using GameTest1.Inputs;
using GameTest1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameTest1
{
    public abstract class GameObject:INeedsUpdate
    {
        //protected vars
        protected SpriteBatch _spriteBatch;
        protected Texture2D _texture;
        protected Rectangle _window;
        private float _maxSpeed;
        
        
        //Boolean Flags
        public bool FlipFlagX { get; set; } = false;
        public bool FlipFlagY { get; set; } = false;
        public bool IsColliding { get; set; } = false;


        //Location params
        public float MaxSpeed { get { return _maxSpeed; } set { _maxSpeed = value; } }
        public Vector2 PrevPosition { get; set; }
        public Vector2 CurPosition { get; set; }
        public Vector2 Speed { get; set; }
        public Vector2 Acceleration { get; set; }

        //Collision

        public Rectangle CollisionRectangle { get; set; }
        public Dictionary<GameObject,CollisionType> CollisionList;


        //Animation+Drawing params
        public enum AnimationType { Run, Jump, Idle, Crouch, Attack, Block, Sleep }
        internal Dictionary<AnimationType, Animation> animationList;
        internal Spritesheet spritesheet { get; set; }
        internal Animation curAnimation;
        public SpriteBatch SpriteBatch { get { return _spriteBatch; } set { _spriteBatch = value; } }
        public Texture2D Texture { get { return _texture; } set { _texture = value; } }
        public Rectangle Window { get { return _window; } set { _window = value; } }
        public IInputReader InputReader { get; set; }
        public float Scale { get; set; }
        public enum CollisionType { Top,Bottom,Right,Left}
        public Vector2 Offsets { get; set; }

        protected Vector2 Limit(Vector2 v, float max)
        {
            if (v.Length() > max)
            {
                var ratio = max / v.Length();
                v.X *= ratio;
                v.Y *= ratio;
            }
            return v;
        }

        public void AddAnimation(AnimationType type, List<int> rowsNeeded)
        {
            Animation anime = new Animation();
            anime.Type = type;
            anime.GetFramesFromTextureProperties(spritesheet, rowsNeeded);
            anime.CurrentFrame = anime.Frames[0];
            animationList[type] = anime;
        }
        public void CollisionCheck(ObjectManager man)
        {
            foreach (var item in man.ObjectList)
            {
                for (int i = 0; i < man.ObjectList.Count; i++)
                {
                    if (item != this)
                    {
                        if (CollisionManager.CheckCollision(this.CollisionRectangle, item.CollisionRectangle))
                        {
                            CollisionType test = new CollisionType();
                            if (this.CollisionRectangle.Bottom>item.CollisionRectangle.Top && this.CollisionRectangle.Top < item.CollisionRectangle.Top)
                            {
                                test = CollisionType.Top;
                            }
                            else if (this.CollisionRectangle.Top < item.CollisionRectangle.Bottom && this.CollisionRectangle.Bottom > item.CollisionRectangle.Bottom)
                            {
                                test = CollisionType.Bottom;
                            }
                            else if (this.CollisionRectangle.Right > item.CollisionRectangle.Left&& this.CollisionRectangle.Right<item.CollisionRectangle.Right)
                            {
                                test = CollisionType.Left;
                            }
                            else if (this.CollisionRectangle.Left < item.CollisionRectangle.Right && this.CollisionRectangle.Left > item.CollisionRectangle.Left)
                            {
                                test = CollisionType.Right;
                            }

                            this.CollisionList[item] = test;
                            this.IsColliding = true;
                            item.CollisionList[this] = test.GetOpposite();
                        }
                        else
                        {
                            item.IsColliding = false;
                            this.CollisionList.Remove(item);
                            this.IsColliding = false;
                            item.CollisionList.Remove(this);
                        }
                    }
                }
            }
        }

        public abstract void Update(GameTime gametime);
        public abstract void Draw();

        public GameObject(Spritesheet spritesheet,SpriteBatch spritebatch,Rectangle window, float scale=1,float maxSpeed=0)
        {
            this.Scale = scale;
            animationList = new Dictionary<AnimationType, Animation>();
            CollisionList = new Dictionary<GameObject, CollisionType>();
            this.spritesheet = spritesheet;
            _maxSpeed = maxSpeed;
            _window = window;
            _texture = this.spritesheet.Texture;
            _spriteBatch = spritebatch;
        }
    }
}