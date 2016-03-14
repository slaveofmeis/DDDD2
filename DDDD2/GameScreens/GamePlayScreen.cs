using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DDDD2.GameComponents;
using DDDD2.GameInformation;
using System.IO;
using System.IO.IsolatedStorage;
namespace DDDD2.GameScreens
{
    public class GamePlayScreen : GameScreen
    {
        ScreenManager manager;
        SpriteFont font;
        BackgroundComponent background;
        private Texture2D spriteTexture;
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
            dialogueManager.LoadContent();
            gameStarted = true;
            LoadScene();
            base.LoadContent();
        }

        private void prepNewGame()
        {
            Game1.gameInfo.resetStats();
            gameStarted = false;
            currentSceneId = "0";
        }

        private void handleTransition()
        {
            if (currentSceneId.Contains("WC") || currentSceneId.Contains("LC"))
            {
                prepNewGame();
                Game1.audioManager.fadeMeOut();
                manager.ChangeScreens(GameRef.startScreen);
            }
            else
                LoadScene();
        }

        public override void Update(GameTime gameTime)
        {
            // TODO button instructions
            // TODO MENU

            if (screenFader.SwitchOK == true || !gameStarted)
            {
                screenFader.FadeMeIn();
                handleTransition();
            }

            if (screenFader.IsFadeOut == false && screenFader.IsFadeIn == false)
            {
                dialogueManager.Update(gameTime);
                if (dialogueManager.DialogueList.Count == 0 && !dialogueManager.hasJobsLeft())
                {
                    string savedBackground = CurrentScene.Background;
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
                    if (!currentSceneId.Contains("WC") && !currentSceneId.Contains("LC"))
                    {
                        if (savedBackground.Equals(CurrentScene.Background))
                        {
                            handleTransition();
                        }
                        else
                            screenFader.IsFadeOut = true;
                    }
                    else
                        screenFader.IsFadeOut = true;
                }
                if (InputManager.KeyReleased(Keys.G))
                {
                    SaveGame();
                }
                if (InputManager.KeyReleased(Keys.H))
                {
                    LoadGame();
                }
            }
            base.Update(gameTime);
        }

        private void SaveGame()
        {
            string filename = "test.dat";
            FileStream stream=File.Open(filename, FileMode.OpenOrCreate); 
            StreamWriter writeStream= new StreamWriter(stream);
            try
            {
                writeStream.WriteLine("ABC");
            }
            finally
            {
                // Close the file
                writeStream.Flush();
                writeStream.Close();
                stream.Close();
            }
            
        }

        public void LoadGame()
        {
            string filename = "test.dat";
            if (System.IO.File.Exists(filename))
            {
                FileStream stream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Read);
                // create a reader to the stream...
                StreamReader readStream = new StreamReader(stream);
                try
                {
                    Console.WriteLine(readStream.ReadLine());
                }
                finally
                {
                    // Tidy up by closing the streams...
                    readStream.Close();
                    stream.Close();
                }
            }
            else
            {
                // BUZZ
            }

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
            background = new BackgroundComponent(
            GameRef, Content.Load<Texture2D>("Graphics/Backgrounds/" + CurrentScene.Background),
            DrawMode.Fill);
            dialogueManager.ShowNPCDialogue(CurrentScene.MainDialogue, DialogueManager.DialogueEnum.Normal);
            if (!CurrentScene.Choices.Equals(""))
            {
                dialogueManager.ShowNPCDialogue(CurrentScene.Choices, DialogueManager.DialogueEnum.Choice);
            }
            if (!CurrentScene.AttributeModify.Equals(""))
            {
                dialogueManager.ShowNPCDialogue(CurrentScene.AttributeModify, DialogueManager.DialogueEnum.Attribute);
            }
            if (!CurrentScene.Music.Equals(""))
            {
                Game1.audioManager.PlayMapSwitch(CurrentScene.Music);
                Console.WriteLine("Playing: " + CurrentScene.Music);
            }
            else
            {
                Game1.audioManager.fadeMeOut();
            }
            if (!CurrentScene.Sprite.Equals(""))
            {
                spriteTexture = Content.Load<Texture2D>("Graphics/Sprites/" + CurrentScene.Sprite);
            }
            else
                spriteTexture = null;
            gameStarted = true;

        }

        public override void Draw(GameTime gameTime)
        {
            background.Draw(Game1.spriteBatch);
            if(spriteTexture != null) // !CurrentScene.Sprite.Equals("") && 
            {
                Game1.spriteBatch.Draw(spriteTexture, Vector2.Zero, Color.White);
            }
            dialogueManager.Draw(Game1.spriteBatch);
            base.Draw(gameTime);
        }
    }
}
