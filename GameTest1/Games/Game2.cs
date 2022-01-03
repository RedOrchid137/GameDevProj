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

    //Alle generieke dingen gaan via GameBase zodat de Game klasses zelf overzichtelijk blijven.
    public class Game2 : GameBase
    {


        public Game2():base()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            //tiledmap klasse gebruikt .tmx uit Tiled software om de TileLayers te gebruiken voor collision detection
            //tiledmap renderer tekent de TileLayers
            _tiledMap = Content.Load<TiledMap>("TileMapResources/Level1/Level1Simple");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);


            Level1 = new Level(_tiledMap, 32);
            _huidigeStatus = new MenuState(this, _graphics.GraphicsDevice, Content);
            InitObjects();
        }

        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                Exit();

            if (_volgendeStatus != null)
            {
                _huidigeStatus = _volgendeStatus;

                _volgendeStatus = null;
            }

            _huidigeStatus.Update(gameTime);

            oMan.UpdateAll(gameTime,Level1,_spriteBatch);
            _camera.Follow(testchar);
            _tiledMapRenderer.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }



        public new void InitObjects()
        {
            base.InitObjects();
            Spritesheet test = new Spritesheet(Content.Load<Texture2D>("Fox Sprite Sheet"), new List<int> { 5, 14, 8, 11, 5, 6, 7 });
            Spritesheet testen = new Spritesheet(Content.Load<Texture2D>("Shardsoul Slayer Sprite Sheet"), new List<int> { 8,8,5,4,6 });
            testchar = new Character(test, WindowRectangle,Level1,new Vector2(0,0), new KeyboardReader(), 2f, 5);
            HunterEnemy testenemy = new HunterEnemy(testen, WindowRectangle, Level1, new Vector2(10, 0),1.2f,5);
            
            oMan.ObjectList.Add(testchar);
            oMan.ObjectList.Add(testenemy);
            _camera = new Camera();
        }
    }
    }
