using GameTest1.Animations;
using GameTest1.Extensions;
using GameTest1.Inputs;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GameTest1.Abstracts
{
    public abstract class Enemy:Entity
    {

        public Vector2 Path { get; set; }

        public bool Direction { get; set; }

        public Rectangle FieldOfView { get; set; }

        public bool Chasing { get; set; }

        public bool Running { get; set; }

        //Starting Tile is de coordinaat van de tegel waarop een gameobject zal spawnen. 
        //Niet hetzelfde als pixel coords, het is de coordinaat van de tegel in een 2d array zogezegd
        //Voor pixel coords wordt dit nog eens vermenigvuldigd met de breedte(in pixels) van een tegel
        public Enemy(Spritesheet spritesheet, Rectangle window, Level curlevel, Vector2 startingtile,Vector2 path, float scale = 1, float maxSpeed = 0) : base(spritesheet, window, curlevel,startingtile, scale, maxSpeed)
        {
            this.Chasing = false;
            this.Running = false;
            this.Direction = true;
            Path = new Vector2(path.X*CurLevel.TileWidth,path.Y*CurLevel.TileWidth);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (FlipFlagX)
            {
                spriteBatch.Draw(_texture, CurPosition, curAnimation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0f);
                spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch), new Vector2(CollisionRectangle.X, CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);           
                spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch), new Vector2(FieldOfView.X, FieldOfView.Y), FieldOfView, Color.Blue * 0.5f);
            }
            else
            {
                spriteBatch.Draw(_texture, CurPosition, curAnimation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch), new Vector2(CollisionRectangle.X, CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);
                spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch), new Vector2(FieldOfView.X, FieldOfView.Y), FieldOfView, Color.Blue * 0.5f);
            }
        }

        public override void Update(GameTime gametime, Level curLevel, SpriteBatch sb)
        {
            //Update Location
            MovementManager.MoveEnemy(this, curLevel, sb);

            //Update Animation
            AnimationManager.setCurrentAnimationEnemy(this);
            this.curAnimation.Update(gametime);
        }

        public int getNearestObstacle()
        {
            Rectangle scanRect;
            int distance = 0;
            if (!this.FlipFlagX)
            {
                scanRect = new Rectangle(this.CollisionRectangle.Right, this.CollisionRectangle.Y, 1, 1);
                while (!CurLevel.existsTile(scanRect.X, scanRect.Y,scanRect))
                {
                    scanRect = new Rectangle(scanRect.X+1, scanRect.Y, 1, 1);
                    distance++;
                }
            }
            else 
            {
                scanRect = new Rectangle(this.CollisionRectangle.Left, this.CollisionRectangle.Y,1,1);
                while (!CurLevel.existsTile(scanRect.X, scanRect.Y, scanRect))
                {
                    scanRect = new Rectangle(scanRect.X - 1, scanRect.Y,1,1);
                    distance++;
                }
            }
            return distance;
        }
        public void UpdateFOV()
        {
            int nearest = getNearestObstacle();
            if (!this.FlipFlagX)
            {
                this.FieldOfView = new Rectangle(this.CollisionRectangle.Right, this.CollisionRectangle.Y,nearest,this.CollisionRectangle.Height);
            }
            else
            {
                this.FieldOfView = new Rectangle(this.CollisionRectangle.X - nearest, this.CollisionRectangle.Y, nearest, this.CollisionRectangle.Height);
            }
        }
    }
}
