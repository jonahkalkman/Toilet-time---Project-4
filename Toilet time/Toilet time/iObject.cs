﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Toilet_time
{
    public interface Drawable
    {
        void Draw(DrawVisitor visitor);
    }

    public interface Updateable
    {
        void Update(float dt, Gui_Manager guimanager);
    }

    public abstract class iObject : Drawable, Updateable
    {
        public Color color = Color.White;
        public Size size;
        public Position position;
        public bool resizeable;
        public bool IsMainCharacter = false;
        public bool IsBaby = false;
        public bool IsEnd = false;
        public bool MoveOnWalk = true;
        public bool HasBaby = false;
        public bool Visible = true;
        public bool IsDeadly = false;
        public int nextscreen;

        public iObject(Position position, Size size, bool resizeable)
        {
            this.size = size;
            this.position = position;
            this.resizeable = resizeable;
        }

        public abstract void Draw(DrawVisitor visitor);

        public abstract void Update(float dt, Gui_Manager guimanager);

        public void Move(float dt, Gui_Manager guimanager, WalkDirectionInput WalkDirectionInput, int speed)
        {
            if (this.MoveOnWalk)
            {
                switch (WalkDirectionInput)
                {
                    case (WalkDirectionInput.Right):
                        {
                            this.position.x = this.position.x - speed;
                            break;
                        }
                    case (WalkDirectionInput.Left):
                        {
                            this.position.x = this.position.x + speed;
                            break;
                        }
                }
            }
        }
    }

    public abstract class Fallable_Object : iObject
    {

        public Fallable_Object(Position position, Size size, bool resizeable)
            : base(position, size, resizeable)
        {

        }

        public float velocity = 0;
        public void Update_Gravity(float dt, Gui_Manager guimanager)
        {
            float startvelocity = velocity;
            velocity = velocity - (15 * dt);
           
            if (startvelocity > 0.6 && velocity < 0.6)
            {
                velocity = -1.0f; // boost for when in midair
            }


            if (guimanager.Check_Collision(this, position.x, position.y - (int)velocity, size.x, size.y)) // check if able to fall
            {
                this.position.y = this.position.y - (int)velocity;
            }
            else
            {
                bool minimalfound = true;
                for (int i = (int)velocity; i < 0 && minimalfound; i++)
                {
                    if (guimanager.Check_Collision(this, position.x, position.y - i, size.x, size.y))
                    {
                        minimalfound = false;
                        this.position.y = this.position.y - i;
                    }
                }

                velocity = 0;

                if (guimanager.Check_Collision(this, position.x, position.y + 1, size.x, size.y) == true) // for when bounching on platform
                {
                    velocity = -1;
                }
            }
        }

        public int jumpvelocity = 0;
        public void Jump(Gui_Manager guimanager)
        {
            if (IsMainCharacter)
            {

                if (this.velocity == 0 && (guimanager.Check_Collision(this, position.x, position.y - 1, size.x, size.y)) == true)
                {
                    this.velocity = jumpvelocity;
                }
            }
        }

        public override void Update(float dt, Gui_Manager guimanager)
        {
            Update_Gravity(dt, guimanager);

        }
    }

    public abstract class Stable_Object : iObject
    {
        public Stable_Object(Position position, Size size, bool resizeable)
            : base(position, size, resizeable)
        {

        }
    }
}