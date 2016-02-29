using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DDDD2.GameComponents;
using DDDD2.GameInformation;
namespace DDDD2.GameScreens
{
    public class GamePlayScreen : GameScreen
    {
        ScreenManager manager;
        SpriteFont font;
        BackgroundComponent background;
        public static string HERO_NAME = "Alan";
        public static DialogueManager dialogueManager;
        private int currentSceneNumber;
        private Scene currentScene;
        private bool sceneStarted;
        public GamePlayScreen(Game game, ScreenManager manager)
            : base(game)
        {
            Content = Game.Content;
            this.manager = manager;
            dialogueManager = new DialogueManager(game);
            currentSceneNumber = 0;
            sceneStarted = false;
        }

        protected override void LoadContent()
        {
            font = Content.Load<SpriteFont>("Fonts/RegularFont");
            background = new BackgroundComponent(
            GameRef, Content.Load<Texture2D>("Graphics/Backgrounds/truetearscar"),
            DrawMode.Fill);
            dialogueManager.LoadContent();
            currentScene = Game1.gameInfo.getSceneDict()[currentSceneNumber];
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            // TODO button instructions
            // TODO MENU
            if (!sceneStarted)
            {
                dialogueManager.ShowNPCDialogue("Barnsy", currentScene.MainDialogue, false);
                dialogueManager.ShowNPCDialogue("Barnsy", "Gained 1", true);
                sceneStarted = true;
            }

            if (screenFader.SwitchOK == true)
            {
                // Load next scene assets HERE
                // tell screenFader to fade you in
                // tell sceneStarted = false;
                manager.ChangeScreens(GameRef.startScreen);
            }

            if (screenFader.IsFadeOut == false && screenFader.IsFadeIn == false)
            {
                dialogueManager.Update(gameTime);

                if (InputManager.KeyReleased(Keys.A) && !dialogueManager.hasJobsLeft())
                {
                    Game1.audioManager.PlayMapSwitch("MagicalOverdrive");
                    screenFader.IsFadeOut = true;
                }
                if (InputManager.KeyReleased(Keys.B))
                {

                }
                if (InputManager.KeyReleased(Keys.C))
                {
                }
            }
            base.Update(gameTime);
        }

        private void LoadScene(int sceneNumber)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            background.Draw(Game1.spriteBatch);
            dialogueManager.Draw(Game1.spriteBatch);
            base.Draw(gameTime);
        }
    }
}
