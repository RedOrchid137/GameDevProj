using GameTest1.Animations;
using GameTest1.Collision;
using GameTest1.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static GameTest1.GameObject;

namespace GameTest1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        Character testchar;
        const int screenWidth = 1600;
        const int screenHeight = 970;
        CollisionManager collisionManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            collisionManager = new CollisionManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Rectangle window = new Rectangle(0,0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            Spritesheet test = new Spritesheet(Content.Load<Texture2D>("Fox Sprite Sheet"), new List<int> { 5, 14, 8, 11, 5, 6, 7 });
            testchar = new Character(test,new SpriteBatch(GraphicsDevice),window,5, new KeyboardReader(),3);
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            testchar.Update(gameTime);
            /*testblok.Update(gameTime)

            if (collisionManager.CheckCollsion(testchar.CollisionRectangle, testblok.CollisionRectangle)
            {
                Debug.WriteLine("Collision detected!");
            }*/
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AliceBlue);
            testchar.Draw(testchar.curAnimation.CurrentFrame.SourceRectangle);
            base.Draw(gameTime);
        }


        }
    }
