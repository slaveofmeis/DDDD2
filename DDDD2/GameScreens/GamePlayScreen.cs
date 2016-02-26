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
    public class GamePlayScreen : GameScreen
    {
        ScreenManager manager;
        SpriteFont font;
        BackgroundComponent background;
        public GamePlayScreen(Game game, ScreenManager manager)
            : base(game)
        {
            Content = Game.Content;
            this.manager = manager;
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("Fonts/RegularFont");
            background = new BackgroundComponent(
            GameRef, Content.Load<Texture2D>("Graphics/Backgrounds/truetearscar"),
            DrawMode.Fill);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (screenFader.SwitchOK == true)
            {
                manager.ChangeScreens(GameRef.startScreen);
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
            base.Draw(gameTime);
        }
    }
}
