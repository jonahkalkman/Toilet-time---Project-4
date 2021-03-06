﻿using System;

namespace Toilet_time_main
{
    public class Screen // returned by factory with all screen data
    {
        public BackgroundType Background;
        public Iterator<Fallable_Object> Fallable_Objects;
        public Iterator<Stable_Object> Stable_Objects;
        public Iterator<iObject> gui_stuff;
        public Iterator<iObject> Interacting_Objects;
        public bool islevel;

        public Screen(BackgroundType Background, Iterator<Fallable_Object> fallable_objects, Iterator<Stable_Object> stable_objects, Iterator<iObject> gui_stuff, Iterator<iObject> Interacting_Objects, bool islevel)
        {
            this.Background = Background;
            this.Fallable_Objects = fallable_objects;
            this.Stable_Objects = stable_objects;
            this.gui_stuff = gui_stuff;
            this.Interacting_Objects = Interacting_Objects;
            this.islevel = islevel;
        }

    }

}