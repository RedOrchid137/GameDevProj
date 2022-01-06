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
using GameTest1.State;
using GameTest1.States;
using GameTest1.UI;

namespace GameTest1
{
    public abstract class GameBase : Game
    {
        //testing vars
        protected static GraphicsDeviceManager _graphics;
        internal static GraphicsDeviceManager Graphics { get { return _graphics; }}

        protected static ObjectManager oMan { get; set; }
        internal static ObjectManager ObjManager { get { return oMan; } set { oMan = value; } }


        internal static List<Rectangle> tilelist { get; set; }
        internal static Rectangle WindowRectangle { get; set; }
        internal static Level CurLevel { get; set; }

        internal SpriteBatch _spriteBatch { get; set; }
        internal Character player;




        //Rendering

        internal Camera _camera;
        protected TiledMapRenderer _tiledMapRenderer;
        protected UIOverlay UI;


        protected Status _huidigeStatus;
        protected Status _volgendeStatus;

        public GameBase()
        {
            oMan = new ObjectManager();
            UI = new UIOverlay();
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
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, CurLevel.Map);
            _camera = new Camera();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();
            UI.Update(gameTime, CurLevel, _spriteBatch);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, CurLevel.Map);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (_huidigeStatus.GetType() == typeof(MenuState))
            {
                _huidigeStatus.Draw(gameTime, _spriteBatch);
            }
            else if (_huidigeStatus.GetType() == typeof(GameState))
            {
                _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null,Matrix.CreateScale(CurLevel.BackgroundScale));
                //_spriteBatch.Draw(CurLevel.Background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, CurLevel.BackgroundScale, SpriteEffects.None, 0f);
                _spriteBatch.Draw(CurLevel.Background,new Rectangle(0,0,CurLevel.Background.Width,CurLevel.Background.Height),Color.White);
                _spriteBatch.End();
                _spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: _camera.Transform);
                
                foreach (var item in CurLevel.Map.Layers)
                {
                    _tiledMapRenderer.Draw(item, _camera.Transform);
                }
                
                
                //foreach (var item in tilelist)
                //{
                //    _spriteBatch.Draw(ExtensionMethods.BlankTexture(_spriteBatch), new Vector2(item.X, item.Y), item, Color.Red * 0.5f);
                //}
                oMan.DrawAll(_spriteBatch);
                
                _spriteBatch.End();

                _spriteBatch.Begin();
                UI.Draw(_spriteBatch);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }



        protected void InitObjects()
        {
            tilelist = new List<Rectangle>();     
            _graphics.PreferredBackBufferWidth = (int)(GraphicsDevice.DisplayMode.Width/1.3);
            _graphics.PreferredBackBufferHeight = (int)(GraphicsDevice.DisplayMode.Height / 1.3);
            _graphics.ApplyChanges();
            WindowRectangle = new Rectangle(0, 0, Game2.Graphics.PreferredBackBufferWidth, Game2.Graphics.PreferredBackBufferHeight);
            Level.screenWidth = _graphics.PreferredBackBufferWidth;
            Level.screenHeight = _graphics.PreferredBackBufferHeight;
        }
    }
    }
