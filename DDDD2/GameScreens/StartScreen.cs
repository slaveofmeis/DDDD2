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
        private BackgroundComponent background, background2;
        //texture = Content.Load<Texture2D>("Graphics/Backgrounds/nagi-no-asukara-miuna-mother");
        private MenuComponent menu;
        private string[] menuItems = { "New Game", "Load Game", "Exit" };
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
            GameRef, Content.Load<Texture2D>("Graphics/Backgrounds/nagi-no-asukara-miuna-mother"),
            DrawMode.Fill);
            Game1.audioManager.Play("MagicalOverdrive");

            menu = new MenuComponent(font, 0);
            Vector2 menuPosition = new Vector2((Game1.Width - menu.Width) / 2,(Game1.Height - menu.Height) / 2);
            menu.SetPostion(menuPosition);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            menu.SetMenuItems(menuItems.ToList<String>());
            if (!screenFader.IsFadeIn && !screenFader.IsFadeOut)
            {
                menu.Update();
                if (InputManager.KeyReleased(Keys.Enter))
                {
                    switch (menu.SelectedIndex)
                    {
                        case 0:
                            Game1.audioManager.PlayMapSwitch("the_field_of_dreams");
                            Game1.audioManager.PlaySelectSound();
                            manager.ChangeScreens(GameRef.gamePlayScreen);
                            //manager.ChangeScreens(GameRef.nameHeroScreen);
                            break;
                        case 1:
                            
                            //GameRef.saveLoadScreen.ClearFlag = true;
                            //GameRef.saveLoadScreen.SaveContext = false;
                            //GameRef.saveLoadScreen.StartScreenContext = true;
                            Game1.audioManager.PlaySelectSound();
                            //manager.ChangeScreens(GameRef.saveLoadScreen);

                            break;
                        case 2:
                            Game.Exit();
                            break;
                    }
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

        public override void Draw(GameTime gameTime)
        {
            background.Draw(Game1.spriteBatch);
            Game1.spriteBatch.DrawString(font, "StartScreen", new Vector2(100,100), Color.Yellow);
            menu.Draw(Game1.spriteBatch, font.LineSpacing, true);
            base.Draw(gameTime);
        }

        public int gotExit()
        {
            return menu.getExitValue;
        }
    }
}
