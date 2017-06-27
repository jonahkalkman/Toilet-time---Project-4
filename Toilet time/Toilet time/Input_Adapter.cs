﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// TO DO: - Add another way of inserting input
// TO DO: - Make a switch to change between input methods
// TO DO: - Make input by buttons possible - add new list in gui_manager and make it give it to inputadapter every update

namespace Toilet_time
{

    public class Input_Adapter
    {
        public Input_Adapter()
        {

        }

        public InputData GetInput(int type)
        {            
            switch (type)
            {
                case 1:
                    return checkKeyboardpijltjes();
                    //break;
                case 2:
                    return checkKeyboardaswd();
                    //break;
                default:
                    return checkKeyboardpijltjes();
                    //break;
                
            }
            
        }



        public InputData checkKeyboardpijltjes()
        {
            iOption<CharacterMovementAction> MoveAction = new None<CharacterMovementAction>();
            iOption<WalkDirectionInput> WalkDirection = new None<WalkDirectionInput>();
            iOption<CharacterActivity> CharacterActivity = new None<CharacterActivity>();
            iOption<SettingsInput> Settings = new None<SettingsInput>();
            iOption<MousePressed> MouseAction = new None<MousePressed>();
            Point cursor = new Point(-1, -1);

            KeyboardState keyboard_state = Keyboard.GetState();

            var mouse_state = Mouse.GetState();
            cursor = new Point(mouse_state.X, mouse_state.Y);
            if (mouse_state.LeftButton == ButtonState.Pressed)
            {
                MouseAction = new Some<MousePressed>(MousePressed.Left_Button);
            }

            // needs to be build out!

            if (keyboard_state.IsKeyDown(Keys.Up))
            {
                MoveAction = new Some<CharacterMovementAction>(CharacterMovementAction.Jump);
            }

            if (keyboard_state.IsKeyDown(Keys.Left))
            {
                WalkDirection = new Some<WalkDirectionInput>(WalkDirectionInput.Left);
            }

            if (keyboard_state.IsKeyDown(Keys.Right))
            {
                WalkDirection = new Some<WalkDirectionInput>(WalkDirectionInput.Right);
            }

            if (keyboard_state.IsKeyDown(Keys.Right) && keyboard_state.IsKeyDown(Keys.Left))
            {
                WalkDirection = new None<WalkDirectionInput>();
            }

            if (keyboard_state.IsKeyDown(Keys.E))
            {
                CharacterActivity = new Some<CharacterActivity>(Toilet_time.CharacterActivity.Action);
            }

            return new InputData(MoveAction, WalkDirection, CharacterActivity, Settings, MouseAction, cursor);
            //return new Input();
        }


        public InputData checkKeyboardaswd()
        {
            iOption<CharacterMovementAction> MoveAction = new None<CharacterMovementAction>();
            iOption<WalkDirectionInput> WalkDirection = new None<WalkDirectionInput>();
            iOption<CharacterActivity> CharacterActivity = new None<CharacterActivity>();
            iOption<SettingsInput> Settings = new None<SettingsInput>();
            iOption<MousePressed> MouseAction = new None<MousePressed>();
            Point cursor = new Point(-1, -1);

            KeyboardState keyboard_state = Keyboard.GetState();

            var mouse_state = Mouse.GetState();
            cursor = new Point(mouse_state.X, mouse_state.Y);

            if (mouse_state.LeftButton == ButtonState.Pressed)
            {
                MouseAction = new Some<MousePressed>(MousePressed.Left_Button);
            }

            // needs to be build out!

            if (keyboard_state.IsKeyDown(Keys.Up))
            {
                MoveAction = new Some<CharacterMovementAction>(CharacterMovementAction.Jump);
            }


            if (keyboard_state.IsKeyDown(Keys.W))
        {
            MoveAction = new Some<CharacterMovementAction>(CharacterMovementAction.Jump);
        }

        if (keyboard_state.IsKeyDown(Keys.A))
        {
            WalkDirection = new Some<WalkDirectionInput>(WalkDirectionInput.Left);
        }

        if (keyboard_state.IsKeyDown(Keys.D))
        {
            WalkDirection = new Some<WalkDirectionInput>(WalkDirectionInput.Right);
        }

        if (keyboard_state.IsKeyDown(Keys.A) && keyboard_state.IsKeyDown(Keys.D))
        {
            WalkDirection = new None<WalkDirectionInput>();
        }

        if (keyboard_state.IsKeyDown(Keys.E))
        {
            CharacterActivity = new Some<CharacterActivity>(Toilet_time.CharacterActivity.Action);
        }
        return new InputData(MoveAction, WalkDirection, CharacterActivity, Settings, MouseAction, cursor);
        //return new Input();
        }
    }  
}