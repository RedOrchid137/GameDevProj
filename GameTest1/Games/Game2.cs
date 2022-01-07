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
using GameTest1.Enemies;
using GameTest1.UI;
using GameTest1.Engine;
using GameTest1.Abstracts;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace GameTest1
{

    //Alle generieke dingen gaan via GameBase zodat de Game klasses zelf overzichtelijk blijven.
    public class Game2 : GameBase
    {

        public Level Level1 { get; set; }
        public Level Level2 { get; set; }

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
            _camera = new Camera();

            //tiledmap klasse gebruikt .tmx bestanden uit Tiled software om de TileLayers te gebruiken voor collision detection
            //tiledmap renderer tekent de TileLayer
            base.InitObjects();

            LoadSounds();
            LoadUI();
            LoadLevels();
            lvlAmount = lvlList.Count;
            
            MenuState = new MenuState(this, _graphics.GraphicsDevice, Content);
            GameState = new GameState(this, _graphics.GraphicsDevice, Content);          
            CurrentState = MenuState;
            
            CurLevel = Level1;
            lvlCount = 0;          
            base.LoadContent();
            InitObjects();
            CurLevel.Player = CurPlayer;
        }

        protected override void Update(GameTime gameTime)
        {


            if (CurPlayer.EndGame&&!Victory&&!StaticScreen)
            {
                Game2.SoundLibrary[GameBase.SoundType.Lose].Play();
                MediaPlayer.Stop();
                this.GameOverState = new GameOverState(this, _graphics.GraphicsDevice, Content, Content.Load<Texture2D>("UI/GameOverScreen"));
                CurrentState = GameOverState;
                StaticScreen = true;
            }
            else if (CurPlayer.EndGame && Victory && !StaticScreen)
            {
                Game2.SoundLibrary[GameBase.SoundType.Victory].Play();
                MediaPlayer.Stop();
                this.GameOverState = new GameOverState(this, _graphics.GraphicsDevice, Content, Content.Load<Texture2D>("UI/WinScreen"));
                CurrentState = GameOverState;
                StaticScreen = true;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape)) 
            {
                if (CurrentState == GameOverState)
                {
                    Exit();
                }
                (this.MenuState as MenuState).StartKnop.Text = "Continue";
                CurrentState = MenuState;
            }

            if (CurrentState == this.GameState)
            {
                CurLevel.oMan.UpdateAll(gameTime, CurLevel, _spriteBatch);
                _camera.Follow(player);
                _tiledMapRenderer.Update(gameTime);
                
            }
            CurrentState.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void LoadLevels()
        {
            LoadLevel1();
            LoadLevel2();
        }


        public void LoadLevel1()
        {
            Level1 = new Level(Content.Load<TiledMap>("TileMapResources/Level1/Level1"), 32, Content.Load<Texture2D>("TileMapResources/Level1/Background"), 3, new Vector2(3, 18));
            Spritesheet foxsheet = new Spritesheet(Content.Load<Texture2D>("Characters/Fox Sprite Sheet"), new List<int> { 5, 14, 8, 11, 5, 6, 7 });
            Spritesheet trollsheet = new Spritesheet(Content.Load<Texture2D>("Enemies/Shardsoul Slayer Sprite Sheet"), new List<int> { 8, 8, 5, 4, 6 });
            Spritesheet huntersheet = new Spritesheet(Content.Load<Texture2D>("Enemies/SpriteSheetHuntress"), new List<int> { 6, 2, 10, 8, 10, 3, 2 });
            Spritesheet berrysheet = new Spritesheet(Content.Load<Texture2D>("Collectibles/Raspberry"), new List<int> { 6 });

            player = new Character(foxsheet, WindowRectangle, Level1, new Vector2(2, 19), new KeyboardReader(), 2f, 5);
            CurPlayer = player;
            TrollEnemy troll = new TrollEnemy(trollsheet, WindowRectangle, Level1, new Vector2(2, 6), new Vector2(1, 12), 1.5f, 2);
            TrollEnemy troll2 = new TrollEnemy(trollsheet, WindowRectangle, Level1, new Vector2(36, 20), new Vector2(27, 42), 1.5f, 2);
            Collectible berry1 = new Collectible(berrysheet, WindowRectangle, Level1, new Vector2(45, 5), 1.5f);
            Collectible berry2 = new Collectible(berrysheet, WindowRectangle, Level1, new Vector2(24, 17), 1.5f);
            Collectible berry3 = new Collectible(berrysheet, WindowRectangle, Level1, new Vector2(2, 4), 1.5f);
            HunterEnemy huntress = new HunterEnemy(huntersheet, Content.Load<Texture2D>("Arrows/Static"), WindowRectangle, Level1, new Vector2(43, 5), Vector2.Zero, 10, 1.3f, 1);
            Level1.oMan.ObjectList.Add(player);
            Level1.oMan.ObjectList.Add(troll);
            Level1.oMan.ObjectList.Add(troll2);
            Level1.oMan.ObjectList.Add(berry1);
            Level1.oMan.ObjectList.Add(berry2);
            Level1.oMan.ObjectList.Add(berry3);
            Level1.oMan.ObjectList.Add(huntress);

            lvlList.Add(Level1);

        }

        public void LoadLevel2()
        {
            Level2 = new Level(Content.Load<TiledMap>("TileMapResources/Level1/Level2"), 32, Content.Load<Texture2D>("TileMapResources/Level1/Background"), 3, new Vector2(2, 24));
            lvlList.Add(Level2);
        }


        protected override void LoadSounds()
        {
            this.BGMusic = Content.Load<Song>("Sound/BGTrack");

            MediaPlayer.Play(BGMusic);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;

            SoundEffect charhit = Content.Load<SoundEffect>("Sound/Hit1");
            SoundEffect enemyhit = Content.Load<SoundEffect>("Sound/HurtEnemy");
            SoundEffect jump = Content.Load<SoundEffect>("Sound/jump1");
            SoundEffect collect = Content.Load<SoundEffect>("Sound/Collect1");
            SoundEffect click = Content.Load<SoundEffect>("Sound/click");
            SoundEffect lose = Content.Load<SoundEffect>("Sound/Lost");
            SoundEffect win = Content.Load<SoundEffect>("Sound/Victory");
            SoundEffect levelComplete = Content.Load<SoundEffect>("Sound/LevelComplete");

            SoundLibrary[SoundType.CharHit] = charhit;
            SoundLibrary[SoundType.EnemyHit] = enemyhit;
            SoundLibrary[SoundType.Jump] = jump;
            SoundLibrary[SoundType.Collect] = collect;
            SoundLibrary[SoundType.Click] = click;
            SoundLibrary[SoundType.Lose] = lose;
            SoundLibrary[SoundType.Victory] = win;
            SoundLibrary[SoundType.LevelComplete] = levelComplete;
        }

        protected override void LoadUI()
        {
            HealthBar healthbar = new HealthBar(Content.Load<Texture2D>("UI/FullHeart"), Content.Load<Texture2D>("UI/HalfHeart"), WindowRectangle, _spriteBatch, 3f, 60, 0.1f);
            Score score = new Score(WindowRectangle, _spriteBatch, Content.Load<SpriteFont>("Font/ScoreFont"), Color.White, new Vector2(200, 200));

            UI.AddElement(new Vector2(30, 30), healthbar);
            UI.AddElement(new Vector2(WindowRectangle.Width - 300, WindowRectangle.Height - 200), score);
        }
    }
}
