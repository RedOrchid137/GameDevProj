﻿using GameTest1.Engine;
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
        public static void MoveCharacter(GameObject movable,Level curLevel,SpriteBatch sb)
        {
            int spriteheight = movable.curAnimation.CurrentFrame.SourceRectangle.Height;
            var direction = movable.InputReader.ReadInput();
            if (movable.InputReader.IsDestinationInput)
            {
                //Sprite orientation + horiz. speed
                if (direction.X > 0)
                {
                    movable.FlipFlagX = false;
                    movable.Speed = new Vector2(movable.Speed.X + movable.Acceleration.X, movable.Speed.Y);
                }
                else if (direction.X < 0)
                {
                    movable.FlipFlagX = true;
                    movable.Speed = new Vector2(movable.Speed.X - movable.Acceleration.X, movable.Speed.Y);
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
                    Debug.WriteLine(movable.CurPosition + "cur ----- prev" + movable.PrevPosition);
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
                    Debug.WriteLine("set ground to 99999999");
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
        public static void MoveEnemy(GameObject movable, Level curLevel, SpriteBatch sb)
        {
            int spriteheight = movable.curAnimation.CurrentFrame.SourceRectangle.Height;
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
                Debug.WriteLine(movable.CurPosition + "cur ----- prev" + movable.PrevPosition);
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
                Debug.WriteLine("set ground to 99999999");
            }
            movable.PrevPosition = movable.CurPosition;
            //if (movable.CurPosition.Y >= movable.Ground - spriteheight * movable.Scale)
            //{
            //    movable.IsFalling = false;
            //    movable.CurPosition = new Vector2(movable.CurPosition.X, movable.Ground - spriteheight * movable.Scale);
            //}



        }

    }

}

