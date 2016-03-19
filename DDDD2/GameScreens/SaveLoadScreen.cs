using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DDDD2.GameComponents;
using System.IO;

namespace DDDD2.GameScreens
{
    public class SaveLoadScreen : GameScreen
    {
        private ScreenManager manager;
        private SpriteFont font;
        private Texture2D dialogueTexture;
        private Rectangle dialogueRectangle;
        private BackgroundComponent background, backgroundUI;
        private MenuComponent menu;
        private bool isSave;
        private bool doneRetrievingData;
        private string filename = "d4inf.dat";
        // TODO: use an enum for different screen
        private bool cameFromStartScreen;
        private string[] menuItems = { "<Empty>", "<Empty>", "<Empty>" };
        private string[] saveDataArray = { "0","0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0"};
        private bool loadGameChosen = false;
        public SaveLoadScreen(Game game, ScreenManager manager)
            : base(game)
        {
            Content = Game.Content;
            this.manager = manager;
            // TODO: put this common black rectangle some place where it can be accessed/drawn everywhere
            dialogueRectangle = new Rectangle(0, 0, Game1.Width, Game1.Height);
            isSave = false;
            cameFromStartScreen = false;
            doneRetrievingData = false;
        }

        protected override void LoadContent()
        { 
            font = Content.Load<SpriteFont>("Fonts/RegularFont");
            backgroundUI = new BackgroundComponent(
            GameRef, Content.Load<Texture2D>("Graphics/Backgrounds/SaveLoadWindow"),
            DrawMode.Fill);
            dialogueTexture = new Texture2D(Game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            dialogueTexture.SetData<Color>(new Color[] { new Color(0, 0, 0, 255) });
            menu = new MenuComponent(font, 0);
            menu.SetMenuItems(menuItems.ToList<String>());
            Vector2 menuPosition = new Vector2((float)(Game1.Width / 10), (float)(Game1.Height / 7));
            menu.SetPosition(menuPosition);
            
            base.LoadContent();
        }

        public bool IsSave
        {
            get { return isSave;  }
            set { isSave = value; }
        }

        public bool CameFromStartScreen
        {
            get { return cameFromStartScreen; }
            set { cameFromStartScreen = value; }
        }

        // todo: refactor savegame, obfuscate, loadgame, deobf
        private void SaveGame()
        {
            doneRetrievingData = false;
            FileStream stream = File.Open(filename, FileMode.OpenOrCreate);
            StreamWriter writeStream = new StreamWriter(stream);
            try
            {
                //Console.WriteLine(formatSaveData());
                writeStream.WriteLine(obfuscate(formatSaveData(), 4940));
            }
            finally
            {
                // Close the file
                writeStream.Flush();
                writeStream.Close();
                stream.Close();
            }
            Game1.audioManager.PlaySelectSound();
            RetrieveData();
        }
        // TODO: HORRIBLE way to do this. Hardcoding stats
        private string formatSaveData()
        {
            // 0 Slot1 Name, 1 Slot1 SceneId, 2 Slot1 Strength, 3 Slot1 Intelligence, 4 Slot1 Charisma, 5 Slot1 AE
            // 6 7 8 9 10 11
            // 12 13 14 15 16 17 = 6 per slot
            int startingPoint = menu.SelectedIndex*6;

            saveDataArray[startingPoint] = GamePlayScreen.HERO_NAME;
            saveDataArray[startingPoint + 1] = GameRef.gamePlayScreen.SceneId;
            saveDataArray[startingPoint + 2] = getGameStat("Strength");
            saveDataArray[startingPoint + 3] = getGameStat("Intelligence");
            saveDataArray[startingPoint + 4] = getGameStat("Charisma");
            saveDataArray[startingPoint + 5] = getGameStat("AffectionEllie");
            string returnedString = "";
            for (int i =0; i< saveDataArray.Length; i++)
            {
                returnedString += saveDataArray[i];
                if (i != saveDataArray.Length - 1)
                    returnedString += "|";
            }
            return returnedString;
        }

        private string getGameStat(string stat)
        {
            Dictionary<string, int> dict = Game1.gameInfo.getStatsDict();
            if(dict.ContainsKey(stat))
            {
                return dict[stat].ToString(); 
            }
            else
                return "0";
        }

        private string obfuscate(string source, Int16 shift)
        {
            var maxChar = Convert.ToInt32(char.MaxValue);
            var minChar = Convert.ToInt32(char.MinValue);

            var buffer = source.ToCharArray();

            for (var i = 0; i < buffer.Length; i++)
            {
                var shifted = Convert.ToInt32(buffer[i]) + shift;

                if (shifted > maxChar)
                {
                    shifted -= maxChar;
                }
                else if (shifted < minChar)
                {
                    shifted += maxChar;
                }

                buffer[i] = Convert.ToChar(shifted);
            }

            return new string(buffer);
        }

        private string deobfuscate(string source, Int16 shift)
        {
            var maxChar = Convert.ToInt32(char.MaxValue);
            var minChar = Convert.ToInt32(char.MinValue);

            var buffer = source.ToCharArray();

            for (var i = 0; i < buffer.Length; i++)
            {
                var shifted = Convert.ToInt32(buffer[i]) + shift;

                if (shifted > maxChar)
                {
                    shifted -= maxChar;
                }
                else if (shifted < minChar)
                {
                    shifted += maxChar;
                }

                buffer[i] = Convert.ToChar(shifted);
            }

            return new string(buffer);
        }

        public void RetrieveData()
        {
            if (System.IO.File.Exists(filename))
            {
                FileStream stream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Read);
                // create a reader to the stream...
                StreamReader readStream = new StreamReader(stream);
                try
                {
                    string readLine = readStream.ReadLine();
                    ParseSaveData(deobfuscate(readLine, -4940));
                }
                finally
                {
                    // Tidy up by closing the streams...
                    readStream.Close();
                    stream.Close();
                }
            }
            // TODO: can condense this into a loop
            if (!saveDataArray[0].Equals("0"))
            {
                menu.MenuItems[0] = saveDataArray[0];
            }
            if (!saveDataArray[6].Equals("0"))
            {
                menu.MenuItems[1] = saveDataArray[6];
            }
            if (!saveDataArray[12].Equals("0"))
            {
                menu.MenuItems[2] = saveDataArray[12];
            }
            getBackground();
            doneRetrievingData = true;
        }
        // TODO: better way to store and save data
        private void ParseSaveData(string saveData)
        {
            // 0 Slot1 Name, 1 Slot1 SceneId, 2 Slot1 Strength, 3 Slot1 Intelligence, 4 Slot1 Charisma, 5 Slot1 AE
            // 6 7 8 9 10 11
            // 12 13 14 15 16 17 = 6 per slot
            string[] s1 = saveData.Split('|');
            if(s1.Length == 1)
            {

            }
            else if (s1.Length == saveDataArray.Length)
            {
                for(int i=0; i < s1.Length; i++)
                {
                    if(!(s1[i].Equals("0")))
                    {
                        saveDataArray[i] = s1[i];
                    }
                }
            }
        }

