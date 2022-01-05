using GameTest1.Animations;
using GameTest1.Engine;
using GameTest1.Extensions;
using GameTest1.GameObjects;
using GameTest1.Inputs;
using GameTest1.Interfaces;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace GameTest1
{
    public abstract class Entity:INeedsUpdate
    {
        //protected vars
        protected SpriteBatch _spriteBatch;
        protected Texture2D _texture;
        protected Rectangle _window;
        private float _maxSpeed;


        //Boolean Flags
        public bool FlipFlagX { get; set; } = false;
        public bool FlipFlagY { get; set; } = false;
        public bool IsFalling { get; set; } = false;


        //Location params
        private Vector2 startingtile;
        public Vector2 StartingTile
        {
            get { return startingtile; }
            set
            {
                startingtile = new Vector2(value.X * CurLevel.TileWidth, value.Y * CurLevel.TileWidth);
            }
        }
        public float MaxSpeed { get { return _maxSpeed; } set { _maxSpeed = value; } }
        public Vector2 PrevPosition { get; set; }
        public Vector2 CurPosition { get; set; }
        public Vector2 Speed { get; set; }
        public Vector2 Acceleration { get; set; }
        public TiledMapTile CurrentTile { get; set; }
        public Level CurLevel { get; set; }
        public bool hKeyPressed { get; set; }

        //Collision

        public Rectangle CollisionRectangle { get; set; }
        public Dictionary<Entity,CollisionType> CollisionList;
        public List<Rectangle> TileRectList { get; set; } = new List<Rectangle>();
        public float Ground { get; set; }
        public bool onGround { get; set; }

        //Animation+Drawing params
        public enum AnimationType { Run, Jump, Idle, Crouch, Attack, Block, Sleep,Death,Damage }
        internal Dictionary<AnimationType, Animation> animationList;
        internal Spritesheet spritesheet { get; set; }
        internal Animation curAnimation;
        public Texture2D Texture { get { return _texture; } set { _texture = value; } }
        public Rectangle Window { get { return _window; } set { _window = value; } }
        public IInputReader InputReader { get; set; }
        public float Scale { get; set; }
        public enum CollisionType { Top,Bottom,Right,Left}
        public Vector2 Offsets { get; set; }
        public float Layer { get; set; }

        //LifeCycle Vars

        public bool Alive { get; set; }
        public int Lives { get; set; }

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
        public void CollisionCheckFull(ObjectManager man)
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
                            if (this.CollisionRectangle.Bottom>item.CollisionRectangle.Top && this.CollisionRectangle.Center.Y < item.CollisionRectangle.Top)
                            {
                                test = CollisionType.Top;
                            }
                            else if (this.CollisionRectangle.Top < item.CollisionRectangle.Bottom && this.CollisionRectangle.Center.Y > item.CollisionRectangle.Bottom )
                            {
                                test = CollisionType.Bottom;
                            }
                            else if (this.CollisionRectangle.Right > item.CollisionRectangle.Left&& this.CollisionRectangle.Center.X < item.CollisionRectangle.Right )
                            {
                                test = CollisionType.Left;
                            }
                            else if (this.CollisionRectangle.Left < item.CollisionRectangle.Right && this.CollisionRectangle.Center.X > item.CollisionRectangle.Left )
                            {
                                test = CollisionType.Right;
                            }

                            this.CollisionList[item] = test;
                            item.CollisionList[this] = test.GetOpposite();
                        }
                        else
                        {
                            this.CollisionList.Remove(item);
                        }
                    }
                }
            }
        }
        public void CollisionCheckTile(Rectangle CollisionRectangle, SpriteBatch sb,int id)
        {
            if (this.TileRectList.Contains(CollisionRectangle))
            {
                return;
            }
            var item = new Tile(ExtensionMethods.BlankTexture(sb), CollisionRectangle, 1, id);
            item.CollisionRectangle = CollisionRectangle;

            if (CollisionManager.CheckCollision(this.CollisionRectangle, CollisionRectangle))
            {
                Rectangle intersectSurface = Rectangle.Intersect(this.CollisionRectangle, item.CollisionRectangle);
                CollisionType test = new CollisionType();
                if (this.CollisionRectangle.Bottom > item.CollisionRectangle.Top && this.CollisionRectangle.Top < item.CollisionRectangle.Top && this.Speed.Y >= 0 && intersectSurface.Width > intersectSurface.Height)
                {
                    test = CollisionType.Top;
                }
                else if (this.CollisionRectangle.Top < item.CollisionRectangle.Bottom && this.CollisionRectangle.Bottom > item.CollisionRectangle.Bottom && this.Speed.Y<0 && intersectSurface.Width > intersectSurface.Height)
                {
                    test = CollisionType.Bottom;
                }
                else if (this.CollisionRectangle.Right > item.CollisionRectangle.Left && this.CollisionRectangle.Right < item.CollisionRectangle.Right && this.Speed.X>0 && intersectSurface.Width < intersectSurface.Height)
                {
                    test = CollisionType.Left;
                }
                else if (this.CollisionRectangle.Left < item.CollisionRectangle.Right && this.CollisionRectangle.Left > item.CollisionRectangle.Left && this.Speed.X < 0 && intersectSurface.Width < intersectSurface.Height)
                { 
                    test = CollisionType.Right;
                }
                item.IntersectSurface = intersectSurface;
                this.CollisionList[item] = test;
                if (!this.TileRectList.Contains(CollisionRectangle))
                {
                    this.TileRectList.Add(CollisionRectangle);
                }
            }
        }
        public abstract void Update(GameTime gametime,Level curLevel, SpriteBatch sb);
        public abstract void Draw(SpriteBatch spriteBatch);

        public Entity(Spritesheet spritesheet,Rectangle window,Level curlevel,Vector2 startingtile, float scale=1,float maxSpeed=0)
        {
            this.Scale = scale;
            CurLevel = curlevel;
            animationList = new Dictionary<AnimationType, Animation>();
            CollisionList = new Dictionary<Entity, CollisionType>();
            this.spritesheet = spritesheet;
            _maxSpeed = maxSpeed;
            _window = window;
            _texture = this.spritesheet.Texture;
            this.StartingTile = startingtile;
            this.CurPosition = StartingTile;
            this.Ground = CurPosition.Y;
            this.CurLevel = curlevel;
        }
        public Entity(Texture2D texture, Rectangle window, float scale)
        {
            CollisionList = new Dictionary<Entity, CollisionType>();
            this.Scale = scale;
            this.Texture = texture;
            this.Window = window;
        }
    }
}