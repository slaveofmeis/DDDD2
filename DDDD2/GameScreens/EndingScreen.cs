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
    public class EndingScreen : GameScreen
    {
        private ScreenManager manager;
        private SpriteFont font;
        private Texture2D dialogueTexture;
        private Rectangle dialogueRectangle;
        private string ENDING_TEXT = "The End";
        private Vector2 endTextPosition;
        public EndingScreen(Game game, ScreenManager manager)
            : base(game)
        {
            Content = Game.Content;
            this.manager = manager;
            dialogueRectangle = new Rectangle(0, 0, Game1.Width, Game1.Height);
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("Fonts/RegularFont");
            dialogueTexture = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            dialogueTexture.SetData<Color>(new Color[] { new Color(0, 0, 0, 255) });
            screenFader.TransitionSpeed = ScreenFader.SLOW_SPEED;
            endTextPosition = new Vector2(Game1.Width / 2 - font.MeasureString(ENDING_TEXT).X / 2, Game1.Height / 2 - font.MeasureString(ENDING_TEXT).Y / 2);
            base.LoadContent();
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

                        Game1.audioManager.fadeMeOutSlow();
                        screenFader.IsFadeOut = true;
                        //manager.ChangeScreens(GameRef.startScreen);
                

            }
            base.Update(gameTime);
        }

        

        public override void Draw(GameTime gameTime)
        {
            Game1.spriteBatch.Draw(dialogueTexture, dialogueRectangle, Color.White);
            Game1.spriteBatch.DrawString(font, ENDING_TEXT, endTextPosition, Color.White);
            base.Draw(gameTime);
        }


    }
}
