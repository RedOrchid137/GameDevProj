using GameTest1.Abstracts;
using GameTest1.Engine;
using GameTest1.Extensions;
using GameTest1.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace GameTest1.Inputs
{
    public class MovementManager
    {
        public static void MoveCharacter(Entity movable,Level curLevel,SpriteBatch sb)
        {
            int spriteheight = movable.curAnimation.CurrentFrame.SourceRectangle.Height;
            var direction = movable.InputReader.ReadInput();

            if (movable.InputReader.IsDestinationInput)
            {
                if (!(movable.Speed.X == 0 && movable.hKeyPressed))
                {
                    //Sprite orientation + horiz. speed
                    if (direction.X > 0)
                    {
                        movable.FlipFlagX = false;
                        movable.Speed = new Vector2(movable.Speed.X + movable.Acceleration.X, movable.Speed.Y);
                        movable.hKeyPressed = true;
                    }
                    else if (direction.X < 0)
                    {
                        movable.FlipFlagX = true;
                        movable.Speed = new Vector2(movable.Speed.X - movable.Acceleration.X, movable.Speed.Y);
                        movable.hKeyPressed = true;
                    }
                    else
                    {
                        if (movable.Speed.X > 0)
                        {
                            float newspeed = movable.Speed.X - Physics.frictionConst;
                            movable.Speed = new Vector2(newspeed < 0 ? 0 : newspeed, movable.Speed.Y);
                        }
                        else if (movable.Speed.X < 0)
                        {
                            float newspeed = movable.Speed.X + Physics.frictionConst;
                            movable.Speed = new Vector2(newspeed > 0 ? 0 : newspeed, movable.Speed.Y);
                        }
                        movable.hKeyPressed = false;
                    }
                }
                else if (direction.X==0)
                {
                    movable.hKeyPressed = false;
                }


                //handle jumping and falling

                if (movable.CurPosition.Y >= movable.Ground - spriteheight * movable.Scale)
                {
                    movable.Speed = new Vector2(movable.Speed.X, 0);
                    movable.IsFalling = false;
                    movable.CurPosition = new Vector2(movable.CurPosition.X, movable.Ground - spriteheight * movable.Scale);
                }
                else if (movable.CurPosition.Y < movable.Ground - spriteheight * movable.Scale && movable.IsFalling == true)
                {
                    movable.Speed = new Vector2(movable.Speed.X, movable.Speed.Y + Physics.gravConst);
                }


                if (direction.Y > 0 && !movable.IsFalling)
                {
                    movable.IsFalling = true;
                    movable.Speed = new Vector2(movable.Speed.X, movable.Speed.Y - movable.Acceleration.Y);
                }
                //if (movable.IsFalling)
                //{
                //    movable.Speed = new Vector2(movable.Speed.X, movable.Speed.Y + Physics.gravConst);
                //}
                //else
                //{
                //    if (direction.Y > 0)
                //    {
                //        movable.Speed = new Vector2(movable.Speed.X, movable.Speed.Y - movable.Acceleration.Y);
                //        movable.IsFalling = true;
                //    }

                //}
                //limit speed
                if (movable.Speed.X > movable.MaxSpeed)
                {
                    movable.Speed = new Vector2(movable.MaxSpeed, movable.Speed.Y);
                }
                if (movable.Speed.X < -movable.MaxSpeed)
                {
                    movable.Speed = new Vector2(-movable.MaxSpeed, movable.Speed.Y);
                }
                if (movable.Speed.Y > Physics.terminalVel)
                {
                    movable.Speed = new Vector2(movable.Speed.X, Physics.terminalVel);
                }
                if (movable.Speed.Y < -Physics.terminalVel)
                {
                    movable.Speed = new Vector2(movable.Speed.X, -Physics.terminalVel);
                }
                movable.CurPosition += movable.Speed;
                movable.Offsets = ExtensionMethods.CalcOffsets(movable.curAnimation.CurrentFrame.SourceRectangle, movable.curAnimation.CurrentFrame.HitBox);
                if (movable.CurPosition != movable.PrevPosition)
                {
                    
                    movable.CollisionRectangle = new Rectangle((int)(movable.CurPosition.X + movable.Speed.X + movable.Offsets.X / 2 * movable.Scale), (int)(movable.CurPosition.Y + movable.Speed.Y + movable.Offsets.Y * movable.Scale), (int)(movable.curAnimation.CurrentFrame.HitBox.Width * movable.Scale), (int)(movable.curAnimation.CurrentFrame.HitBox.Height * movable.Scale));
                    curLevel.CheckCollision(movable, sb);
                    //movable.CollisionCheckFull(Game2.ObjManager);
                    CollisionManager.HandleCollisionsCharacter(movable);
                }
                if (movable.onGround)
                {
                    movable.IsFalling = false;
                }
                else
                {
                    movable.IsFalling = true;
                    movable.Ground = 99999999;
                }
                movable.PrevPosition = movable.CurPosition;
                if (movable.CurPosition.Y > 1000)
                {
                    movable.CurPosition = movable.StartingTile;
                }
                //if (movable.CurPosition.Y >= movable.Ground - spriteheight * movable.Scale)
                //{
                //    movable.IsFalling = false;
                //    movable.CurPosition = new Vector2(movable.CurPosition.X, movable.Ground - spriteheight * movable.Scale);
                //}
            }
        }
        public static void MoveEnemy(Enemy movable, Level curLevel, SpriteBatch sb)
        {
            int spriteheight = movable.curAnimation.CurrentFrame.SourceRectangle.Height;

            movable.FlipFlagX = !movable.Direction;
            if (movable.Direction)
            {
                movable.Speed = new Vector2(movable.Speed.X+movable.Acceleration.X, 0);
            }
            else
            {
                movable.Speed = new Vector2(movable.Speed.X - movable.Acceleration.X, 0);
            }

            //handle jumping and falling

            if (movable.CurPosition.Y >= movable.Ground - spriteheight * movable.Scale)
            {
                movable.Speed = new Vector2(movable.Speed.X, 0);
                movable.IsFalling = false;
                movable.CurPosition = new Vector2(movable.CurPosition.X, movable.Ground - spriteheight * movable.Scale);
            }
            else if (movable.CurPosition.Y < movable.Ground - spriteheight * movable.Scale && movable.IsFalling == true)
            {
                movable.Speed = new Vector2(movable.Speed.X, movable.Speed.Y + Physics.gravConst);
            }

            //limit speed
            if (movable.Speed.X > movable.MaxSpeed)
            {
                movable.Speed = new Vector2(movable.MaxSpeed, movable.Speed.Y);
            }
            if (movable.Speed.X < -movable.MaxSpeed)
            {
                movable.Speed = new Vector2(-movable.MaxSpeed, movable.Speed.Y);
            }
            if (movable.Speed.Y > Physics.terminalVel)
            {
                movable.Speed = new Vector2(movable.Speed.X, Physics.terminalVel);
            }
            if (movable.Speed.Y < -Physics.terminalVel)
            {
                movable.Speed = new Vector2(movable.Speed.X, -Physics.terminalVel);
            }
            movable.CurPosition += movable.Speed;


            movable.UpdateFOV();
            if (movable.CurLevel.Player.CollisionRectangle.Intersects(movable.FieldOfView))
            {
                movable.Chasing = true;
            }
            else
            {
                movable.Chasing = false;
                movable.Running = false;
                movable.MaxSpeed = 5;
            }
            if (movable.Chasing&&!movable.Running)
            {
                movable.MaxSpeed += 2;
                movable.Running = true;
            }

            if (movable.CurPosition.X >= movable.Path.Y)
            {
                movable.Direction = false;
            }
            else if (movable.CurPosition.X <= movable.Path.X)
            {
                movable.Direction = true;
            }
            movable.Offsets = ExtensionMethods.CalcOffsets(movable.curAnimation.CurrentFrame.SourceRectangle, movable.curAnimation.CurrentFrame.HitBox);

            if (movable.CurPosition != movable.PrevPosition)
            {
                
                movable.CollisionRectangle = new Rectangle((int)(movable.CurPosition.X + movable.Speed.X + movable.Offsets.X / 2 * movable.Scale), (int)(movable.CurPosition.Y + movable.Speed.Y + movable.Offsets.Y * movable.Scale), (int)(movable.curAnimation.CurrentFrame.HitBox.Width * movable.Scale), (int)(movable.curAnimation.CurrentFrame.HitBox.Height * movable.Scale));
                curLevel.CheckCollision(movable, sb);
                //movable.CollisionCheckFull(Game2.ObjManager);
                CollisionManager.HandleCollisionsCharacter(movable);
            }

            if (movable.onGround)
            {
                movable.IsFalling = false;
            }
            else
            {
                movable.IsFalling = true;
                movable.Ground = 99999999;
            }
            movable.PrevPosition = movable.CurPosition;
            if (movable.CurPosition.Y > 1000)
            {
                movable.CurPosition = movable.StartingTile;
            }
        }

    }

}

