﻿using GameTest1.Animations;
using GameTest1.Inputs;
using GameTest1.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameTest1
{
    public abstract class GameObject
    {
        public enum AnimationType { Run, Jump, Idle, Crouch, Attack, Block, Sleep }
        internal Dictionary<AnimationType, Animation> animationList;

        protected SpriteBatch _spriteBatch;
        protected Texture2D _texture;
        protected Rectangle _window;
        protected float _resize;
        private float _maxSpeed;

        public float Scale { get; set; }
        public bool FlipFlagX { get; set; } = false;
        public bool FlipFlagY { get; set; } = false;
        public float MaxSpeed { get { return _maxSpeed; } set { _maxSpeed = value; } }
        public SpriteBatch SpriteBatch { get { return _spriteBatch; } set { _spriteBatch = value; } }
        public Texture2D Texture { get { return _texture; } set { _texture = value; } }
        public Rectangle Window { get { return _window; } set { _window = value; } }
        public float Resize { get { return _resize; } set { _resize = value; } }
        public IInputReader InputReader { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Speed { get; set; }
        public Vector2 Acceleration { get; set; }

        internal Spritesheet spritesheet { get; set; }
        internal bool isMoving;
        internal Animation curAnimation;

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
            anime.GetFramesFromTextureProperties(spritesheet.Width, spritesheet.Height, spritesheet.Max, spritesheet.RowCounts.Count, spritesheet.RowCounts, rowsNeeded);
            anime.CurrentFrame = anime.Frames[0];
            animationList[type] = anime;
        }

        public GameObject(Spritesheet spritesheet,SpriteBatch spritebatch,Rectangle window, float maxSpeed,float scale,float resize = 1)
        {
            this.Scale = scale;
            animationList = new Dictionary<AnimationType, Animation>();
            this.spritesheet = spritesheet;
            _maxSpeed = maxSpeed;
            _resize = resize;
            _window = window;
            _texture = this.spritesheet.Texture;
            _spriteBatch = spritebatch;
            AnimationManager.setTimer();
        }
    }
}