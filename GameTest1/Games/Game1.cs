using GameTest1.Animations;
using GameTest1.Engine;
using GameTest1.GameObjects;
using GameTest1.Inputs;
using GameTest1.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace GameTest1
{
    public class Game1 : Game
    {
        //testing vars
        int count = 0;
        private static GraphicsDeviceManager _graphics;
        public static GraphicsDeviceManager Graphics { get { return _graphics; }}
        private static ObjectManager oMan;
        public static ObjectManager ObjManager { get { return oMan; } set { oMan = value; } }

        SpriteBatch spriteBatch;

        private List<ScrollingBackground> _scrollingBackgrounds;


        public Game1()
        {
            oMan = new ObjectManager();
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = World.screenWidth;
            _graphics.PreferredBackBufferHeight = World.screenHeight;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            InitObjects();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            
        }
        public void InitObjects()
        {
            Character testchar;
            Static testblock;
            Rectangle window = new Rectangle(0, 0, Game1.Graphics.PreferredBackBufferWidth, Game1.Graphics.PreferredBackBufferHeight);

            Spritesheet test = new Spritesheet(Content.Load<Texture2D>("Fox Sprite Sheet"), new List<int> { 5, 14, 8, 11, 5, 6, 7 });
            Spritesheet grass = new Spritesheet(Content.Load<Texture2D>("GrassBlock"), new List<int> { 1 });
            //Spritesheet test = new Spritesheet(Content.Load<Texture2D>("ground_monk_FREE_v1.2-SpriteSheet_288x128"), new List<int> { 6,8,3,6,12,24,25,16,6,13,6,14 });
            
            testchar = new Character(test, new SpriteBatch(Game1.Graphics.GraphicsDevice), window, new KeyboardReader(), 2.5f, 5);
            testblock = new Static(grass, new SpriteBatch(Game1.Graphics.GraphicsDevice), window, 2f);
            testblock.CurPosition = new Vector2(World.screenWidth / 2, World.FloorHeight - testblock.Texture.Height * testblock.Scale);

            oMan.ObjectList.Add(testchar);
            oMan.ObjectList.Add(testblock);

            _scrollingBackgrounds = new List<ScrollingBackground>()
            {
                new ScrollingBackground(Content.Load<Texture2D>("ScrollingBackground/Trees"), testchar, 60f)
                {
                  Layer = 0.99f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("ScrollingBackground/Floor"), testchar, 60f)
                {
                  Layer = 0.9f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("ScrollingBackground/Hills_Front"), testchar, 40f)
                {
                  Layer = 0.8f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("ScrollingBackground/Hills_Middle"), testchar, 30f)
                {
                  Layer = 0.79f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("ScrollingBackground/Clouds_Fast"), testchar, 25f, true)
                {
                  Layer = 0.78f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("ScrollingBackground/Hills_Back"), testchar, 0f)
                {
                  Layer = 0.77f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("ScrollingBackground/Clouds_Slow"), testchar, 10f, true)
                {
                  Layer = 0.7f,
                },
                new ScrollingBackground(Content.Load<Texture2D>("ScrollingBackground/Sky"), testchar, 0f)
                {
                  Layer = 0.1f,
                },
            };

        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();
            oMan.UpdateAll(gameTime);
            foreach (var sb in _scrollingBackgrounds)
                sb.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (var sb in _scrollingBackgrounds)
                sb.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            oMan.DrawAll();
            
            base.Draw(gameTime);
        }
        }
    }
