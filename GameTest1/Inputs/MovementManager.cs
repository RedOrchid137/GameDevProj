﻿using GameTest1.Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameTest1.Inputs
{
    public class MovementManager
    {
        public static void Move(GameObject movable)
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
                if(movable.Position.Y>World.FloorHeight)
                {
                    movable.Speed = new Vector2(movable.Speed.X, 0);
                    (movable as Character).JumpFlag = false;
                    movable.Position = new Vector2(movable.Position.X, World.FloorHeight);
                }
                else if(movable.Position.Y < World.FloorHeight)
                {
                    movable.Speed = new Vector2(movable.Speed.X, movable.Speed.Y + Physics.gravConst);
                }
            }
            movable.Position += movable.Speed;
            HandleCollisions(movable);
        }
        public static void HandleCollisions (GameObject movable)
        {
            //TODO Verder Uitwerken
            if (movable.Position.X < 0)
            {
                movable.Position = new Vector2(movable.Window.Width, movable.Position.Y);
            }
            if (movable.Position.X > movable.Window.Width)
            {
                movable.Position = new Vector2(0, movable.Position.Y);
            }
        }

    }
}