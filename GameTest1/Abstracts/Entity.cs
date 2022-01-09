using GameTest1.Animations;
using GameTest1.Engine;
using GameTest1.Extensions;
using GameTest1.Entities;
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
using static GameTest1.Animation;
using GameTest1.Abstracts;
using System.Linq;

namespace GameTest1
{
    public abstract class Entity:INeedsUpdate, IMobile
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
        public bool Interacting { get; set; }

        //Location params
        private Vector2 startingtile;
        public Vector2 StartingTile
        {
            get { return startingtile; }
            set
            {
                startingtile = new Vector2(value.X * CurLevel.TileWidth, value.Y * CurLevel.TileWidth);
                this.CurPosition = startingtile;
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
        public Rectangle IntersectSurface { get; set; }

        //Animation+Drawing params
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

        private float _lives;
        public float Lives { get {return _lives; } set { if (value <= 3) _lives = value;}}
        
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
            anime.Owner = this;
            anime.Type = type;
            anime.GetFramesFromTextureProperties(spritesheet, rowsNeeded);
            anime.CurrentFrame = anime.Frames[0];
            animationList[type] = anime;
        }
        #region Collisions
        public void CollisionCheckFull(ObjectManager man)
        {
            CollisionManager.CollisionCheckFull(man, this);
        }
        public void CollisionCheckTile(Rectangle CollisionRectangle, SpriteBatch sb,int id)
        {
            CollisionManager.CollisionCheckTile(CollisionRectangle,sb ,id,this);
        }
        #endregion
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
            this.Ground = CurPosition.Y;
            this.CurLevel = curlevel;
            this.Alive = true;
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