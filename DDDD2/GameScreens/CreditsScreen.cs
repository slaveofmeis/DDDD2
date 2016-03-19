using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DDDD2.GameComponents;
namespace DDDD2.GameScreens
{
    public class CreditsScreen : GameScreen
    {
        private ScreenManager manager;
        private BackgroundComponent background;
        public CreditsScreen(Game game, ScreenManager manager)
            : base(game)
        {
            Content = Game.Content;
            this.manager = manager;
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            background = new BackgroundComponent(
            GameRef, Content.Load<Texture2D>("Graphics/Backgrounds/CreditsScreen"),
            DrawMode.Fill);
        }



        public override void Update(GameTime gameTime)
        {
            if (screenFader.SwitchOK == true)
            {
                screenFader.SwitchOK = false;
                manager.ChangeScreens(GameRef.startScreen);
            }
            if (!screenFader.IsFadeIn && !screenFader.IsFadeOut && !Game1.audioManager.audioTransitioning())
            {
                if (InputManager.KeyReleased(Keys.Space) || InputManager.KeyReleased(Keys.Escape))
                {
                    Game1.audioManager.fadeMeOut();
                    screenFader.IsFadeOut = true;
                }
                //manager.ChangeScreens(GameRef.startScreen);


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
