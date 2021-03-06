﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Toilet_time_main
{
    public enum Systemtype { windows, android};

    public class Gui_Manager
    {
        bool VieuwDebugConsole = false; //true when game needs to show debugstats

        public bool autopickup;

        public bool exit = false;

        public Systemtype systemtype;

        public Iterator<Fallable_Object> Fallable_Objects;
        public Iterator<Stable_Object> Stable_Objects;
        public Iterator<iObject> Gui_stuff;
        public Iterator<iObject> Interacting_Objects;

        public Screen Current_screen;
        public int screen;

        public Factory_screen screenFactory;
        public iDrawVisitor Drawvisitor;
        public SoundHandler sound_handler;
        public Input_Adapter inputadapter;
        public Point Cursor;
        public InputData LatestInput;

        public float drawdt;
        public float updatedt;

        public int inputmechanism;
        public bool Gamepadonline = false;

        public int CharacterSpeed; 
        public float localwalkspeed = 0;

        public float buttoncooldown = 0;
        public float pickupcooldown = 0;
        public float Controls_Cooldown = 0;

        public int movementchange = 0;
   
        public float End_Of_Level_Cooldown = 0;
        public bool End_Of_Level = false;

        public bool paused;
        public bool Lifes_enabled;

        public int lowestyvalue = 800;
        public int lifes;

        public BackgroundType Background;

        InputData input;

        public Gui_Manager(iDrawVisitor drawvisitor, SoundHandler sound_handler, Input_Adapter inputadapter, Systemtype systemtype)
        {
            this.systemtype = systemtype;

            this.Drawvisitor = drawvisitor;
            this.CharacterSpeed = 300;                      //speed of the character (pixels / sec)
            this.inputmechanism = 1;                        //inputmechanism: given to inputadapter
            this.screenFactory = new Factory_screen(this);
            this.inputadapter = inputadapter;
            this.lifes = 3;                                 //number of lifes
            this.screen = 1;                                //first screen to load
            this.Cursor = new Point(0,0);
            this.sound_handler = sound_handler;
            Create_screen(screen);
            Controls_Cooldown = 0;
            sound_handler.PlayBackground(ChooseBackGroundMusic.menu);
            this.paused = false;

            if (systemtype == Systemtype.android)
            {
                autopickup = true;
            }
            else
            {
                autopickup = true; // set to false to use E to pickup
            } 
       }


        public bool Check_Collision(iObject Object, int x_pos, int y_pos, int x_size, int y_size) //Checks if the fictional stats are interfering with a stable object: returns false when it interferes and true when not;
        {
            Stable_Objects.Reset();
            bool returnbool = true;
            Stable_Objects.Reset(); // reset iterator
            while (Stable_Objects.GetNext().Visit(() => false, unusedvalue => true)) //while it returns some<item>
            {
                // checking gravity: falling and other
                if (Stable_Objects.GetCurrent().Visit(

                                                            () => { return false; }

                                                            ,


                                                            item =>
                                                                {
                                                                    bool returnstatement = false;

                                                                   if (         x_pos < item.position.x + item.size.x      &&      x_pos + x_size > item.position.x)
                                                                    {
                                                                        if (    y_pos + y_size> item.position.y            &&      y_pos < item.position.y + item.size.y)
                                                                        {
                                                                            return true;
                                                                        }

                                                                    }

                                                                    return returnstatement;

                                                                }
                                                            ) // get returnstatement of it interfering
                    )
                {
                    returnbool = false;
                }
                   
            }

            return returnbool;
        }

        

        public Fallable_Object GetMain_Character() //returns maincharacter as fallable object (fallable_object.IsMain = true)
        {
            {
                Fallable_Objects.Reset();
                while (Fallable_Objects.GetNext().Visit(() => false, _ => true))
                {
                    if (Fallable_Objects.GetCurrent().Visit<bool>(() => false, item => item.IsMainCharacter))
                    {
                        return (Fallable_Objects.GetCurrent().Visit<Fallable_Object>(() => null, item => { return item; }));
                    }
                }
                return null;
            }
        }

        public void Create_screen(int screen_to_load) // creates screen from number in parameter
        {
            End_Of_Level_Cooldown = 0;
            End_Of_Level = false;

            Reset_screen(); // resets screen data

            screen = screen_to_load;
            Current_screen = screenFactory.Create_screen(screen); // gets new data for next screen


            //adds data next screen to guimanager
            Background = Current_screen.Background;                    
            this.Fallable_Objects = Current_screen.Fallable_Objects;
            this.Stable_Objects = Current_screen.Stable_Objects;
            this.Gui_stuff = Current_screen.gui_stuff;
            this.Interacting_Objects = Current_screen.Interacting_Objects;

            //Plays the corresponding sounds
            if (Current_screen.islevel == true)
            {
                sound_handler.PlayBackground(ChooseBackGroundMusic.game_cry);
                Controls_Cooldown = 0.8f;
            }
            else
            {
                sound_handler.PlayBackground(ChooseBackGroundMusic.menu);
            }
        }

        public void Getinputmechanism(int inputnumber) // Sets inputmechanism
        {
            this.inputmechanism = inputnumber;
        }

        private void Reset_screen() // resets all screen data
        {
            this.movementchange = 0;
            this.Fallable_Objects = null;
            this.Stable_Objects = null;
            this.Gui_stuff = null;
            this.Interacting_Objects = null;
        }

        public void Reload_screen() // reload the current screen
        {
            Create_screen(screen);
        }

        public void DrawDebugConsole() // draw the debug console
        {
            Drawvisitor.DrawDebugConsole(this);
        }

        public void Draw(float dt) // global draw function: draws all objects in the screen data
        {
            drawdt = dt;

            Drawvisitor.spriteBatch.Begin(); // spritebatch begin: for monogame -> when using multiple draw platforms: move to drawmanager

            
            //background
            Drawvisitor.DrawBackground(Background); // draw the background
            


            //debugconsole
            if (VieuwDebugConsole) // draw the debug screen when necessary
            {
                DrawDebugConsole(); 
            }

            //drawing all objects out of the iterators: reset -> getnext -> getcurrent

            Stable_Objects.Reset();
            while (Stable_Objects.GetNext().Visit(() => false, unusedvalue => true))
            {
                Stable_Objects.GetCurrent().Visit(() => { }, item => { item.Draw(Drawvisitor); });
            }

            Fallable_Objects.Reset();
            while (Fallable_Objects.GetNext().Visit(() => false, unusedvalue => true))
            {
                Fallable_Objects.GetCurrent().Visit(() => { }, item => { item.Draw(Drawvisitor); });
            }

            Gui_stuff.Reset();
            while (Gui_stuff.GetNext().Visit(() => false, unusedvalue => true))
            {
                Gui_stuff.GetCurrent().Visit(() => { }, item => { item.Draw(Drawvisitor); });
            }

            Interacting_Objects.Reset();
            while (Interacting_Objects.GetNext().Visit(() => false, unusedvalue => true))
            {
                Interacting_Objects.GetCurrent().Visit(() => { }, item => { item.Draw(Drawvisitor); });
            }

            //drawcursor
            Drawvisitor.DrawCursor(Cursor); // draw the cursor on current position

            Drawvisitor.spriteBatch.End(); // spritebatch.end -> move to drawmanager when using multiple draw platforms
        }

        // Update
        // CheckIfTouching
        public List<iObject> CheckIfMainTouching() // returns a list of iteracting objects touching the main character
        {
            List<iObject> returnlist = new List<iObject>();
            Fallable_Object main = GetMain_Character();
            if (main != null)
            {

            

                Interacting_Objects.Reset();

                while (Interacting_Objects.GetNext().Visit(() => false, unusedvalue => true))
                {
                    // checking gravity: falling and other
                    if (Interacting_Objects.GetCurrent().Visit<bool>(

                                                                () => { return false; }
                                                                
                                                                ,


                                                                item =>
                                                                {

                                                                    if (main.position.x < item.position.x + item.size.x && main.position.x + main.size.x > item.position.x)
                                                                    {
                                                                        if (main.position.y + main.size.y > item.position.y && main.position.y < item.position.y + item.size.y)
                                                                        {
                                                                           return true;
                                                                        }
                                                                    }

                                                                    return false;
                                                                }
                                                                
                                                                )
                        )
                    {
                        returnlist.Add(Interacting_Objects.GetCurrent().Visit<iObject>(() => { throw new Exception("interaction error"); }, item => { return item; }));
                    } 
                }

            }

            return returnlist;
        }

        public void Main_Dead() // handles the death of the main character: lifes_enabled is if the life system is active or not (easy vs hard mode)
        {
            if (!End_Of_Level)
            {
                if (Lifes_enabled)
                {
                    lifes -= 1;
                }
                if (lifes >= 0)
                {
                    Reload_screen();
                }
                else
                {
                    Create_screen(1); //ondead -> go to main menu: past ondead screen here
                    this.lifes = 3; // resets lifes for next try
                }
            }
        }
        // small walk check
        public bool CheckIfMove(float dt, WalkDirectionInput way, int walkspeed) // returns a bool if the maincharacter can walk in a certain direction
        {
            Fallable_Object main = GetMain_Character() ;
            bool moveable = false;
            int multiplyer = 1;
            if (main != null)
            {
                if (way == WalkDirectionInput.Right) { multiplyer = 1; }
                if (way == WalkDirectionInput.Left) { multiplyer = -1; }

                moveable = Check_Collision(main, main.position.x + walkspeed * multiplyer, main.position.y, main.size.x, main.size.y);
            }
            return moveable;
        }
        //


        public void Update(float dt) // global update function of the application
        {
            updatedt = dt;
            bool controllsenabled = true; // able to walk?

            //manages cooldowns
            if (pickupcooldown > 0) { pickupcooldown -= dt; }
            if (buttoncooldown > 0) { buttoncooldown -= dt;}
            if (Controls_Cooldown > 0) { Controls_Cooldown -= dt;  }
            if (End_Of_Level_Cooldown > 0) { End_Of_Level_Cooldown -= dt ; }

            if (Controls_Cooldown > 0) { controllsenabled = false; }; 

            input = inputadapter.GetInput(inputmechanism); //gets input from inputadapter from current input mechanism

            LatestInput = input;
            // Check if gamepad is enabled


            bool newgamepadonline = input.GamePadOnline; // manages the gamepadonline option: reloads screen when becomes active when in option menu

            if (Gamepadonline != newgamepadonline && screen == 2)
            {
                Gamepadonline = newgamepadonline;
                Reload_screen();
            }
            Gamepadonline = input.GamePadOnline;

            //kill on fall


            Fallable_Object main = GetMain_Character(); // checks if main falls down the screen -> Main_dead();
            if (main != null)
            {
                if (main.position.y + main.size.y > lowestyvalue)
                {
                    Main_Dead();
                }
            }
                // checks if baby falls down the screen -> calls Miain_dead();
            Interacting_Objects.Reset();
            while (Interacting_Objects.GetNext().Visit(() => false, _ => true))
            {
                if (Interacting_Objects.GetCurrent().Visit(() => false, item => { return item.IsBaby; }))
                {
                    iObject baby = Interacting_Objects.GetCurrent().Visit<iObject>(() => throw new Exception("failed getting interaction"), act => { return act; });
                    if (baby.position.y + baby.size.y > lowestyvalue)
                    {
                        Main_Dead();
                    }
                }
            }

            bool walk = false;
            bool CanMove = false;
            int walkspeed = 0;

            localwalkspeed += CharacterSpeed * dt; //calculates walkspeed: float -> int: don't lose data!!
            for (int i = (int)(localwalkspeed); i > 0; i--)
            {
                localwalkspeed = localwalkspeed - 1.0f;
                walkspeed++;
            }

            // cursor
            this.Cursor = input.cursor; // reloads cursor position form input

            if (paused == false) // if game not paused then update the rest
            {

                // walk
                // manages walk on walk input
                WalkDirectionInput walkdirection = WalkDirectionInput.Right;

                if (input.Walk.Visit(() => false, _ => true))
                {
                    walk = true;
                    walkdirection = input.Walk.Visit<WalkDirectionInput>(() => { throw new Exception("walkdirection failed"); }, item => { return item; });

                    CanMove = CheckIfMove(dt, walkdirection, walkspeed); // Checks if allowed to move character
                    if (CanMove == false && walkspeed > 1)
                    {
                        for (int i = walkspeed; i > 0 && CanMove == false; i--)
                        {
                            CanMove = CheckIfMove(dt, walkdirection, i); // Checks if allowed to move character
                            if (CanMove == true)
                            {
                                walkspeed = i;
                                    
                            }
                        }
                    }
                }


                // handles jump input: if possible :)
                if (controllsenabled == true)
                {
                    if (input.MoveAction.Visit(() => false, _ => true))
                    {
                        if (input.MoveAction.Visit<CharacterMovementAction>(() => { throw new Exception("Charactermovement failed"); }, item => { return item; }) == CharacterMovementAction.Jump)
                        {
                            if (main != null)
                            {
                                if (main.IsMainCharacter == true)
                                {
                                    main.Jump(this);
                                }
                            }
                        }
                    }
                }

                //handles movement if left/right
                if (controllsenabled == true && walk == true && CanMove == true)
                {
                    if (walkdirection == WalkDirectionInput.Left)
                    {
                        movementchange -= walkspeed;
                    }
                    else if (walkdirection == WalkDirectionInput.Right)
                    {
                        movementchange += walkspeed;
                    }
                }

                // updates stableobjects + make them move if character has to move
                Stable_Objects.Reset();
                while (Stable_Objects.GetNext().Visit(() => false, _ => true))
                {
                    Stable_Objects.GetCurrent().Visit(() => { }, item => { item.Update(dt, this); });

                    if (controllsenabled == true)
                    {
                        if (controllsenabled == true && walk == true && CanMove == true)
                        {
                            Stable_Objects.GetCurrent().Visit(() => { }, item => { item.Move(dt, this, walkdirection, walkspeed); });
                        }
                    }
                }
                // updates fallable objects + make them move if character has to move
                Fallable_Objects.Reset();
                while (Fallable_Objects.GetNext().Visit(() => false, _ => true))
                {
                    Fallable_Objects.GetCurrent().Visit(() => { }, item => { item.Update(dt, this); });

                    if (controllsenabled == true)
                    {
                        if (controllsenabled == true && walk == true && CanMove == true)
                        {
                            Fallable_Objects.GetCurrent().Visit(() => { }, item => { item.Move(dt, this, walkdirection, walkspeed); });
                        }
                    }
                }


                // updates interaction objects + make them move if character has to move
                Interacting_Objects.Reset();
                while (Interacting_Objects.GetNext().Visit(() => false, unusedvalue => true))
                {
                    Interacting_Objects.GetCurrent().Visit(() => { }, item => { item.Update(dt, this); });

                    if (controllsenabled == true)
                    {
                        if (walk == true && CanMove == true)
                        {
                            Interacting_Objects.GetCurrent().Visit(() => { }, item => { item.Move(dt, this, walkdirection, walkspeed); });
                        }
                    }
                }

                // Checking interaction with main character by interacting objects

                List<iObject> interacton = CheckIfMainTouching();

                // checking for baby: picking up when autopickup is on

                if (autopickup == false)
                {
                    if (input.CharacterActivity.Visit(() => false, _ => true))
                    {
                        CharacterActivity activityinput = input.CharacterActivity.Visit<CharacterActivity>(() => throw new Exception("failed getting interaction"), act => { return act; });
                        if (controllsenabled == true)
                        {
                            if (activityinput == CharacterActivity.Action && pickupcooldown <= 0)
                            {
                                pickupcooldown = 0.5f;
                                if (main != null)
                                {
                                    if (main.HasBaby == false)
                                    {

                                        interacton.Reset();
                                        while (interacton.GetNext().Visit(() => false, unusedvalue => true))
                                        {
                                            if (interacton.GetCurrent().Visit(() => false, item => { return item.IsBaby; }))
                                            {
                                                // on babypickup...
                                                iObject baby = interacton.GetCurrent().Visit<iObject>(() => throw new Exception("failed getting interaction"), act => { return act; });
                                                main.HasBaby = true;
                                                baby.Visible = false;
                                                sound_handler.PlayBackground(ChooseBackGroundMusic.game_noncry);
                                                sound_handler.PlaySoundEffect(ChooseSoundEffect.baby_laugh);
                                            }
                                        }
                                    }


                                    else
                                    {
                                        // on babydrop...
                                        Interacting_Objects.Reset();
                                        while (Interacting_Objects.GetNext().Visit(() => false, unusedvalue => true))
                                        {
                                            if (Interacting_Objects.GetCurrent().Visit(() => false, item => { return item.IsBaby; }))
                                            {
                                                iObject baby = Interacting_Objects.GetCurrent().Visit<iObject>(() => throw new Exception("failed getting interaction"), act => { return act; });
                                                main.HasBaby = false;
                                                baby.position = new Position(main.position.x, main.position.y + 20);
                                                baby.Visible = true;
                                                sound_handler.PlayBackground(ChooseBackGroundMusic.game_cry);

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                } 

                // checking if touching items


                interacton.Reset();
                while (interacton.GetNext().Visit(() => false, unusedvalue => true))
                {
                    // killing main character when touching object is deadly
                    iObject TouchingObject = interacton.GetCurrent().Visit<iObject>(() => throw new Exception("failed getting interaction"), act => { return act; });

                    if (TouchingObject.IsDeadly)
                    {
                        Main_Dead();
                    }

                    // picking baby up when autopickup is on
                    if (TouchingObject.IsBaby)
                    {
                        if (autopickup == true)
                        {
                            if (main.HasBaby == false)
                            {
                                main.HasBaby = true;
                                TouchingObject.Visible = false;
                                sound_handler.PlayBackground(ChooseBackGroundMusic.game_noncry);
                                sound_handler.PlaySoundEffect(ChooseSoundEffect.baby_laugh);
                            }

                        }
                    }

                    // when the main character reaches the end
                    if (TouchingObject.IsEnd)
                    {
                        //then the main character is carying the baby...
                        if (main.HasBaby)
                        {
                            //on levelend
                            main.HasBaby = false;
                            TouchingObject.HasBaby = true;
                            this.End_Of_Level_Cooldown = 3;
                            this.Controls_Cooldown = 3;
                            this.End_Of_Level = true;
                            sound_handler.PlaySoundEffect(ChooseSoundEffect.game_end);
                        }
                    }
                }
            }
                // update our gui_stuff: buttons and labels
            Gui_stuff.Reset();
            while (Gui_stuff.GetNext().Visit(() => false, unusedvalue => true))
            {
                Gui_stuff.GetCurrent().Visit(() => { }, item => { item.Update(dt, this); });
            }

            // when end animation is over: load next level
            if ( End_Of_Level_Cooldown <= 0 && End_Of_Level == true )
            {
                End_Of_Level_Cooldown = 0;
                End_Of_Level = false;
                Controls_Cooldown = 0;
                Create_screen(main.nextscreen);
            }
        }
    }
}
 