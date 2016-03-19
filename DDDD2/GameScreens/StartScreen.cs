using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DDDD2.GameComponents;

namespace DDDD2.GameScreens
{
    public class StartScreen : GameScreen
    {
        private ScreenManager manager;
        private SpriteFont font;
        private BackgroundComponent background;
        private MenuComponent menu;
        private string[] menuItems = { "", "", "", "", "" };
        private Texture2D speakerTexture, speakerTexture0, speakerTexture25, speakerTexture50, speakerTexture75, speakerTexture100;
        public StartScreen(Game game, ScreenManager manager)
            : base(game)
        {
            Content = Game.Content;
            this.manager = manager;
            
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("Fonts/RegularFont");
            background = new BackgroundComponent(
            GameRef, Content.Load<Texture2D>("Graphics/Backgrounds/StartScreen"),
            DrawMode.Fill);
            speakerTexture0 = Content.Load<Texture2D>("Graphics/Backgrounds/Speaker0");
            speakerTexture25 = Content.Load<Texture2D>("Graphics/Backgrounds/Speaker25");
            speakerTexture50 = Content.Load<Texture2D>("Graphics/Backgrounds/Speaker50");
            speakerTexture75 = Content.Load<Texture2D>("Graphics/Backgrounds/Speaker75");
            speakerTexture100 = Content.Load<Texture2D>("Graphics/Backgrounds/Speaker100");
            

            menu = new MenuComponent(font, 0);
            Vector2 menuPosition = new Vector2((float)((Game1.Width - menu.Width) *0.315),(float)((Game1.Height - menu.Height)*0.57));
            menu.SetPosition(menuPosition);
            menu.SetMenuItems(menuItems.ToList<String>());
            base.LoadContent();
        }



        public override void Update(GameTime gameTime)
        {
            //menu.SetMenuItems(menuItems.ToList<String>());
            if (!screenFader.IsFadeIn && !screenFader.IsFadeOut) // && !Game1.audioManager.audioTransitioning())
            {
                Game1.audioManager.Play("CasualBGM");
                menu.Update();
                if (InputManager.KeyReleased(Keys.Space))
                {
                    switch (menu.SelectedIndex)
                    {
                        case 0:
                            Game1.audioManager.PlaySelectSound();
                            //screenFader.IsFadeOut = true;
                            GameRef.nameHeroScreen.reset();
                            manager.ChangeScreens(GameRef.nameHeroScreen);
                            //manager.ChangeScreens(GameRef.nameHeroScreen);
                            break;
                        case 1:
                            Game1.audioManager.PlaySelectSound();
                            GameRef.saveLoadScreen.IsSave = false;
                            GameRef.saveLoadScreen.CameFromStartScreen = true;
                            manager.ChangeScreens(GameRef.saveLoadScreen);

                            break;
                        case 2:
                            Game1.audioManager.PlaySelectSound();
                            manager.ChangeScreens(GameRef.creditsScreen);
                            break;
                        case 4:
                            Game.Exit();
                            break;

                    }
                }
                if (InputManager.KeyReleased(Keys.Left) && menu.SelectedIndex == 3)
                {
                    Game1.audioManager.decrementMaxVolume();
                    //Console.WriteLine(Game1.audioManager.maxVolumeSetting);
                    Game1.audioManager.PlayCaChingSound();
                }
                else if (InputManager.KeyReleased(Keys.Right) && menu.SelectedIndex == 3)
                {
                    Game1.audioManager.incrementMaxVolume();
                    //Console.WriteLine(Game1.audioManager.maxVolumeSetting);
                    Game1.audioManager.PlayCaChingSound();
                }
            }
            
            /*if (screenFader.SwitchOK == true)
            {
                manager.ChangeScreens(GameRef.gamePlayScreen);
            }
            if (InputManager.KeyReleased(Keys.A) && screenFader.IsFadeOut == false && screenFader.IsFadeIn == false)
            {
                Game1.audioManager.PlayMapSwitch("the_field_of_dreams");
                screenFader.IsFadeOut = true;
            }*/

            base.Update(gameTime);
        }

        // TODO: This way of doing things sucks
        private void HandleSpeakerDraw()
        {
            if (Game1.audioManager.maxVolumeSetting == 0f)
            {
                speakerTexture = speakerTexture0;
            }
            else if (Game1.audioManager.maxVolumeSetting == 0.25f)
            {
                speakerTexture = speakerTexture25;
            }
            else if (Game1.audioManager.maxVolumeSetting == 0.50f)
            {
                speakerTexture = speakerTexture50;
            }
            else if (Game1.audioManager.maxVolumeSetting == 0.75f)
            {
                speakerTexture = speakerTexture75;
            }
            else if (Game1.audioManager.maxVolumeSetting == 1f)
            {
                speakerTexture = speakerTexture100;
            }
            Game1.spriteBatch.Draw(speakerTexture, Vector2.Zero, Color.White);
        }

        public override void Draw(GameTime gameTime)
        {
            background.Draw(Game1.spriteBatch);
            //Game1.spriteBatch.DrawString(font, "StartScreen", new Vector2(100,100), Color.Yellow);
            menu.Draw(Game1.spriteBatch, (int)(Game1.Height*0.065), true);
            HandleSpeakerDraw();
            base.Draw(gameTime);
        }

        public int gotExit()
        {
            return menu.getExitValue;
        }
    }
}
