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
        private static GraphicsDeviceManager _graphics;
        public static GraphicsDeviceManager Graphics { get { return _graphics; }}
        Character testchar;
        Static testblock;
        private List<GameObject>objectList = new List<GameObject>();


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = World.screenWidth;
            _graphics.PreferredBackBufferHeight = World.screenHeight;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Rectangle window = new Rectangle(0,0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            Spritesheet test = new Spritesheet(Content.Load<Texture2D>("Fox Sprite Sheet"), new List<int> { 5, 14, 8, 11, 5, 6, 7 });
            Spritesheet grass = new Spritesheet(Content.Load<Texture2D>("GrassBlock"), new List<int> {1});
            //Spritesheet test = new Spritesheet(Content.Load<Texture2D>("ground_monk_FREE_v1.2-SpriteSheet_288x128"), new List<int> { 6,8,3,6,12,24,25,16,6,13,6,14 });
            testchar = new Character(test,new SpriteBatch(GraphicsDevice),window, new KeyboardReader(),2.5f,5);

            testblock = new Static(grass,new SpriteBatch(GraphicsDevice),window,2.5f);
            testblock.Position = new Vector2(World.screenWidth/2, World.FloorHeight-testblock.Texture.Height*testblock.Scale);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();

            testchar.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Debug.WriteLine(1/gameTime.ElapsedGameTime.TotalSeconds);
            GraphicsDevice.Clear(Color.White);
            testchar.Draw(testchar.curAnimation.CurrentFrame.SourceRectangle);
            testblock.Draw();
            base.Draw(gameTime);
        }
        }
    }
