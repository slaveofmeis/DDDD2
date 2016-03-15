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
        private MenuComponent menu;
        private string[] menuItems = { "A", "B", "C", "D", "E" };
        public NameHeroScreen(Game game, ScreenManager manager)
            : base(game)
        {
            Content = Game.Content;
            this.manager = manager;
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("Fonts/RegularFont");
            background = new BackgroundComponent(
            GameRef, Content.Load<Texture2D>("Graphics/Backgrounds/NamingScreen"),
            DrawMode.Fill);

            menu = new MenuComponent(font, 0);
            Vector2 menuPosition = new Vector2((float)((Game1.Width - menu.Width) * 0.31), (float)((Game1.Height - menu.Height) * 0.61));
            menu.SetPosition(menuPosition);
            menu.SetMenuItems(menuItems.ToList<String>());
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!screenFader.IsFadeIn && !screenFader.IsFadeOut && !Game1.audioManager.audioTransitioning())
            {
                menu.Update();

            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            background.Draw(Game1.spriteBatch);
            //Game1.spriteBatch.DrawString(font, "StartScreen", new Vector2(100,100), Color.Yellow);
            menu.Draw(Game1.spriteBatch, (int)(Game1.Height * 0.060), true);
            base.Draw(gameTime);
        }

    }
}
