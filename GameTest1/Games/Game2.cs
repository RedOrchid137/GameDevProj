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
using GameTest1.Enemies;
using GameTest1.UI;
using GameTest1.Engine;

namespace GameTest1
{

    //Alle generieke dingen gaan via GameBase zodat de Game klasses zelf overzichtelijk blijven.
    public class Game2 : GameBase
    {

        public Level Level1 { get; set; }
        public Level Level2 { get; set; }

        GameAction<Object> testaction;
        public static Character CurPlayer { get; set; }
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
            //tiledmap renderer tekent de TileLayer
            base.InitObjects();
            Level1 = new Level(Content.Load<TiledMap>("TileMapResources/Level1/Level1"),32, Content.Load<Texture2D>("TileMapResources/Level1/Background"));
            _huidigeStatus = new MenuState(this, _graphics.GraphicsDevice, Content);
            CurLevel = Level1;
            base.LoadContent();
            InitObjects();
            CurLevel.Player = CurPlayer;
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
            oMan.UpdateAll(gameTime,CurLevel,_spriteBatch);
            _camera.Follow(player);
            _tiledMapRenderer.Update(gameTime);
            testaction.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }



        public new void InitObjects()
        {
            HealthBar healthbar = new HealthBar(Content.Load<Texture2D>("UI/FullHeart"), Content.Load<Texture2D>("UI/HalfHeart"), WindowRectangle,_spriteBatch,3f,60,0.1f);


            UI.AddElement(new Vector2(30,30), healthbar);


            Spritesheet foxsheet = new Spritesheet(Content.Load<Texture2D>("Fox Sprite Sheet"), new List<int> { 5, 14, 8, 11, 5, 6, 7 });
            Spritesheet trollsheet = new Spritesheet(Content.Load<Texture2D>("Shardsoul Slayer Sprite Sheet"), new List<int> { 8,8,5,4,6 });
            Spritesheet huntersheet = new Spritesheet(Content.Load<Texture2D>("Shardsoul Slayer Sprite Sheet"), new List<int> { 8, 8, 5, 4, 6 });

            player = new Character(foxsheet, WindowRectangle,Level1, new Vector2(2, 16), new KeyboardReader(), 2f, 5);
            CurPlayer = player;
            TrollEnemy troll = new TrollEnemy(trollsheet, WindowRectangle, Level1, new Vector2(2,6),new Vector2(1,12),1.5f,2);
            TrollEnemy troll2 = new TrollEnemy(trollsheet, WindowRectangle, Level1, new Vector2(30, 18), new Vector2(26, 45), 1.5f, 2);
            TrollEnemy troll3 = new TrollEnemy(trollsheet, WindowRectangle, Level1, new Vector2(45, 18), new Vector2(26, 45), 1.5f, 2);
            oMan.ObjectList.Add(player);
            oMan.ObjectList.Add(troll);
            oMan.ObjectList.Add(troll2);
            oMan.ObjectList.Add(troll3);
            _camera = new Camera();
     
        }

    }

   
}
