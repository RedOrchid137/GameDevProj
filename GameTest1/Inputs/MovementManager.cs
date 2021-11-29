using GameTest1.Engine;
using GameTest1.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.Inputs
{
    public class MovementManager
    {
        public static void MoveCharacter(GameObject movable)
        {
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
                        movable.Speed = new Vector2( newspeed< 0 ? 0: newspeed, movable.Speed.Y);
                    }
                    else if (movable.Speed.X < 0)
                    {
                        float newspeed = movable.Speed.X + Physics.frictionConst;
                        movable.Speed = new Vector2(newspeed > 0 ? 0 : newspeed, movable.Speed.Y);
                    }
                    
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

                //handle jumping and falling
                if (direction.Y>0&&!(movable as Character).JumpFlag)
                {
                    (movable as Character).JumpFlag = true;
                    movable.Speed = new Vector2(movable.Speed.X, movable.Speed.Y - movable.Acceleration.Y);
                }

                int spriteheight = movable.curAnimation.CurrentFrame.SourceRectangle.Height;
                if (movable.CurPosition.Y>World.FloorHeight-spriteheight*movable.Scale)
                {
                    movable.Speed = new Vector2(movable.Speed.X, 0);
                    (movable as Character).JumpFlag = false;
                    movable.CurPosition = new Vector2(movable.CurPosition.X, World.FloorHeight- spriteheight * movable.Scale);
                }
                else if(movable.CurPosition.Y < World.FloorHeight - spriteheight * movable.Scale)
                {
                    movable.Speed = new Vector2(movable.Speed.X, movable.Speed.Y + Physics.gravConst);
                }
            }
            movable.PrevPosition = movable.CurPosition;
            movable.CurPosition += movable.Speed;
        }

    }
}
