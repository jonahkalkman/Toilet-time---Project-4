﻿using System;

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
        public Size size;
        public Position position;
        public bool resizeable;
        public bool IsMainCharacter;
        public bool MoveOnWalk;

        public iObject(Position position, Size size, bool resizeable)
        {
            this.size = size;
            this.position = position;
            this.resizeable = resizeable;
            this.IsMainCharacter = false;
            this.MoveOnWalk = true;
        }

        public abstract void Draw(DrawVisitor visitor);

        public abstract void Update(float dt, Gui_Manager guimanager);

        public void Move(float dt, Gui_Manager guimanager, WalkDirectionInput WalkDirectionInput)
        {
            if (this.MoveOnWalk)
            {
                switch (WalkDirectionInput)
                {
                    case (WalkDirectionInput.Left):
                        {
                            this.position.x = this.position.x + 1;
                            break;
                        }
                    case (WalkDirectionInput.Right):
                        {
                            this.position.x = this.position.x - 1;
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
            velocity = velocity - (4 * dt);
           
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

        public void Jump(Gui_Manager guimanager)
        {
            if (IsMainCharacter)
            {

                if (this.velocity == 0 && (guimanager.Check_Collision(this, position.x, position.y - 1, size.x, size.y)) == true)
                {
                    this.velocity = 4;
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