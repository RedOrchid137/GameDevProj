using GameTest1.Animations;
using GameTest1.World;
using GameTest1.Entities;
using GameTest1.Inputs;
using GameTest1.Misc;
using GameTest1.Perspective;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using GameTest1.Extensions;
using GameTest1.States;
using GameTest1.UI;
using GameTest1.Abstracts;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace GameTest1
{
    public abstract class GameBase : Game
    {
        public enum SoundType { CharHit, EnemyHit, Music, Jump, Collect, LevelComplete,Click,Lose,Victory,Heal,Shot }

        protected static GraphicsDeviceManager _graphics;
        internal static GraphicsDeviceManager Graphics { get { return _graphics; }}
        internal static Rectangle WindowRectangle { get; set; }
        private static Level curLevel;
        public static Dictionary<SoundType,SoundEffect>SoundLibrary = new Dictionary<SoundType, SoundEffect>();
        public Song BGMusic { get; set; }

        public static Level CurLevel
        {
            get { return curLevel; }
            set { curLevel = value; curLevel.Player = CurPlayer; }
        }

        internal static Character CurPlayer { get; set; }
        internal static List<Level> lvlList { get; set; }
        internal static int lvlAmount{ get; set; }
        internal static int lvlCount { get; set; }
        internal SpriteBatch _spriteBatch { get; set; }
        internal Character player;


        //https://stackoverflow.com/questions/16556071/xna-how-to-exit-game-from-class-other-than-main/29089475
        public static Game self;


        //Rendering

        internal Camera _camera;
        protected TiledMapRenderer _tiledMapRenderer;
        protected UIOverlay UI;


        public Status CurrentState { get; set; }
        public Status MenuState { get; set; }
        public Status GameState { get; set; }
        public Status GameOverState { get; set; }

        public static bool Victory { get; set; } = false;
        public bool StaticScreen { get; set; } = false;

        public GameBase()
        {
            self = this;
            UI = new UIOverlay();
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            lvlList = new List<Level>();
            _camera = new Camera();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, CurLevel.Map);
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            UI.Update(gameTime, CurLevel, _spriteBatch);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, CurLevel.Map);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (CurrentState.GetType() == typeof(MenuState))
            {
                CurrentState.Draw(gameTime, _spriteBatch);
            }
            else if (CurrentState.GetType() == typeof(GameState))
            {

                //Background Tekenen

                _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null,Matrix.CreateScale(CurLevel.BackgroundScale));
                //_spriteBatch.Draw(CurLevel.Background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, CurLevel.BackgroundScale, SpriteEffects.None, 0f);
                _spriteBatch.Draw(CurLevel.Background,new Rectangle(0,0,CurLevel.Background.Width,CurLevel.Background.Height),Color.White);
                _spriteBatch.End();


                //Map + Sprites Tekenen
                _spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: _camera.Transform);
                
                foreach (var item in CurLevel.Map.Layers)
                {
                    _tiledMapRenderer.Draw(item, _camera.Transform);
                }
                
                
                //foreach (var item in tilelist)
                //{
                //    _spriteBatch.Draw(ExtensionMethods.BlankTexture(_spriteBatch), new Vector2(item.X, item.Y), item, Color.Red * 0.5f);
                //}
                CurLevel.oMan.DrawAll(_spriteBatch);
                
                _spriteBatch.End();


                //UI Tekenen
                _spriteBatch.Begin();
                UI.Draw(_spriteBatch);
                _spriteBatch.End();
            }
            else if (CurrentState.GetType() == typeof(GameOverState))
            {
                GameOverState.Draw(gameTime,_spriteBatch);
            }

            base.Draw(gameTime);
        }
        protected abstract void LoadSounds();
        protected abstract void LoadUI();
        protected abstract void LoadLevels();
        protected void InitObjects()
        {
            _graphics.PreferredBackBufferWidth = (int)(GraphicsDevice.DisplayMode.Width/1.3);
            _graphics.PreferredBackBufferHeight = (int)(GraphicsDevice.DisplayMode.Height / 1.3);
            _graphics.ApplyChanges();
            WindowRectangle = new Rectangle(0, 0, Game2.Graphics.PreferredBackBufferWidth, Game2.Graphics.PreferredBackBufferHeight);
            Level.screenWidth = _graphics.PreferredBackBufferWidth;
            Level.screenHeight = _graphics.PreferredBackBufferHeight;
        }
    }
    }
