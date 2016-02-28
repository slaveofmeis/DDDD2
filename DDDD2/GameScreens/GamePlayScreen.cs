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
        public static string HERO_NAME = "Alan";
        public static DialogueManager dialogueManager;


        public GamePlayScreen(Game game, ScreenManager manager)
            : base(game)
        {
            Content = Game.Content;
            this.manager = manager;
            dialogueManager = new DialogueManager(game);
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("Fonts/RegularFont");
            background = new BackgroundComponent(
            GameRef, Content.Load<Texture2D>("Graphics/Backgrounds/truetearscar"),
            DrawMode.Fill);
            dialogueManager.LoadContent();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (screenFader.SwitchOK == true)
            {
                manager.ChangeScreens(GameRef.startScreen);
            }

            if (screenFader.IsFadeOut == false && screenFader.IsFadeIn == false)
            {
                dialogueManager.Update(gameTime);
                if (InputManager.KeyReleased(Keys.A) && dialogueManager.DialogueCount == 0)
                {
                    Game1.audioManager.PlayMapSwitch("MagicalOverdrive");
                    screenFader.IsFadeOut = true;
                }
                if (InputManager.KeyReleased(Keys.B))
                {
                    dialogueManager.ShowNPCDialogue("Barnsy", "hello. this is test.", false);
                }
                if (InputManager.KeyReleased(Keys.C))
                {
                    dialogueManager.ShowNPCDialogue("Barnsy", "Gained 1", true);
                    //dialogueManager.ShowEventDialogue("hello. this is a test^please refrain from being fdsaaaddddddddddddddddddddddddddddddddddddd^ THIS IS A TES TI REPAT A TEST!!", 0);
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            background.Draw(Game1.spriteBatch);
            dialogueManager.Draw(Game1.spriteBatch);
            base.Draw(gameTime);
        }
    }
}
