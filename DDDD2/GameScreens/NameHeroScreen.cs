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
    public class NameHeroScreen : GameScreen
    {
        private ScreenManager manager;
        private SpriteFont font;
        private BackgroundComponent background;
        private BlockMenuComponent menu;
        private string shownName;
        private bool needReset;
        private Vector2 shownNamePosition;
        private string[] menuItems = { "A", "B", "C", "D", "E", "F", "G", "[CAPS]", "H", "I", "J", "K", "L", "M", "N", "[DEL]", "O", "P", "Q", "R", "S", "T", "U", "[DONE]", "V", "W", "X", "Y", "Z" };
        public NameHeroScreen(Game game, ScreenManager manager)
            : base(game)
        {
            Content = Game.Content;
            this.manager = manager;
            needReset = true;
            shownName = "";
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("Fonts/RegularFont");
            background = new BackgroundComponent(
            GameRef, Content.Load<Texture2D>("Graphics/Backgrounds/NamingScreen"),
            DrawMode.Fill);
            menu = new BlockMenuComponent(font, 0, 8);
            Vector2 menuPosition = new Vector2((float)((Game1.Width - menu.Width) * 0.11), (float)((Game1.Height - menu.Height) * 0.40));
            menu.SetPosition(menuPosition);
            shownNamePosition = new Vector2(menuPosition.X, menuPosition.Y - (float)(Game1.Height* 0.14));
            menu.SetMenuItems(menuItems.ToList<String>());
            base.LoadContent();
        }

        public void prepNewGame()
        {
            shownName = "";
            menu.SelectedIndex = 0;
            menu.IsCaps = true;
            needReset = false;
        }

        public void reset()
        {
            needReset = true;
        }

        public override void Update(GameTime gameTime)
        {
            if(needReset)
            {
                prepNewGame();
            }
            if (!screenFader.IsFadeIn && !screenFader.IsFadeOut && !Game1.audioManager.audioTransitioning())
            {
                if (InputManager.KeyReleased(Keys.Space))
                {
                    if (menuItems[menu.SelectedIndex].Contains("CAPS"))
                    {
                        menu.IsCaps = !menu.IsCaps;
                        Game1.audioManager.PlaySelectSound();
                    }
                    else if (menuItems[menu.SelectedIndex].ToUpper().Contains("DEL"))
                    {
                        if (shownName.Length > 0)
                        {
                            shownName = shownName.Substring(0, shownName.Length - 1);
                            Game1.audioManager.PlaySelectSound();
                        }
                        else
                            Game1.audioManager.PlayBuzzSound();
                    }
                    else if (menuItems[menu.SelectedIndex].Contains("DONE"))
                    {
                        GameRef.gamePlayScreen.setHeroName(shownName);
                        Game1.audioManager.PlaySelectSound();
                        //Game1.audioManager.fadeMeOut();
                        GameRef.gamePlayScreen.prepNewGame();
                        manager.ChangeScreens(GameRef.gamePlayScreen);
                    }
                    else
                    {
                        if (shownName.Length > 10)
                        {
                            Game1.audioManager.PlayBuzzSound();
                        }
                        else
                        {
                            if (!menu.IsCaps)
                                shownName += menuItems[menu.SelectedIndex].ToLower();
                            else
                                shownName += menuItems[menu.SelectedIndex];
                            Game1.audioManager.PlaySelectSound();
                        }
                    }
                }
                menu.Update();
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            background.Draw(Game1.spriteBatch);
            //Game1.spriteBatch.DrawString(font, "StartScreen", new Vector2(100,100), Color.Yellow);
            menu.Draw(Game1.spriteBatch, (int)(Game1.Height * 0.060), true, false);
            Game1.spriteBatch.DrawString(font, shownName, shownNamePosition, Color.White);
            base.Draw(gameTime);
        }

    }
}
