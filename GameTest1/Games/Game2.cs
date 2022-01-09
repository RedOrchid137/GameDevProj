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
using static GameTest1.Entities.Collectible;

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
            Level1 = new Level(Content.Load<TiledMap>("TileMapResources/Levels/Level1"), 32, Content.Load<Texture2D>("TileMapResources/Levels/Background"), 3, new Vector2(2, 17));
            Spritesheet foxsheet = new Spritesheet(Content.Load<Texture2D>("Characters/Fox Sprite Sheet"), new List<int> { 5, 14, 8, 11, 5, 6, 7 });
            Spritesheet trollsheet = new Spritesheet(Content.Load<Texture2D>("Enemies/Shardsoul Slayer Sprite Sheet"), new List<int> { 8, 8, 5, 4, 6 });
            Spritesheet huntersheet = new Spritesheet(Content.Load<Texture2D>("Enemies/SpriteSheetHuntress"), new List<int> { 6, 2, 10, 8, 10, 3, 2 });
            Spritesheet raspberrysheet = new Spritesheet(Content.Load<Texture2D>("Collectibles/Raspberry"), new List<int> { 6 });
            

            player = new Character(foxsheet, WindowRectangle, Level1, Level1.StartingTile, new KeyboardReader(), 2f, 5);
            CurPlayer = player;
            TrollEnemy troll = new TrollEnemy(trollsheet, WindowRectangle, Level1, new Vector2(2, 5), new Vector2(1, 12), 1.5f,3);
            TrollEnemy troll2 = new TrollEnemy(trollsheet, WindowRectangle, Level1, new Vector2(36, 20), new Vector2(27, 42), 1.5f, 4);
            Collectible berry1 = new Collectible(raspberrysheet, WindowRectangle, Level1, new Vector2(45, 5), CollectibleType.Raspberry, 1.5f);
            Collectible berry2 = new Collectible(raspberrysheet, WindowRectangle, Level1, new Vector2(24, 17), CollectibleType.Raspberry, 1.5f);
            Collectible berry3 = new Collectible(raspberrysheet, WindowRectangle, Level1, new Vector2(2, 4), CollectibleType.Raspberry, 1.5f);
            HunterEnemy huntress = new HunterEnemy(huntersheet, Content.Load<Texture2D>("Arrows/Static"), WindowRectangle, Level1, new Vector2(43, 5), Vector2.Zero, 10, 1.3f, 1);

            Level1.Player = player;

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
            Level2 = new Level(Content.Load<TiledMap>("TileMapResources/Levels/Level2"), 32, Content.Load<Texture2D>("TileMapResources/Levels/Background"), 5, new Vector2(2, 25));
            Spritesheet trollsheet = new Spritesheet(Content.Load<Texture2D>("Enemies/Shardsoul Slayer Sprite Sheet"), new List<int> { 8, 8, 5, 4, 6 });
            Spritesheet huntersheet = new Spritesheet(Content.Load<Texture2D>("Enemies/SpriteSheetHuntress"), new List<int> { 6, 2, 10, 8, 10, 3, 2 });
            Spritesheet skeletonsheet = new Spritesheet(Content.Load<Texture2D>("Enemies/Skeleton"), new List<int> { 13, 13, 12, 4, 3 });
            
            Spritesheet raspberrysheet = new Spritesheet(Content.Load<Texture2D>("Collectibles/Raspberry"), new List<int> { 6 });
            Spritesheet blueberrysheet = new Spritesheet(Content.Load<Texture2D>("Collectibles/blueberry"), new List<int> { 6 });
            
            TrollEnemy troll = new TrollEnemy(trollsheet, WindowRectangle, Level2, new Vector2(29, 9), new Vector2(24, 32), 1.5f, 3);
            //TrollEnemy troll2 = new TrollEnemy(trollsheet, WindowRectangle, Level2, new Vector2(41, 24), new Vector2(37, 45), 1.5f, 5);
            SkeletonEnemy skeleton1 = new SkeletonEnemy(skeletonsheet, WindowRectangle, Level1, new Vector2(41, 24), new Vector2(37, 45), 1.5f, 3);
            Collectible berry1 = new Collectible(raspberrysheet, WindowRectangle, Level2, new Vector2(1, 1),CollectibleType.Raspberry, 1.5f);
            Collectible berry2 = new Collectible(raspberrysheet, WindowRectangle, Level2, new Vector2(18, 25), CollectibleType.Raspberry, 1.5f);
            Collectible berry3 = new Collectible(raspberrysheet, WindowRectangle, Level2, new Vector2(35, 6), CollectibleType.Raspberry, 1.5f);
            Collectible berry4 = new Collectible(raspberrysheet, WindowRectangle, Level2, new Vector2(47, 8), CollectibleType.Raspberry, 1.5f);
            Collectible berry5 = new Collectible(raspberrysheet, WindowRectangle, Level2, new Vector2(12,24), CollectibleType.Raspberry, 1.5f);
            Collectible blueberry1 = new Collectible(blueberrysheet, WindowRectangle, Level2, new Vector2(1, 5), CollectibleType.BlueBerry, 1.5f);
            Collectible blueberry2 = new Collectible(blueberrysheet, WindowRectangle, Level2, new Vector2(24, 6), CollectibleType.BlueBerry, 1.5f);
            HunterEnemy huntress = new HunterEnemy(huntersheet, Content.Load<Texture2D>("Arrows/Static"), WindowRectangle, Level2, new Vector2(32, 16), Vector2.Zero, 10, 1.3f, 1);

            Level2.Player = player;
            Level2.oMan.ObjectList.Add(player);
            Level2.oMan.ObjectList.Add(troll);
            //Level2.oMan.ObjectList.Add(troll2);
            Level2.oMan.ObjectList.Add(skeleton1);
            Level2.oMan.ObjectList.Add(berry1);
            Level2.oMan.ObjectList.Add(berry2);
            Level2.oMan.ObjectList.Add(berry3);
            Level2.oMan.ObjectList.Add(berry4);
            Level2.oMan.ObjectList.Add(berry5);
            Level2.oMan.ObjectList.Add(blueberry1);
            Level2.oMan.ObjectList.Add(blueberry2);
            Level2.oMan.ObjectList.Add(huntress);


            //player.StartingTile = Level2.StartingTile;
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
            SoundEffect heal = Content.Load<SoundEffect>("Sound/Heal");
            SoundEffect shot = Content.Load<SoundEffect>("Sound/BowShot");
            SoundEffect speedup = Content.Load<SoundEffect>("Sound/SpeedUp");

            SoundLibrary[SoundType.CharHit] = charhit;
            SoundLibrary[SoundType.EnemyHit] = enemyhit;
            SoundLibrary[SoundType.Jump] = jump;
            SoundLibrary[SoundType.Collect] = collect;
            SoundLibrary[SoundType.Click] = click;
            SoundLibrary[SoundType.Lose] = lose;
            SoundLibrary[SoundType.Victory] = win;
            SoundLibrary[SoundType.LevelComplete] = levelComplete;
            SoundLibrary[SoundType.Heal] = heal;
            SoundLibrary[SoundType.Shot] = shot;
            SoundLibrary[SoundType.SpeedUp] = speedup;
        }

        protected override void LoadUI()
        {
            HealthBar healthbar = new HealthBar(Content.Load<Texture2D>("UI/FullHeart"), Content.Load<Texture2D>("UI/HalfHeart"), WindowRectangle, _spriteBatch, 3f, 60, 0.1f);
            Score score = new Score(WindowRectangle, _spriteBatch, Content.Load<SpriteFont>("Font/ScoreFont"), Color.Red);

            UI.AddElement(new Vector2(30, 30), healthbar);
            UI.AddElement(new Vector2(30, WindowRectangle.Height-150), score);
        }
    }
}
