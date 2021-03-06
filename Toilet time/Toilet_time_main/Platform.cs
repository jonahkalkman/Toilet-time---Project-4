﻿using System;

namespace Toilet_time_main
{
    public class Platform : Stable_Object
    {
        public Platform(int x_pos, int y_pos, int x_size, int y_size)
            : base(new Position(x_pos, y_pos), new Size(x_size, y_size), true)
        {

        }

        public override void Update(float dt, Gui_Manager guimanager)
        {

        }

        public override void Draw(iDrawVisitor visitor)
        {
            visitor.DrawPlatform(this);
        }
    }
}