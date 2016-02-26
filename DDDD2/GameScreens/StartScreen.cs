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
        ScreenManager manager;
        SpriteFont font;
        BackgroundComponent background, background2;
        //texture = Content.Load<Texture2D>("Graphics/Backgrounds/nagi-no-asukara-miuna-mother");
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
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (screenFader.SwitchOK == true)
            {
                manager.ChangeScreens(GameRef.gamePlayScreen);
            }
            if (InputManager.KeyReleased(Keys.A) && screenFader.IsFadeOut == false && screenFader.IsFadeIn == false)
            {
                screenFader.IsFadeOut = true;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            background.Draw(Game1.spriteBatch);
            Game1.spriteBatch.DrawString(font, "StartScreen", new Vector2(100,100), Color.Yellow);
            base.Draw(gameTime);
        }

        public int gotExit()
        {
            return 0;
            //return menu.getExitValue;
        }
    }
}
