﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Toilet_time_main;

namespace Toilet_time_Windows
{
    public class DrawVisitor : Toilet_time_main.iDrawVisitor
    {
        public SpriteBatch spritebatch;
        public SpriteFont arial;
        public GraphicsDeviceManager graphics;
        public Texture2D Texture_White_Pixel;
        public Texture2D Texture_Platform;
        public Texture2D Texture_Main_Char;
        public Texture2D Texture_Main_Char_with_Baby;
        public Texture2D Texture_Baby;
        public Texture2D Texture_Toilet;
        public Texture2D Texture_Toilet_With_Baby;
        public Texture2D Texture_Deadly_Bricks;
        public Texture2D Texture_Toilet_Paper;
        public Texture2D Texture_Mouse;
        float screenmultiplier;
        public Texture2D Texture_Ingame_Background;
        public Texture2D Texture_Menu_Background;
        public Texture2D Texture_Spikes;
        public Texture2D Texture_Enemy_Grandma;
        public Texture2D Texture_Heart;
        public Texture2D Texture_Spider;

        public int CurrentHeight;
        public int CurrentWidth;

        public DrawVisitor(int CurrentWidth, int CurrentHeight, float screenmultiplier, SpriteBatch spriteBatch, GraphicsDeviceManager graphics, SpriteFont arial, Texture2D Texture_White_Pixel, Texture2D Texture_Platform, Texture2D Texture_Main_Char, Texture2D Texture_Main_Char_with_Baby, Texture2D Texture_Baby, Texture2D Texture_Toilet, Texture2D Texture_Toilet_With_Baby, Texture2D Texture_Deadly_Bricks, Texture2D Texture_Toilet_Paper, Texture2D Texture_Spikes, Texture2D Texture_Background_Wood, Texture2D Texture_Menu_Background, Texture2D Texture_Mouse, Texture2D Texture_Enemy_Grandma, Texture2D Texture_Heart, Texture2D Texture_Spider)
        {
            this.graphics = graphics;


            this.spritebatch = spriteBatch;
            this.Texture_White_Pixel = Texture_White_Pixel;
            this.Texture_Platform = Texture_Platform;
            this.Texture_Main_Char = Texture_Main_Char;
            this.Texture_Main_Char_with_Baby = Texture_Main_Char_with_Baby;
            this.Texture_Baby = Texture_Baby;
            this.Texture_Toilet = Texture_Toilet;
            this.Texture_Toilet_With_Baby = Texture_Toilet_With_Baby;
            this.Texture_Deadly_Bricks = Texture_Deadly_Bricks;
            this.Texture_Toilet_Paper = Texture_Toilet_Paper;
            this.Texture_Ingame_Background = Texture_Background_Wood;
            this.Texture_Mouse = Texture_Mouse;
            this.Texture_Enemy_Grandma = Texture_Enemy_Grandma;
            this.arial = arial;
            this.Texture_Spikes = Texture_Spikes;
            this.CurrentHeight = CurrentHeight;
            this.CurrentWidth = CurrentWidth;
            this.screenmultiplier = screenmultiplier;
            this.Texture_Menu_Background = Texture_Menu_Background;
            this.Texture_Spider = Texture_Spider;
            this.Texture_Heart = Texture_Heart;
        }

        public SpriteBatch spriteBatch
        {
            get { return spritebatch; }
        }

        public void DrawCursor(Toilet_time_main.Point mousepoint)
        {
            spritebatch.Draw(Texture_Mouse, ConvertRectangle(new Rectangle(mousepoint.x - 3, mousepoint.y - 3, 25, 25)), Color.White);
        }

        public void DrawCharacter(Toilet_time_main.Main_Character character)
        {
            if (character.HasBaby)
            {
                spritebatch.Draw(Texture_Main_Char_with_Baby, ConvertRectangle((new Rectangle(character.position.x, character.position.y, character.size.x, character.size.y))), character.color);
            }
            else
            {
                spritebatch.Draw(Texture_Main_Char, ConvertRectangle(new Rectangle(character.position.x, character.position.y, character.size.x, character.size.y)), character.color);
            }
        }



        public void DrawBaby(Toilet_time_main.Baby baby)
        {
            if (baby.Visible == true)
            {
                spritebatch.Draw(Texture_Baby, ConvertRectangle(new Rectangle(baby.position.x, baby.position.y, baby.size.x, baby.size.y)), baby.color);
            }
        }

