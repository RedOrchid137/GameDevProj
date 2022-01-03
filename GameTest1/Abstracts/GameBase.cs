using GameTest1.Animations;
using GameTest1.World;
using GameTest1.GameObjects;
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
using GameTest1.State;
using GameTest1.States;

namespace GameTest1
{
    public abstract class GameBase : Game
    {
        //testing vars
        Stopwatch _watch;
        protected static GraphicsDeviceManager _graphics;
        public static GraphicsDeviceManager Graphics { get { return _graphics; }}

        protected static ObjectManager oMan { get; set; }
        public static ObjectManager ObjManager { get { return oMan; } set { oMan = value; } }
        

        public static List<Rectangle> tilelist { get; set; }
        public static Rectangle WindowRectangle { get; set; }


        protected SpriteBatch _spriteBatch { get; set; }
        protected Character testchar;

        SpriteFont FPSFont;
        float curFPS;
        //Levels
        public Level Level1 { get; set; }
        public Level Level2 { get; set; }


        //Rendering

        protected Camera _camera;
        protected TiledMap _tiledMap;
        protected TiledMapRenderer _tiledMapRenderer;


        protected Status _huidigeStatus;
        protected Status _volgendeStatus;

        public GameBase()
        {
            _watch = new Stopwatch();
            oMan = new ObjectManager();
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        internal void WisselStatus(GameState gameState)
        {
            _volgendeStatus = gameState;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            InitObjects();
        }

        protected override void Update(GameTime gameTime)
        {
            _watch.Start();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            if (_huidigeStatus.GetType() == typeof(MenuState))
            {
                _huidigeStatus.Draw(gameTime, _spriteBatch);
            }
            else if (_huidigeStatus.GetType() == typeof(GameState))
            {
                _spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: _camera.Transform);
                foreach (var item in _tiledMap.Layers)
                {
                    _tiledMapRenderer.Draw(item, _camera.Transform);
                }
                //foreach (var item in tilelist)
                //{
                //    _spriteBatch.Draw(ExtensionMethods.BlankTexture(_spriteBatch), new Vector2(item.X, item.Y), item, Color.Red * 0.5f);
                //}
                oMan.DrawAll(_spriteBatch);
                
                _spriteBatch.End();
            }

            base.Draw(gameTime);
            _watch.Stop();
            curFPS = _watch.ElapsedMilliseconds;
            _watch.Reset();
        }



        public void InitObjects()
        {
            FPSFont =  Content.Load<SpriteFont>("Font/Font");
            tilelist = new List<Rectangle>();     
            _graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width/2;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height/2;
            _graphics.ApplyChanges();
            WindowRectangle = new Rectangle(0, 0, Game2.Graphics.PreferredBackBufferWidth, Game2.Graphics.PreferredBackBufferHeight);
            Level.screenWidth = _graphics.PreferredBackBufferWidth;
            Level.screenHeight = _graphics.PreferredBackBufferHeight;
        }
    }
    }
