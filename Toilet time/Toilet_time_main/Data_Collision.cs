﻿using System;

namespace Toilet_time_main
{
    public class Collision // unused way for returning collision data
    {
        public iOption<iObject> UpObject;
        public iOption<iObject> DownObject;
        public iOption<iObject> LeftObject;
        public iOption<iObject> RightObject;
        public Collision(iOption<iObject> UpObject, iOption<iObject> DownObject, iOption<iObject> LeftObject, iOption<iObject> RightObject)
        {
            this.UpObject = UpObject;
            this.DownObject = DownObject;
            this.LeftObject = LeftObject;
            this.RightObject = RightObject;
        }
    }
}