        public void DrawEnemyGrandma(Enemy_Grandma enemy_grandma)
        {
            spritebatch.Draw(Texture_Enemy_Grandma, ConvertRectangle(new Rectangle(enemy_grandma.position.x, enemy_grandma.position.y, enemy_grandma.size.x, enemy_grandma.size.y)), enemy_grandma.color);
        }


        public void DrawPlatform(Toilet_time_main.Platform platform)
        {
            spritebatch.Draw(Texture_Platform, ConvertRectangle(new Rectangle(platform.position.x, platform.position.y, platform.size.x, platform.size.y)), platform.color);
        }

        public void DrawSpawn(Toilet_time_main.Spawn spawn)
        {

        }

        public void DrawEnd(Toilet_time_main.End_Goal end)
        {
            if (end.HasBaby == false)
            {
                spritebatch.Draw(Texture_Toilet, ConvertRectangle(new Rectangle(end.position.x, end.position.y, end.size.x, end.size.y)), end.color);
            }
            else if (end.HasBaby == true)
            {
                spritebatch.Draw(Texture_Toilet_With_Baby, ConvertRectangle(new Rectangle(end.position.x, end.position.y, end.size.x, end.size.y)), end.color);
            }
        }

        public void DrawDeadlyBrick(Toilet_time_main.Deadly_Brick brick)
        {
            spritebatch.Draw(Texture_Deadly_Bricks, ConvertRectangle(new Rectangle(brick.position.x, brick.position.y, brick.size.x, brick.size.y)), brick.color);
        }

        public void DrawButton(Toilet_time_main.Button button)
        {
            spritebatch.Draw(Texture_White_Pixel, ConvertRectangle(new Rectangle((int)button.position.x, (int)button.position.y, (int)button.size.x, (int)button.size.y)), button.color);
            if (button.label != null)
            {
                this.DrawLabel(button.label);
            }

        }

        public void DrawLabel(Toilet_time_main.Label label)
        {
            Vector2 textsize = arial.MeasureString(label.text);
            int textsize_x = (int)textsize.X;
            int textsize_y = (int)textsize.Y;
            spritebatch.DrawString(arial, label.text, ConvertVector2(new Vector2(label.position.x + ((label.size.x - textsize_x / screenmultiplier) / 2), label.position.y + ((label.size.y - textsize_y / screenmultiplier) / 2))), label.color);
        }

        public void DrawToiletPaper(Toilet_time_main.Toilet_Paper toilet_paper)
        {
            spritebatch.Draw(Texture_Toilet_Paper, ConvertRectangle(new Rectangle(toilet_paper.position.x, toilet_paper.position.y, toilet_paper.size.x, toilet_paper.size.y)), toilet_paper.color);
        }

        public void DrawSpider(Spider spider)
        {
            spritebatch.Draw(Texture_Spider, ConvertRectangle(new Rectangle(spider.position.x, spider.position.y, spider.size.x, spider.size.y)), spider.color);
        }

        enum Dimensions { x, y }
        private int ConvertToSize(Dimensions dimension, float percent, int extra)
        {
            int returntype = 0;
            switch (dimension)
            {
                case Dimensions.x:
                    {
                        returntype = (int)(CurrentWidth * percent) + extra;
                        break;
                    }
                case Dimensions.y:
                    {
                        returntype = (int)(CurrentHeight * percent) + extra;
                        break;
                    }
            }

            return returntype;
        }

        Toilet_time_main.Label MouseInformation = new Toilet_time_main.Label(600, 0, 100, 20, "");
        Toilet_time_main.Label InputInformation = new Toilet_time_main.Label(600, 20, 100, 20, "");
        Toilet_time_main.Label CooldownInformation = new Toilet_time_main.Label(600, 40, 100, 20, "");
        Toilet_time_main.Label MainInformation = new Toilet_time_main.Label(600, 60, 100, 20, "No main_character found");
        Toilet_time_main.Label ScreenStats = new Toilet_time_main.Label(600, 80, 100, 20, "");
        Toilet_time_main.Label LevelStats = new Toilet_time_main.Label(600, 100, 100, 20, "");
        Toilet_time_main.Label PerformanceInformation = new Toilet_time_main.Label(600, 120, 100, 20, "");


