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
        //States
        public bool Chasing { get; set; }
        public bool Running { get; set; }
        public bool Attacking { get; set; }
        public bool Direction { get; set; }
        public bool ToBeRemoved { get; set; }

        //Calc vars
        public float AttackRange { get; set; }
        public Vector2 Path { get; set; }
        public Rectangle FieldOfView { get; set; }
        public float InitialMaxSpeed { get; set; }

        //Starting Tile is de coordinaat van de tegel waarop een gameobject zal spawnen. 
        //Niet hetzelfde als pixel coords, het is de coordinaat van de tegel in een 2d array zogezegd
        //Voor pixel coords wordt dit nog eens vermenigvuldigd met de breedte(in pixels) van een tegel
        public Enemy(Spritesheet spritesheet, Rectangle window, Level curlevel, Vector2 startingtile,Vector2 path, float scale = 1, float maxSpeed = 5f) : base(spritesheet, window, curlevel,startingtile, scale, maxSpeed)
        {
            this.Chasing = false;
            this.Running = false;
            Path = new Vector2(path.X*CurLevel.TileWidth,path.Y*CurLevel.TileWidth);
            this.InitialMaxSpeed = maxSpeed;
            CurLevel = curlevel;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (FlipFlagX)
            {
                //Texture
                spriteBatch.Draw(_texture, CurPosition, curAnimation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0f);
                
                //CollisionRectangle
                //spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch), new Vector2(CollisionRectangle.X, CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);           
                
                //Field of View               
                //spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch), new Vector2(FieldOfView.X, FieldOfView.Y), FieldOfView, Color.Blue * 0.1f);
            }
            else
            {
                
                spriteBatch.Draw(_texture, CurPosition, curAnimation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                //spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch), new Vector2(CollisionRectangle.X, CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);
                //spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch), new Vector2(FieldOfView.X, FieldOfView.Y), FieldOfView, Color.Blue * 0.1f);
            }
        }

        public override void Update(GameTime gametime, Level curLevel, SpriteBatch sb)
        {
            //Update Location
            MovementManager.MoveEnemy(this, curLevel, sb);

            //update FOV Rectangle
            UpdateFOV();


            //Handle Player Observed
            if (CurLevel.Player.CollisionRectangle.Intersects(FieldOfView))
            {
                Chasing = true;
            }
            else
            {
                Chasing = false;
                Running = false;
                MaxSpeed = InitialMaxSpeed;
            }
            if (Chasing && !Running)
            {
                MaxSpeed += 2;
                Running = true;
            }

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
                    if (distance>CurLevel.TileWidth*CurLevel.Map.Width)
                    {
                        break;
                    }
                }
            }
            else 
            {
                scanRect = new Rectangle(this.CollisionRectangle.Left, this.CollisionRectangle.Y,1,1);
                while (!CurLevel.existsTile(scanRect.X, scanRect.Y, scanRect))
                {
                    scanRect = new Rectangle(scanRect.X - 1, scanRect.Y,1,1);
                    distance++;
                    if (distance > CurLevel.TileWidth * CurLevel.Map.Width)
                    {
                        break;
                    }
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
        public int distanceToPlayer(Rectangle playerCollisionRectangle)
        {
            Rectangle overlap = Rectangle.Intersect(this.FieldOfView, playerCollisionRectangle);
            int distance = 0;
            if (this.FlipFlagX)
            {
                distance = Math.Abs(overlap.X+overlap.Width - this.CollisionRectangle.Left);
            }
            else
            {
                distance = Math.Abs(overlap.X - this.CollisionRectangle.Right);
            }
            
            return distance;
        }
    }
}
