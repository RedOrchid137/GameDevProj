using GameTest1.Animations;
using GameTest1.Extensions;
using GameTest1.Inputs;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GameTest1.Abstracts
{
    public abstract class Enemy:GameObject
    {

        //Starting Tile is de coordinaat van de tegel waarop een gameobject zal spawnen. 
        //Niet hetzelfde als pixel coords, het is de coordinaat van de tegel in een 2d array zogezegd
        //Voor pixel coords wordt dit nog eens vermenigvuldigd met de breedte(in pixels) van een tegel
        public Enemy(Spritesheet spritesheet, Rectangle window, Level curlevel, Vector2 startingtile, float scale = 1, float maxSpeed = 0) : base(spritesheet, window, curlevel, scale, maxSpeed)
        {
            this.Acceleration = new Vector2(0.2f, 5f);
            this.CurLevel = curlevel;
            this.StartingTile = startingtile;
            Debug.WriteLine(StartingTile);
            this.CurPosition = StartingTile;
            this.Ground = CurPosition.Y;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (FlipFlagX)
            {
                spriteBatch.Draw(_texture, CurPosition, curAnimation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.FlipHorizontally, 0f);
                spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch), new Vector2(CollisionRectangle.X, CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);
            }
            else
            {
                spriteBatch.Draw(_texture, CurPosition, curAnimation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(ExtensionMethods.BlankTexture(spriteBatch), new Vector2(CollisionRectangle.X, CollisionRectangle.Y), CollisionRectangle, Color.Red * 0.5f);
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
    }
}
