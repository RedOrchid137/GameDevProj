using GameTest1.Animations;
using GameTest1.Engine;
using GameTest1.GameObjects;
using GameTest1.Inputs;
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
        private ObjectManager oMan;

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
            testblock.Position = new Vector2(World.screenWidth / 2, World.FloorHeight - testblock.Texture.Height * testblock.Scale);

            oMan.ObjectList.Add(testchar);
            oMan.ObjectList.Add(testblock);
        
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();
            oMan.UpdateAll(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            oMan.DrawAll();
            base.Draw(gameTime);
        }
        }
    }