        public void DrawDebugConsole(Toilet_time_main.Gui_Manager guimanager)
        {
            MouseInformation.text = "Mouse: | " + guimanager.Cursor.x.ToString() + "," + guimanager.Cursor.y.ToString() + " | " + guimanager.LatestInput.MouseButton.Visit<string>(() => { return ""; }, item => { return "" + item.ToString(); }) + " |";
            MouseInformation.Draw(this);

            InputInformation.text = "Input: | " + guimanager.LatestInput.Walk.Visit<string>(() => { return ""; }, item => { return "   " + item.ToString(); }) + guimanager.LatestInput.MoveAction.Visit<string>(() => { return ""; }, item => { return "   " + item.ToString(); }) + guimanager.LatestInput.CharacterActivity.Visit<string>(() => { return ""; }, item => { return "   " + item.ToString(); }) + guimanager.LatestInput.Settings.Visit<string>(() => { return ""; }, item => { return "   " + item.ToString(); }) + "   |";
            InputInformation.Draw(this);

            CooldownInformation.text = "Countdowns: | B: " + ((int)(guimanager.buttoncooldown)).ToString() + " | P: " + ((int)guimanager.pickupcooldown).ToString() + " | C: " + ((int)guimanager.Controls_Cooldown).ToString() + " | E " + ((int)guimanager.End_Of_Level_Cooldown).ToString() + " |";
            CooldownInformation.Draw(this);


            ScreenStats.text = ("MC " + guimanager.movementchange.ToString() + " | Scr: " + guimanager.screen.ToString() + " | Input: " + guimanager.inputmechanism.ToString()) + " | GPonl: " + guimanager.Gamepadonline.ToString() + " | IsLvl: " + guimanager.Current_screen.islevel.ToString();
            ScreenStats.Draw(this);


            LevelStats.text = ("Fallable: " + guimanager.Fallable_Objects.Count().ToString() + " | Inter: " + guimanager.Interacting_Objects.Count().ToString() + " | Stable: " + guimanager.Stable_Objects.Count().ToString() + " | Gui: " + guimanager.Gui_stuff.Count().ToString());
            LevelStats.Draw(this);

            Toilet_time_main.Fallable_Object main = guimanager.GetMain_Character();

            if (main != null)
            {
                MainInformation.text = "MainY: " + main.position.y.ToString() + " | Vel: " + ((int)(main.velocity)).ToString() + " | Baby: " + main.HasBaby.ToString() + " | Next: " + main.nextscreen.ToString();

            }

            MainInformation.Draw(this);


            PerformanceInformation.text = ((int)(1 / guimanager.drawdt)).ToString() + " fps | " + ((int)(1 / guimanager.updatedt)).ToString() + " ups";
            PerformanceInformation.Draw(this);
        }


        public void DrawBackground(Toilet_time_main.BackgroundType background)
        {
            switch (background)
            {
                case (Toilet_time_main.BackgroundType.ingamebackground):
                    {
                        spritebatch.Draw(Texture_Ingame_Background, new Rectangle(0, 0, CurrentWidth, CurrentHeight), Color.SkyBlue);
                        break;
                    }
                case (Toilet_time_main.BackgroundType.menubackground):
                    {
                        spritebatch.Draw(Texture_Menu_Background, new Rectangle(0, 0, CurrentWidth, CurrentHeight), Color.LightBlue);
                        break;
                    }
            }

        }

        private int ConvertInt(int convert)
        {
            return (int)(convert * screenmultiplier);
        }
        private Rectangle ConvertRectangle(Rectangle rect)
        {
            return new Rectangle((int)((float)(rect.X * screenmultiplier)), (int)((float)(rect.Y * screenmultiplier)), (int)((float)(rect.Width * screenmultiplier)), (int)((float)(rect.Height * screenmultiplier)));
        }

        private Vector2 ConvertVector2(Vector2 vect)
        {
            return new Vector2((int)((float)(vect.X * screenmultiplier)), (int)((float)(vect.Y * screenmultiplier)));
        }

        public void DrawSpikes(Spike spikes)
        {
            spritebatch.Draw(Texture_Spikes, ConvertRectangle(new Rectangle(spikes.position.x, spikes.position.y, spikes.size.x, spikes.size.y)), spikes.color);
        }

        public void DrawHeart(Heart heart)
        {
            spritebatch.Draw(Texture_Heart, ConvertRectangle(new Rectangle(heart.position.x, heart.position.y, heart.size.x, heart.size.y)), Color.Red);
        }
    }

}
