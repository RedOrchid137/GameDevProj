using GameTest1.Abstracts;
using GameTest1.Enemies;
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

            if (movable.InputReader.IsDestinationInput&&movable.Alive==true)
            {
                //Sprite orientation + horiz. speed

                //Detect if keypress has persisted, if collision occurred, don't update position
                //To prevent stuttering
                if (!(movable.Speed.X == 0 && movable.hKeyPressed))
                {
                    
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
                
                if (movable.CurPosition != movable.PrevPosition)
                {
                    movable.Offsets = ExtensionMethods.CalcOffsets(movable.curAnimation.CurrentFrame.SourceRectangle, movable.curAnimation.CurrentFrame.HitBox);
                    movable.CollisionRectangle = new Rectangle((int)(movable.CurPosition.X + movable.Speed.X + movable.Offsets.X / 2 * movable.Scale), (int)(movable.CurPosition.Y + movable.Speed.Y + movable.Offsets.Y * movable.Scale), (int)(movable.curAnimation.CurrentFrame.HitBox.Width * movable.Scale), (int)(movable.curAnimation.CurrentFrame.HitBox.Height * movable.Scale));
                    curLevel.CheckCollision(movable, sb);
                    movable.CollisionCheckFull(Game2.CurLevel.oMan);
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
        public static void MoveEnemy(Enemy movable, Level curLevel, SpriteBatch sb)
        {
            int spriteheight = movable.curAnimation.CurrentFrame.SourceRectangle.Height;

            var hunter = movable as HunterEnemy;
            var troll = movable as TrollEnemy;
            //Update Direction and Flip Sprite

            movable.FlipFlagX = !movable.Direction;
            if (movable.Attacking&&hunter!=null)
            {
                if (movable.FlipFlagX)
                {
                    movable.Speed = new Vector2(0.1f, movable.Speed.Y);
                }
                else
                {
                    movable.Speed = new Vector2(-0.1f, movable.Speed.Y);
                }
            }
            else if(troll!=null)
            {
                if (movable.Direction)
                {
                    movable.Speed = new Vector2(movable.Speed.X + movable.Acceleration.X, movable.Speed.Y);
                }
                else
                {
                    movable.Speed = new Vector2(movable.Speed.X - movable.Acceleration.X, movable.Speed.Y);
                }
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

            //Update Position after move
            movable.CurPosition += movable.Speed;

            //update FOV Rectangle
            movable.UpdateFOV();


            //Handle Player Observed
            if (movable.CurLevel.Player.CollisionRectangle.Intersects(movable.FieldOfView))
            {
                movable.Chasing = true;
            }
            else
            {
                movable.Chasing = false;
                movable.Running = false;
                movable.MaxSpeed = movable.InitialMaxSpeed;
            }
            if (movable.Chasing&&!movable.Running)
            {
                movable.MaxSpeed += 2;
                movable.Running = true;
            }


            //Handle Path Limit Reached
            if (movable.CurPosition.X >= movable.Path.Y)
            {
                movable.Direction = false;
            }
            else if (movable.CurPosition.X <= movable.Path.X)
            {
                movable.Direction = true;
            }


            movable.Offsets = ExtensionMethods.CalcOffsets(movable.curAnimation.CurrentFrame.SourceRectangle, movable.curAnimation.CurrentFrame.HitBox);
            movable.CollisionRectangle = new Rectangle((int)(movable.CurPosition.X + movable.Speed.X + movable.Offsets.X / 2 * movable.Scale), (int)(movable.CurPosition.Y + movable.Speed.Y + movable.Offsets.Y * movable.Scale), (int)(movable.curAnimation.CurrentFrame.HitBox.Width * movable.Scale), (int)(movable.curAnimation.CurrentFrame.HitBox.Height * movable.Scale));
            //Collision Detection if only if Sprite has moved
            if (movable.CurPosition != movable.PrevPosition)
            {

                curLevel.CheckCollision(movable, sb);
                //movable.CollisionCheckFull(Game2.ObjManager);
                CollisionManager.HandleCollisionsCharacter(movable);
            }


            //Handle Sprite without Support
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


            //Reset position if out of range
            if (movable.CurPosition.Y > 1000)
            {
                movable.CurPosition = movable.StartingTile;
            }
        }

        public static void MoveArrow(Arrow a, Level curLevel, SpriteBatch sb)
        {
            if (a.FlipFlagX)
            {
                a.CurPosition = new Vector2(a.CurPosition.X - a.Hunter.ArrowSpeed, a.CurPosition.Y);
            }
            else
            {
                a.CurPosition = new Vector2(a.CurPosition.X + a.Hunter.ArrowSpeed, a.CurPosition.Y);
            }


            a.CollisionRectangle = new Rectangle((int)a.CurPosition.X, (int)a.CurPosition.Y, a.Texture.Width, a.Texture.Height);

            curLevel.CheckCollision(a, sb);
            a.CollisionCheckFull(Game2.CurLevel.oMan);
            CollisionManager.HandleCollisionsProjectile(a);

        }

    }

}