        public void LoadGame()
        {
            if (menu.MenuItems[menu.SelectedIndex].Contains("Empty"))
                Game1.audioManager.PlayBuzzSound();
            else
            {
                int startingPoint = menu.SelectedIndex * 6;
                GamePlayScreen.HERO_NAME = saveDataArray[startingPoint];
                GameRef.gamePlayScreen.SceneId = saveDataArray[startingPoint + 1];
                Game1.gameInfo.getStatsDict()["Strength"] = Int32.Parse(saveDataArray[startingPoint + 2]);
                Game1.gameInfo.getStatsDict()["Intelligence"] = Int32.Parse(saveDataArray[startingPoint + 3]);
                Game1.gameInfo.getStatsDict()["Charisma"] = Int32.Parse(saveDataArray[startingPoint + 4]);
                Game1.gameInfo.getStatsDict()["AffectionEllie"] = Int32.Parse(saveDataArray[startingPoint + 5]);
                GameRef.gamePlayScreen.prepLoadedGame();
                Game1.audioManager.PlaySelectSound();
                Game1.audioManager.fadeMeOut();
                screenFader.IsFadeOut = true;
                loadGameChosen = true;
            }
        }

        private void getBackground()
        {
            if (!menu.MenuItems[menu.SelectedIndex].Contains("Empty"))
            {
                background = new BackgroundComponent(
        GameRef, Content.Load<Texture2D>("Graphics/Backgrounds/" + (Game1.gameInfo.getSceneDict()[saveDataArray[menu.SelectedIndex*6+1]]).Background),
        DrawMode.Fill);
            }
            else
            {
                background = null;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (screenFader.SwitchOK == true)
            {
                screenFader.SwitchOK = false;
                if (!loadGameChosen) // came from Escape
                {
                    if (cameFromStartScreen)
                        manager.ChangeScreens(GameRef.startScreen);
                    else
                        manager.ChangeScreens(GameRef.gamePlayScreen);
                }
                else // chose to load game
                {
                    loadGameChosen = false; // reset flag
                    manager.ChangeScreens(GameRef.gamePlayScreen);

                }
            }
            if (!screenFader.IsFadeIn && !screenFader.IsFadeOut && !Game1.audioManager.audioTransitioning() && doneRetrievingData)
            {
                menu.Update();
    
                if (InputManager.KeyReleased(Keys.Space))
                {
                    if(!isSave)
                    {
                        LoadGame();
                    }
                    else
                    {
                        SaveGame();
                    }
                }
                else if(InputManager.KeyDirectionPressed(Keys.Up) || InputManager.KeyDirectionPressed(Keys.Down))
                {
                    getBackground();
                }
                else if (InputManager.KeyReleased(Keys.Escape))
                {
                    screenFader.IsFadeOut = true;
                }
            }
            else if (!doneRetrievingData)
            {
                RetrieveData();
            }
            base.Update(gameTime);
        }



        public override void Draw(GameTime gameTime)
        {
            Game1.spriteBatch.Draw(dialogueTexture, dialogueRectangle, Color.White);
            if (background != null)
                background.Draw(Game1.spriteBatch);
            backgroundUI.Draw(Game1.spriteBatch);
            menu.Draw(Game1.spriteBatch, (int)(Game1.Height * 0.25), true);
            screenFader.Draw();
            base.Draw(gameTime);
        }
    }
}
