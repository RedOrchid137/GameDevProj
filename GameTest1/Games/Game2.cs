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

namespace GameTest1
{
    public class Game2 : Game
    {
        //testing vars
        private static GraphicsDeviceManager _graphics;
        public static GraphicsDeviceManager Graphics { get { return _graphics; }}

        private static ObjectManager oMan;
        public static ObjectManager ObjManager { get { return oMan; } set { oMan = value; } }
        public static List<Rectangle> tilelist { get; set; }

        
        SpriteBatch _spriteBatch { get; set; }
        Character testchar;
        public Level Level1 { get; set; }
        private Camera _camera;

        TiledMap _tiledMap;
        TiledMapRenderer _tiledMapRenderer;


        public Game2()
        {
            oMan = new ObjectManager();
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = (int)Level.screenWidth;
            _graphics.PreferredBackBufferHeight = (int)Level.screenHeight;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _tiledMap = Content.Load<TiledMap>("TileMapResources/Level1/Level1Simple");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            Level1 = new Level(_tiledMap, 32);
            InitObjects();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();
            oMan.UpdateAll(gameTime,Level1,_spriteBatch);
            _camera.Follow(testchar);
            _tiledMapRenderer.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: _camera.Transform);
            foreach (var item in _tiledMap.Layers)
            {
                _tiledMapRenderer.Draw(item, _camera.Transform);
            }
            foreach (var item in tilelist)
            {
                _spriteBatch.Draw(ExtensionMethods.BlankTexture(_spriteBatch), new Vector2(item.X, item.Y), item, Color.Red * 0.5f);
            }
            oMan.DrawAll(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }



        public void InitObjects()
        {

            Rectangle window = new Rectangle(0, 0, Game2.Graphics.PreferredBackBufferWidth, Game2.Graphics.PreferredBackBufferHeight);
            tilelist = new List<Rectangle>();
            Spritesheet test = new Spritesheet(Content.Load<Texture2D>("Fox Sprite Sheet"), new List<int> { 5, 14, 8, 11, 5, 6, 7 });
            Spritesheet testen = new Spritesheet(Content.Load<Texture2D>("SpriteSheetHuntress"), new List<int> { 6, 10, 2, 3, 10, 2, 8 });
            testchar = new Character(test, window,Level1,new Vector2(0,0), new KeyboardReader(), 2f, 5);
            HunterEnemy testenemy = new HunterEnemy(testen, window, Level1, new Vector2(10, 0),1f,5);
            
            oMan.ObjectList.Add(testchar);
            _camera = new Camera();
        }
    }
    }
