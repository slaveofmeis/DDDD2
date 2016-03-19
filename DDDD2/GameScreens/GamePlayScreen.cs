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
using Microsoft.Xna.Framework.Media;
namespace DDDD2.GameScreens
{
    public class GamePlayScreen : GameScreen
    {
        ScreenManager manager;
        //SpriteFont font;
        BackgroundComponent background;
        private Texture2D spriteTexture;
        public static string HERO_NAME = "Alan";
        public static string MC_IDENTIFIER = "[MC]";
        public static DialogueManager dialogueManager;
        private string currentSceneId;
        private bool gameStarted;
        private Texture2D menuTexture;
        private Rectangle menuRectangle;
        private bool menuIsActivated;
        private MenuComponent menu;
        private SpriteFont menuFont;
        private string[] menuItems = { "Save Game", "Load Game", "Main Menu", "Return to Game" };
        public GamePlayScreen(Game game, ScreenManager manager)
            : base(game)
        {
            Content = Game.Content;
            this.manager = manager;
            dialogueManager = new DialogueManager(game);
            currentSceneId = "0";
            gameStarted = false;
            menuIsActivated = false;
        }

        public void setHeroName(string name)
        {
            HERO_NAME = name;
        }

        public Scene CurrentScene
        {
            get {  return Game1.gameInfo.getSceneDict()[currentSceneId]; }
        }

        public string SceneId
        {
            get { return currentSceneId; }
            set { currentSceneId = value; }
        }

        protected override void LoadContent()
        {
            //font = Content.Load<SpriteFont>("Fonts/RegularFont");
            dialogueManager.LoadContent();
            gameStarted = true;
            menuFont = Content.Load<SpriteFont>("Fonts/RegularFont");
            menuTexture = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            menuTexture.SetData<Color>(new Color[] { new Color(0, 0, 0, 140) });
            menu = new MenuComponent(menuFont, 0);
            menuRectangle = new Rectangle(0, 0, Game1.Width, Game1.Height);
            
            menu.SetMenuItems(menuItems.ToList<String>());
            Vector2 menuPosition = new Vector2((float)(Game1.Width / 2 - menu.Width / 2), (float)(Game1.Height / 2 - menu.Height / 2));
            menu.SetPosition(menuPosition);
            LoadScene();
            base.LoadContent();
        }

        public void prepNewGame()
        {
            Game1.gameInfo.resetStats();
            gameStarted = false;
            currentSceneId = "0";
            dialogueManager.ClearDialogueManager();
        }

        public void prepLoadedGame()
        {
            gameStarted = false;
            dialogueManager.ClearDialogueManager();
        }

        private void handleTransition()
        {
            if (currentSceneId.Contains("WC") || currentSceneId.Contains("LC"))
            {
                //prepNewGame();
                //Game1.audioManager.fadeMeOut();
                manager.ChangeScreens(GameRef.endingScreen);
            }
            else
                LoadScene();
        }

        public override void Update(GameTime gameTime)
        {
            // TODO button instructions
            // TODO MENU
            if(!gameStarted)
            {
                handleTransition();
            }
            if (screenFader.SwitchOK == true)
            {
                screenFader.FadeMeIn();
                handleTransition();
            }
            
            if (screenFader.IsFadeOut == false && screenFader.IsFadeIn == false && !Game1.audioManager.audioTransitioning())
            {
                if (InputManager.KeyReleased(Keys.Escape) && dialogueManager.DialogueList.Count != 0 && dialogueManager.hasJobsLeft())
                {
                    menuIsActivated = !menuIsActivated;
                    menu.SelectedIndex = 0;

                }
                if (!menuIsActivated)
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
                }
                else
                {
                    menu.Update();
                    if (InputManager.KeyReleased(Keys.Space))
                    {
                        Game1.audioManager.PlaySelectSound();
                        switch (menu.SelectedIndex)
                        {
                            case 0:
                                manager.ChangeScreens(GameRef.saveLoadScreen);
                                GameRef.saveLoadScreen.CameFromStartScreen = false;
                                GameRef.saveLoadScreen.IsSave = true;
                                break;
                            case 1:
                                manager.ChangeScreens(GameRef.saveLoadScreen);
                                GameRef.saveLoadScreen.CameFromStartScreen = false;
                                GameRef.saveLoadScreen.IsSave = false;
                                break;
                            case 2:
                                menuIsActivated = false;
                                manager.ChangeScreens(GameRef.startScreen);
                                break;
                            case 3:
                                menuIsActivated = false;
                                break;

                        }
                    }
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
            //Console.WriteLine("Loading scene: " + currentSceneId);
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
                //Console.WriteLine("Playing: " + CurrentScene.Music);
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
            menuIsActivated = false;
        }

        public override void Draw(GameTime gameTime)
        {
            background.Draw(Game1.spriteBatch);
            if(spriteTexture != null) // !CurrentScene.Sprite.Equals("") && 
            {
                Game1.spriteBatch.Draw(spriteTexture, Vector2.Zero, Color.White);
            }
            //dialogueManager.Draw(Game1.spriteBatch);
            if (menuIsActivated)
            {
                Game1.spriteBatch.Draw(menuTexture, menuRectangle, Color.White);
                menu.Draw(Game1.spriteBatch, (int)(Game1.Height * 0.070), true);
            }
            else
                dialogueManager.Draw(Game1.spriteBatch);
            base.Draw(gameTime);
        }
    }
}
