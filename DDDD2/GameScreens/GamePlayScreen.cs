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
        private string currentSceneId;
        private bool gameStarted;
        public GamePlayScreen(Game game, ScreenManager manager)
            : base(game)
        {
            Content = Game.Content;
            this.manager = manager;
            dialogueManager = new DialogueManager(game);
            currentSceneId = "0";
            gameStarted = false;
        }

        private Scene CurrentScene
        {
            get {  return Game1.gameInfo.getSceneDict()[currentSceneId]; }
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

        private void prepNewGame()
        {
            Game1.gameInfo.resetStats();
            gameStarted = false;
            currentSceneId = "0";
        }

        public override void Update(GameTime gameTime)
        {
            // TODO button instructions
            // TODO MENU
            if (!gameStarted)
            {
                LoadScene();
            }

            if (screenFader.SwitchOK == true)
            {
                screenFader.FadeMeIn();
                if (currentSceneId.Contains("WC") || currentSceneId.Contains("LC"))
                {
                    prepNewGame();
                    Game1.audioManager.Play("MagicalOverdrive");
                    manager.ChangeScreens(GameRef.startScreen);
                }
                else
                    LoadScene();
            }

            if (screenFader.IsFadeOut == false && screenFader.IsFadeIn == false)
            {
                dialogueManager.Update(gameTime);
                if (dialogueManager.DialogueList.Count == 0 && !dialogueManager.hasJobsLeft())
                {
                    // Based on the last dialogue job, see what the next scene is
                    if (dialogueManager.DialogueType == DialogueManager.DialogueEnum.Choice)
                    {
                        Game1.audioManager.PlaySelectSound();
                        currentSceneId = dialogueManager.ChoiceValues[dialogueManager.ChoiceMenu.SelectedIndex];
                        dialogueManager.ClearChoices();
                    }
                    else if (dialogueManager.DialogueType == DialogueManager.DialogueEnum.Normal || dialogueManager.DialogueType == DialogueManager.DialogueEnum.Attribute)
                    {
                        UpdateAttributes();
                        if (!CurrentScene.AttributeFork.Equals(""))
                        {
                            HandleAttributeFork();
                        }
                        else
                        {
                            currentSceneId = CurrentScene.NextScene;
                        }
                    }

                    screenFader.IsFadeOut = true;
                }
            }
            base.Update(gameTime);
        }

        private void HandleAttributeFork()
        {
            string[] tempArray = CurrentScene.AttributeFork.Split(' ');
            int attributeValue = Int32.Parse(tempArray[0].Trim());
            string attributeToCheck = tempArray[1].Trim();
            if (Game1.gameInfo.getStatsDict().ContainsKey(attributeToCheck))
            {
                if (Game1.gameInfo.getStatsDict()[attributeToCheck] < attributeValue)
                {
                    currentSceneId = tempArray[2].Trim();
                }
                else
                {
                    currentSceneId = tempArray[3].Trim();
                }
            }
            else
            {
                currentSceneId = tempArray[2].Trim();
            }
        }

        private void UpdateAttributes()
        {
            if (!CurrentScene.AttributeModify.Equals(""))
            {
                int attributeValue = Int32.Parse(CurrentScene.AttributeModify.Split(' ')[0].Trim());
                string attributeToModify = CurrentScene.AttributeModify.Split(' ')[1].Trim();
                if(Game1.gameInfo.getStatsDict().ContainsKey(attributeToModify))
                    Game1.gameInfo.getStatsDict()[attributeToModify] += attributeValue;
                else
                    Game1.gameInfo.getStatsDict()[attributeToModify] = attributeValue;
            }
        }

        private void LoadScene()
        {
            Console.WriteLine("Loading scene: " + currentSceneId);
            dialogueManager.ShowNPCDialogue(CurrentScene.MainDialogue, DialogueManager.DialogueEnum.Normal);
            if (!CurrentScene.Choices.Equals(""))
            {
                dialogueManager.ShowNPCDialogue(CurrentScene.Choices, DialogueManager.DialogueEnum.Choice);
            }
            if (!CurrentScene.AttributeModify.Equals(""))
            {
                dialogueManager.ShowNPCDialogue(CurrentScene.AttributeModify, DialogueManager.DialogueEnum.Attribute);
            }
            gameStarted = true;

        }

        public override void Draw(GameTime gameTime)
        {
            background.Draw(Game1.spriteBatch);
            dialogueManager.Draw(Game1.spriteBatch);
            base.Draw(gameTime);
        }
    }
}
